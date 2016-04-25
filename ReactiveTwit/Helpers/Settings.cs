// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ReactiveTwit.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        private const string consumerKey = "6KSoSRJzDrHhOVivOnZS54fbV";
        private const string consumerSecret = "PUFMr9TnDU3ykZl0oMX1seqUqNJzuuqev5fJGSPck14EqtkvMD";
        private const string authtoken = "395296733-1zWxPQbcQKK3pdAsezKzCYtufS8vwNexECelZI8M";
        private const string authtokenSecret = "kdhHJuKwBpYlQSErxyVJrE1aZx0NW5hGZBjJpeha5BSjE";

        #endregion


        public static string ConsumerKey
        {
            get
            {
                return AppSettings.GetValueOrDefault(consumerKey, string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(consumerKey, value);
            }
        }

        public static string ConsumerSecret
        {
            get
            {
                return AppSettings.GetValueOrDefault(consumerSecret, string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(consumerSecret, value);
            }
        }

        public static string Authtoken
        {
            get
            {
                return AppSettings.GetValueOrDefault(authtoken, string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(authtoken, value);
            }
        }

        public static string AuthtokenSecret
        {
            get
            {
                return AppSettings.GetValueOrDefault(authtokenSecret, string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(authtokenSecret, value);
            }
        }

    }
}