using Acr.UserDialogs;
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
            //_viewModel.FavoriteStops = new List<Stop>(StopOptionsHelper.Instance.MySettings.FavoriteStops);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.FavoriteStops = new ObservableRangeCollection<Stop>(DbHelper.Instance.GetFavoriteStops());
            PreferencesHelper.Instance.OnPredictionsChanged = _viewModel.OnPredictionsChanged;

            PreferencesHelper.Instance.ViewStopsToUpdate = new ObservableRangeCollection<Stop>(
                _viewModel.FavoriteStops);


            PreferencesHelper.Instance.LoadPredictions();
            PreferencesHelper.Instance.StartAutoRefresh();

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            PreferencesHelper.Instance.StopAutoRefresh();
            base.OnDisappearing();
            PreferencesHelper.Instance.SaveSttingsToFile();
        }

        private void RemoveFave_OnClicked(object sender, EventArgs e)
        {
            var b = sender as Button;
            var stop = b.BindingContext as Stop;
            if (stop != null)
            {
                //StopOptionsHelper.Instance.ChangeFavouriteStop(stop, false);
            }
        }

        private void FavesListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Stop stop)
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
            ////Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.ShowLoading("Testing...", MaskType.Black));
            ////Task.Run(async () =>
            ////{
            ////    await Task.Delay(1000 * 30);

            ////}).ContinueWith(result => Device.BeginInvokeOnMainThread(() =>
            ////{

            ////    UserDialogs.Instance.HideLoading();

            ////}));

            ConfirmConfig config = new ConfirmConfig
            {
                Title = "Remove from faves?",
                CancelText = "No",
                OkText = "Yes",
                Message = "Message",
                OnAction = DoSomething
            };
            UserDialogs.Instance.Confirm(config);


        }

        private void btnFave_Clicked(object sender, EventArgs e)
        {
            var stop = (sender as Button)?.BindingContext as Stop;

            ConfirmConfig config = new ConfirmConfig
            {
                Title = "Remove from faves?",
                CancelText = "No",
                OkText = "Yes",
                Message = "Message",
                OnAction=(yes) =>
                {
                    if (yes)
                    {
                        stop.IsFavorite = false;

                        DbHelper.Instance.SaveEntity(stop);
                        _viewModel.FavoriteStops.Remove(stop);
                        OnPropertyChanged("FavoriteStops");
                    }
                   
                }

            };

            UserDialogs.Instance.Confirm(config);

        }


        




        private void DoSomething(bool ok)
        {
            
        }


    }
}
