using System.Collections.Generic;

namespace TwitterStreamer
{
    public interface IDataCache
    {
        T GetCache<T>(string key);
        void AddCache(string key, object dataSet);
        bool RemoveCache(string key);
        void UpdateCache(string key, object dataSet);
    }


    public class DataCache : IDataCache
    {
        private readonly Dictionary<string, object> _cachedData = new Dictionary<string, object>();

        public T GetCache<T>(string key)
        {
            if (_cachedData.TryGetValue(key, out object value))
            {
                return (T)value;
            }

            return default(T);
        }

        public void AddCache(string key, object dataSet)
        {
            _cachedData.Add(key, dataSet);
        }

        public bool RemoveCache(string key)
        {
            return _cachedData.Remove(key);
        }

        public void UpdateCache(string key, object dataSet)
        {
            _cachedData[key] = dataSet;
        }
    }

}
