using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TwitterStreamer.Models;
using TwitterStreamer.Services.Interfaces;

namespace TwitterStreamer.Services
{
    public class TwitterSampleStreamService : BackgroundService
    {
        ///public IOrderCompletedTaskQueue TaskQueue { get; }

        private readonly ILogger<TwitterSampleStreamService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMessageTotalService _messageService;
        private readonly IHashTagCounterService _hashtagService;
        private readonly IMessageQueue _messageQueue;

        public TwitterSampleStreamService(ILogger<TwitterSampleStreamService> logger, 
            IConfiguration configuration,
            IMessageTotalService messageService,
            IHashTagCounterService hashtagService,
            IMessageQueue messageQueue)
        {
            _logger = logger;
            _configuration = configuration;
            _messageService = messageService;
            _hashtagService = hashtagService;
            _messageQueue = messageQueue;
        }

        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await getTwitterSampleStream(cancellationToken);

        }

        private async Task getTwitterSampleStream(CancellationToken token)
        {
            HttpClient httpClient = null;
            Stream responseStream = null;
            StreamReader responseStreamReader = null;

            try
            {
                _logger.LogInformation("Create http request to retrieve twitter sample stream.");

                httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configuration["AppSettings:TwitterBearerToken"]}");
                httpClient.Timeout = new TimeSpan(0, 0, 0, 0, -1);

                responseStream = await httpClient.GetStreamAsync("https://api.twitter.com/2/tweets/sample/stream?tweet.fields=id,entities", token).ConfigureAwait(false);
                
                if (responseStream != null)
                {
                    responseStreamReader = new StreamReader(responseStream, true);
                    string line = responseStreamReader.ReadLine();
                    while (line != null)
                    {
                        _messageQueue.QueueWorkItem((ct) => processMessage(line));
                        Console.WriteLine(line);
                        line = responseStreamReader.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                if(responseStreamReader != null)
                {
                    responseStreamReader.Close();
                    responseStreamReader = null;
                }
                if(responseStream != null)
                {
                    responseStream.Close();
                    responseStream.Dispose();
                }
            }
        }

        private async Task processMessage(string message)
        {
            try
            {
                _messageService.IncrementMessageTotal();

                TweetData formattedMessage = JsonSerializer.Deserialize<TweetData>(message);
                if (formattedMessage.data.entities.hashtags != null)
                {
                    formattedMessage.data.entities.hashtags.ForEach(ht =>
                    {
                        _hashtagService.IncrementTagCounter(ht.tag);
                    });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
