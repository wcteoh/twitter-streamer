
namespace TwitterStreamer.Models
{
    public class Tweet
    {
        public string id { get; set; }
        public string text { get; set; }
        public TweetEntity entities { get; set; }
    }
}
