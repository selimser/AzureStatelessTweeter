using TweetSharp;

namespace TortoiseTweet.Core
{
    public sealed class TweetHandler
    {
        private const string _apiKey = "";
        private const string _apiSecretKey = "";
        private const string _accessToken = "";
        private const string _accessTokenSecret = "";



        private static readonly TwitterService _twitterService = new TwitterService(_apiKey, _apiSecretKey);

        public TweetHandler()
        {
            _twitterService.AuthenticateWith(_accessToken, _accessTokenSecret);
        }

        public void SendTweet(string tweetText)
        {
            var tweetResult =
                _twitterService.SendTweet(new SendTweetOptions
                {
                    Status = tweetText
                });
        }
    }
}
