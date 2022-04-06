using System.Collections.Generic;

namespace TwitterStreamer.Services.Interfaces
{
    public interface IHashTagCounterService
    {
        IList<string> GetTop10HashTag();
        void IncrementTagCounter(string tag);
    }
}
