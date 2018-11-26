using Newtonsoft.Json;

namespace TortoiseTweet.Core.Models
{
    public class TweetData
    {
        [JsonProperty(PropertyName = "id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "tweetContent")]
        public string TweetContent { get; set; }

        [JsonProperty(PropertyName = "isPublished")]
        public bool IsPublished { get; set; }

        [JsonProperty(PropertyName = "publishDate")]
        public string PublishDate { get; set; }
    }
}
