using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using TortoiseTweet.Core;
using TortoiseTweet.Core.Cosmos;
using TortoiseTweet.Core.Models;

namespace TortoiseTweeter
{
    public static class TweetSender
    {
        private static TweetHandler _tweetHandler = new TweetHandler();

        [FunctionName("SendTweet")]
        public static void Run([TimerTrigger("0 3,12,20,32,44,59 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"Timer trigger function executed at (UTC): {DateTime.UtcNow}");

            var nextTweet = GetNextTweetToSend(log).GetAwaiter().GetResult(); //Get the next tweet
            _tweetHandler.SendTweet(nextTweet.Item1); //Send it
            UpdateTweetStatus(log, nextTweet.Item2, nextTweet.Item1); //Update cosmos db record


            //LogOperationStatus(log).GetAwaiter().GetResult(); //TODO: Would be nice to log into the stream logs.
        }

        private static async Task<Tuple<string, string>> GetNextTweetToSend(TraceWriter log)
        {
            log.Info($"Getting next item from document db repo");
            var result = await DocumentDbRepository<TweetData>.GetItemsAsync(p => p.IsPublished == false);
            log.Info($"Got the tweet from Cosmos DB!");

            return new Tuple<string, string>(result.FirstOrDefault().TweetContent, result.FirstOrDefault().Id);
        }

        private static void UpdateTweetStatus(TraceWriter log, string entryId, string tweetContent)
        {
            //do the update operation

            DocumentDbRepository<TweetData>.UpdateItemAsync(entryId, new TweetData()
            {
                Id = entryId,
                IsPublished = true,
                PublishDate = DateTime.UtcNow.ToString(),
                TweetContent = tweetContent
            }).GetAwaiter().GetResult();

        }
        
    }
}
