using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using LinqToTwitter;
using Plugin.Settings;
using ReactiveTwit.Models;

namespace ReactiveTwit.Services
{
    public class TwitterApi
    {
        private readonly SingleUserAuthorizer auth = new SingleUserAuthorizer
        {
			CredentialStore = new InMemoryCredentialStore
            {
                ConsumerKey = "6KSoSRJzDrHhOVivOnZS54fbV",
                ConsumerSecret = "PUFMr9TnDU3ykZl0oMX1seqUqNJzuuqev5fJGSPck14EqtkvMD",
                OAuthToken = "395296733-1zWxPQbcQKK3pdAsezKzCYtufS8vwNexECelZI8M",
                OAuthTokenSecret = "kdhHJuKwBpYlQSErxyVJrE1aZx0NW5hGZBjJpeha5BSjE"
            }
        };

        private readonly TwitterContext twitterContext;

        public TwitterApi()
        {
            if (string.IsNullOrWhiteSpace(auth.CredentialStore.ConsumerKey)
                || string.IsNullOrWhiteSpace(auth.CredentialStore.ConsumerSecret)
                || string.IsNullOrWhiteSpace(auth.CredentialStore.OAuthToken)
                || string.IsNullOrWhiteSpace(auth.CredentialStore.OAuthTokenSecret))
                throw new Exception("User Credentials are not set. Please update your App.config file.");

            twitterContext = new TwitterContext(auth);
        }

        private IObservable<StreamContent> TwitterStream(string track)
        {
            return Observable.Create<StreamContent>(async o =>
            {
                var query = from s in twitterContext.Streaming
                            where s.Type == StreamingType.Filter && s.Track == track
                            select s;

                var disposed = false;

                await query.StartAsync(s =>
                {
                    if (disposed)
                        s.CloseStream();

                    o.OnNext(s);
                    return Task.FromResult(s);
                    
                });

                return Disposable.Create(() => { disposed = true; });
            });
        }

        public IObservable<Tweet> AllTweetsAbout(string topic)
        {
            return TwitterStream(topic)
                //.Do(x => Debug.WriteLine("step1"))
                .Where(x => x.EntityType == StreamEntityType.Status)
                //.Do(x => Debug.WriteLine("step2"))
                .Where(x => Predicate(x.Entity as Status, topic))
                //.Do(x => Debug.WriteLine("step3"))
                .Select(status =>
                {
                    Tweet tweet;
                    Tweet.TryParse(status.Content, topic, out tweet);
                    return tweet;
                })
                //.Do(x => Debug.WriteLine( x != null ? "si" : "no" ))
                //.Do(x => Debug.WriteLine(x.Text))
                .Where(t => t != null);              
        }

        private static bool Predicate(Status status, string topic)
        {
            return status.Text.ToLower().Contains(topic.ToLower());
        }
    }
}
