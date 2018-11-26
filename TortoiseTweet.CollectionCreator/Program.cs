using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TortoiseTweet.Core.Cosmos;
using TortoiseTweet.Core.Models;

namespace TortoiseTweet.CollectionCreator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Reading text file");
            var textDataLines = File.ReadAllLines(@"TweetInputData.txt");
            Console.WriteLine($"File read contains {textDataLines.Length} lines");

            Console.WriteLine("Generating object collection...");
            var tweetList = new List<TweetData>();
            foreach (var item in textDataLines)
            {
                if (item.Length <= 280) //Trim the text length just in case.
                {
                    var tempObject = new TweetData()
                    {
                        TweetContent = item,
                        IsPublished = false,
                        PublishDate = "0000-00-00 00:00:00"
                    };

                    tweetList.Add(tempObject);
                }
            }

            Console.WriteLine($"Object collection generated... {tweetList.Count} of {textDataLines.Length}");
            Console.WriteLine("Ready to push data... presss enter to start");
            Console.ReadLine(); //Just to verify the numbers on console before we submit stuff. Azure ops can be expensive :)

            //Choose to do bulk operation if collection size is large...
            foreach (var item in tweetList)
            {
                Console.WriteLine("Inserting..." + item.TweetContent.Substring(0, 10) + "...");
                DocumentDbRepository<TweetData>.CreateItemAsync(item).GetAwaiter().GetResult();
                Task.Delay(100);
            }

            Console.WriteLine("Push done");
            Console.ReadLine();
        }
    }
}
