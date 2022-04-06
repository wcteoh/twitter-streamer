
using TwitterStreamer.Services.Interfaces;

namespace TwitterStreamer.Services
{
    public class MessageTotalService : IMessageTotalService
    {
        private readonly IDataCache _dataCache;

        public MessageTotalService(IDataCache dataCache)
        {
            _dataCache = dataCache;
        }

        public int GetMessageTotal()
        {
            var result = _dataCache.GetCache<int>("TotalMessage");
            return result;
        }

        public void IncrementMessageTotal()
        {
            var result = _dataCache.GetCache<int>("TotalMessage");
            _dataCache.UpdateCache("TotalMessage", result + 1);
        }

    }
}
