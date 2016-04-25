using Xamarin.Forms.Xaml;
using Xamarin.Forms;

namespace ReactiveTwit.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TweetSearchPage : ContentPage
    {
        public TweetSearchPage()
        {
            InitializeComponent();
        }

        private void TweetsView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            TweetsView.SelectedItem = null;
        }

        private void TweetsView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            TweetsView.SelectedItem = null;

        }
    }
}
