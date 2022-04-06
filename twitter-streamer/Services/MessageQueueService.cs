using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TwitterStreamer.Services.Interfaces;

namespace TwitterStreamer.Services
{

    public class MessageQueueService : IMessageQueue
    {
        private ConcurrentQueue<Func<CancellationToken, Task>> _workItems = new ConcurrentQueue<Func<CancellationToken, Task>>();
        private readonly List<Task> _runItems = new List<Task>();
        private readonly SemaphoreSlim _signal;
        private const int InitialCount = 10;

        public MessageQueueService()
        {
            _signal = new SemaphoreSlim(InitialCount);
        }


        public void QueueWorkItem(Func<CancellationToken, Task> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            _workItems.Enqueue(workItem);
            _signal.Release();
        }

        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            if (_workItems.IsEmpty) return async token => await Task.CompletedTask;
            var removed = _runItems.RemoveAll(x => x.IsCompleted);
            while (!_workItems.IsEmpty && _runItems.Count < InitialCount)
            {
                _workItems.TryDequeue(out var workItem);
                if (workItem != null)
                {
                    _runItems.Add(Task.Run(() => workItem(cancellationToken), cancellationToken));
                }
            }
            return async token => await Task.WhenAny(_runItems);
        }
    }
}
