using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CovidInformer.Core
{
    public sealed class TaskQueue : ITaskQueue
    {
        private static TaskQueue instance;

        private readonly BlockingCollection<Func<Task>> queue;
        private Task executionTask;

        public static TaskQueue GetInstance()
        {
            if (null == instance)
            {
                instance = new TaskQueue();
            }

            return instance;
        }

        public TaskQueue()
        {
            queue = new BlockingCollection<Func<Task>>();
            executionTask = null;
        }

        public void Initialize(CancellationToken cancellationToken = default)
        {
            if (null == executionTask)
            {
                executionTask = Task.Factory.StartNew(DoWork, cancellationToken, TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);
            }
        }

        public void EnqueueTask(Func<Task> func)
        {
            if (false == queue.IsAddingCompleted)
            {
                queue.Add(func);
            }
        }

        private async Task DoWork()
        {
            try
            {
                foreach (var func in queue.GetConsumingEnumerable(CancellationToken.None))
                {
                    var task = func.Invoke();
                    await task;
                }
            }
            catch (OperationCanceledException exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                ;
            }
        }
    }
}