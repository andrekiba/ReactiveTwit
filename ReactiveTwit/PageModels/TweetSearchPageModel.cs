using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using ReactiveTwit.Models;
using ReactiveTwit.Services;
using Xamarin.Forms;
using System.Diagnostics;

namespace ReactiveTwit.PageModels
{
    public class TweetSearchPageModel : BasePageModel
    {
        public string SearchText { get; set; }
        
		public ObservableCollection<Tweet> Tweets { get; set; } = new ObservableCollection<Tweet>();

        public ObservableCollection<string> SearchStrings { get; set; } = new ObservableCollection<string>();

        public bool IsBusy { get; set; }


        public TweetSearchPageModel()
        {
            var twitterApi = new TwitterApi();

            var searchTexts = this
                .ToObservable(() => SearchText)
                .Throttle(TimeSpan.FromSeconds(1))
                .Where(x => x.Length > 3)
                .Do(x => Debug.WriteLine(x));
                
			var switched = searchTexts
				.Select(twitterApi.AllTweetsAbout)               
                .Do(x => Debug.WriteLine("Switching"))             
				.Switch()
                //.Merge()
                .Do(x => Debug.WriteLine(x.Text));

			var tweets = switched
                .Sample(TimeSpan.FromSeconds(1))
				.Select(TweetAnalysis.Classify)
                .Publish();

            tweets
                .Where(x => x.Score > 100)
                .Select(x => x.Tweet)
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(UpdateTweets);

            tweets
                .Where(x => x.Score <= 100)
                .Select(x => x.Tweet)
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(UpdateTweets);

            tweets.Connect();
           
        }

        private void UpdateTweets(Tweet tweet)
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    AddToList(tweet, Tweets);
            //});

            AddToList(tweet, Tweets);
        }

        private static void AddToList(Tweet tweet, IList<Tweet> collection)
        {
            collection.Insert(0, tweet);
            if (collection.Count > 20)
                collection.Remove(collection.Last());
        }

        private void UpdateSearchList(string search)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
               SearchStrings.Insert(0, search);
                if (SearchStrings.Count > 20)
                    SearchStrings.Remove(SearchStrings.Last());
            });           
        }

        #region Commands

        private ICommand addCommand;
        public ICommand AddCommand => addCommand ?? (addCommand = new Command(ExecuteAddCommand));

        private void ExecuteAddCommand()
        {
            SearchStrings.Add("ciao");
            SearchStrings.Add("mila");
            SearchStrings.Add("xamarin");
        }

        private ICommand refreshCommand;
        public ICommand RefreshCommand => refreshCommand ?? (refreshCommand = new Command(ExecuteRefreshCommand));

        private void ExecuteRefreshCommand()
        {
            //SearchStrings = new ObservableCollection<string>(SearchStrings);
			//Tweets = new ObservableCollection<Tweet>(Tweets);
        }

        #endregion
    }
}
