using System;
using System.Threading.Tasks;

namespace CovidInformer.Core
{
    public interface ITaskQueue
    {
        void EnqueueTask(Func<Task> func);
    }
}