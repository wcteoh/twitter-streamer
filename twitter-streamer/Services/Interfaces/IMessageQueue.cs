using System;
using System.Threading;
using System.Threading.Tasks;

namespace TwitterStreamer.Services.Interfaces
{
    public interface IMessageQueue
    {
        void QueueWorkItem(Func<CancellationToken, Task> workItem);
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
