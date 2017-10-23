using FxITransit.Helpers;
using FxITransit.Models;
using FxITransit.ViewModels;
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
        FavoritesViewModel _viewModel;
        
        public FavouritesPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new FavoritesViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            StopOptionsHelper.Instance.OnPredictionsChanged = _viewModel.OnPredictionsChanged;
            StopOptionsHelper.Instance.ViewStopsToUpdate = new ObservableRangeCollection<Stop>(
               StopOptionsHelper.Instance.MySettings.FavoriteStops);

            StopOptionsHelper.Instance.LoadPredictions();
            StopOptionsHelper.Instance.StartAutoRefresh();

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            StopOptionsHelper.Instance.StopAutoRefresh();
        }

        private void RemoveFave_OnClicked(object sender, EventArgs e)
        {
            var b = sender as Button;
            var stop = b.BindingContext as Stop;
            if (stop != null)
            {
                stop.IsFavorited = true;
                StopOptionsHelper.Instance.ChangeFavouriteStop(stop);
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