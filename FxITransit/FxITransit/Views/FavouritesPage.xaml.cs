using FxITransit.Helpers;
using FxITransit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FxITransit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavouritesPage : ContentPage
    {
        public FavouritesPage()
        {
            InitializeComponent();
            BindingContext = OptionsHelper.Instance;
            var items = OptionsHelper.Instance.Alerts.FavoriteStops;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }

        private void RemoveFave_OnClicked(object sender, EventArgs e)
        {
            var b = sender as Button;
            var stop = b.BindingContext as Stop;
            if (stop != null)
            {
                stop.IsFavorited = true;
                OptionsHelper.Instance.ChangeFavourite(stop);
            }
        }

        private void FavesListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var stop = e.Item as Stop;
            if (stop != null)
            {
                var page = Application.Current.MainPage as MainLaunchPage;

                var browse = page.Children.FirstOrDefault(x => (x as NavigationPage).Title == "Browse");

                if (browse != null)
                {
                    page.CurrentPage = browse;

                    browse.Navigation.PushAsync(new PredictionsPage(stop));
                }




            }

        }

        private void BackButtio_OnClicked(object sender, EventArgs e)
        {
           //// var cur = sender.
           // var cur = Application.Current.MainPage as TabbedPage;
           // cur.SendBackButtonPressed();



        }
    }
}