using System;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;

namespace ReactiveTwit.Models
{
    public class Tweet
    {
        #region Properties

        public string User { get; private set; }
        public string Text { get; private set; }
        public string Topic { get; private set; }
        public string Url { get; private set; }
        public string ProfileImageUrl { get; private set; }
        public int UserFavouritesCount { get; private set; }
        public int UserFollowersCount { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public int Score => UserFavouritesCount + UserFollowersCount;

        #endregion

        public static bool TryParse(string json, string topic, out Tweet tweet)
        {
            tweet = null;
            try
            {
                tweet = ParseTweet(json, topic);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        private static Tweet ParseTweet(string json, string topic)
        {
            const string format = "ddd MMM dd HH:mm:ss zzzz yyyy";

            var parsed = JsonConvert.DeserializeObject<RawTweet>(json);

            return new Tweet()
            {
                User = parsed.user.screen_name,
                Text = parsed.text,
                Topic = topic,
                Url = $"https://twitter.com/statuses/{parsed.id}",
                ProfileImageUrl = parsed.user.profile_image_url,
                UserFavouritesCount = parsed.user.favourites_count,
                UserFollowersCount = parsed.user.followers_count,
                TimeStamp = DateTime.ParseExact(parsed.created_at, format, CultureInfo.InvariantCulture)
            };
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
