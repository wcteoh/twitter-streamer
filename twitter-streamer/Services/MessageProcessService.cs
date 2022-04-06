using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TwitterStreamer.Services.Interfaces;

namespace TwitterStreamer.Services
{
    public class MessageProcessService : BackgroundService
    {
        private readonly ILogger<MessageProcessService> _logger;
        private readonly IMessageTotalService _message;
        private readonly IMessageQueue _messageQueue;

        public MessageProcessService(ILogger<MessageProcessService> logger,
            IMessageQueue messageQueue)
        {
            _logger = logger;
            _messageQueue = messageQueue;
        }

        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var workItem = await _messageQueue.DequeueAsync(cancellationToken);
                try
                {
                    if (workItem != null)
                    {
                        await workItem(cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error occurred executing {nameof(workItem)}.");
                }
            }
        }
    }
}
