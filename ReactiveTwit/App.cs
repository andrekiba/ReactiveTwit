using System;
using FreshMvvm;
using ReactiveTwit.Models;
using ReactiveTwit.PageModels;
using ReactiveTwit.Pages;
using Xamarin.Forms;

namespace ReactiveTwit
{
	public class App : Application
	{
		public App()
		{
            LoadBasicNav();
		}

        private void LoadBasicNav()
        {
            var tweetSearchPage = FreshPageModelResolver.ResolvePageModel<TweetSearchPageModel>();
            var navContainer = new FreshNavigationContainer(tweetSearchPage);
            MainPage = navContainer;
        }

        protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

