using System.Collections.Generic;
using System.Linq;
using TwitterStreamer.Services.Interfaces;

namespace TwitterStreamer.Services
{
    public class HashTagCounterService : IHashTagCounterService
    {
        private readonly IDataCache _dataCache;

        public HashTagCounterService(IDataCache dataCache)
        {
            _dataCache = dataCache;
        }

        public IList<string> GetTop10HashTag()
        {
            var data = _dataCache.GetCache<Dictionary<string, int>>("HashTagCounter");
            var sortedData = data.OrderByDescending(key => key.Value);
            return sortedData.Select(ht => ht.Key).Take(10).ToList();
        }

        public void IncrementTagCounter(string tag)
        {
            var data = _dataCache.GetCache<Dictionary<string, int>>("HashTagCounter");
            if(data == null)
            {
                data = new Dictionary<string, int>();
            }
            if(data.TryGetValue(tag, out int counter))
            {
                data[tag] = counter + 1;
            }
            else
            {
                data.Add(tag, 1);
            }
            _dataCache.UpdateCache("HashTagCounter", data);
        }
    }
}
