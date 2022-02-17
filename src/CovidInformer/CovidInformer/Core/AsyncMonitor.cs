using System;
using System.Threading.Tasks;

namespace CovidInformer.Core
{
    internal sealed class AsyncMonitor
    {
        private readonly object mutex;
        private TaskCompletionSource<bool> awaiter;

        public AsyncMonitor()
        {
            mutex = new object();
            awaiter = null;
        }

        public void Pulse()
        {
            var tcs = awaiter;

            if (null == tcs)
            {
                return;
            }

            lock (mutex)
            {
                tcs = awaiter;

                if (null == tcs)
                {
                    return;
                }

                awaiter = null;

                tcs.TrySetResult(true);
            }
        }

        public Task WaitAsync(TimeSpan timeout)
        {
            var w = awaiter;
            
            if (null != w)
            {
                return Task.WhenAny(w.Task, Task.Delay(timeout));
            }

            lock (mutex)
            {
                w = awaiter;

                if (null != w)
                {
                    return w.Task;
                }

                w = awaiter = new TaskCompletionSource<bool>();

                return Task.WhenAny(w.Task, Task.Delay(timeout));
            }
        }

        public Task WaitAsync()
        {
            var w = awaiter;
            
            if (null != w)
            {
                return w.Task;
            }

            lock (mutex)
            {
                w = awaiter;

                if (null != w)
                {
                    return w.Task;
                }

                w = awaiter = new TaskCompletionSource<bool>();

                return w.Task;
            }
        }
    }
}