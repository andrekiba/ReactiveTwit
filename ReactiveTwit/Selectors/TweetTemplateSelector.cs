using ReactiveTwit.CustomCells;
using ReactiveTwit.Models;
using Xamarin.Forms;

namespace ReactiveTwit.Selectors
{
    public class TweetTemplateSelector : Xamarin.Forms.DataTemplateSelector
    {
        public TweetTemplateSelector()
        {
            // Retain instances!
            this.highDataTemplate = new DataTemplate(typeof (HighScoreCell));
            this.lowDataTemplate = new DataTemplate(typeof (LowScoreCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var tweet = item as Tweet;
            if (tweet == null)
                return null;
            return tweet.Score > 100 ? highDataTemplate : lowDataTemplate;
        }

        private readonly DataTemplate highDataTemplate;
        private readonly DataTemplate lowDataTemplate;
    }
}
