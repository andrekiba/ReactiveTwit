using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveTwit.Models
{
    public class ClassifiedTweet
    {
        public ClassifiedTweet(Tweet tweet, int score)
        {
            Score = score;
            Tweet = tweet;
        }

        public Tweet Tweet { get; private set; }
        public int Score { get; private set; }
    }

    public static class TweetAnalysis
    {
        public static ClassifiedTweet Classify(this Tweet tweet)
        {
            return new ClassifiedTweet(tweet, tweet.UserFollowersCount + tweet.UserFavouritesCount);
        }
    }
}
