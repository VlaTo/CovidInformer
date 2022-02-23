using System;
using System.Threading.Tasks;

namespace CovidInformer.Core
{
    public sealed class TaskExecutor
    {
        private Task task;

        public TaskExecutor()
        {
            task = null;
        }

        public bool TryRun(Func<Task> func)
        {
            if (null != task)
            {
                return false;
            }

            task = Task.Factory.StartNew(() => ExecuteAsync(func), TaskCreationOptions.LongRunning);

            return true;
        }

        private async Task ExecuteAsync(Func<Task> func)
        {
            try
            {
                var temp = func.Invoke();

                await temp;
            }
            finally
            {
                task = null;
            }
        }
    }
}