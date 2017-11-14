using FxITransit.Helpers;
using FxITransit.Models;
using FxITransit.Services;
using FxITransit.ViewModels;
using Plugin.Geolocator;
using Plugin.TextToSpeech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FxITransit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StopsPage : ContentPage
    {
        StopsViewModel _viewModel;
        CustomMap _map;
        public StopsPage()
        {
            InitializeComponent();


        }

        public StopsPage(Route route)
        {
            InitializeComponent();
           
            BindingContext = this._viewModel = new StopsViewModel(route);
            _viewModel.TabsContainer = StkDir;


        }



        private async void OnStopSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var stop = args.SelectedItem as Stop;
            if (stop == null)
                return;

            await Navigation.PushAsync(new PredictionsPage(stop));

            // Manually deselect item
            StopsListView.SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _viewModel.ConfigureRoute();
            MapHolder.Children.Add(_viewModel.Map);
        }

        private async Task BtnClosest_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PredictionsPage(_viewModel.ClosestStop));
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {

        }

        private void OnFavoriteChanged(object sender, EventArgs e)
        {
            var stop = (sender as Button).BindingContext as Stop;
            if (stop != null)
            {
                stop.IsFavorite = !stop.IsFavorite;
                DbHelper.Instance.SaveEntity(stop);
                
            }
        }

        private void OnDirectionChanged(object sender, EventArgs e)
        {
            var direction = (sender as Button).BindingContext as Direction;
            if (direction != null)
            {
                _viewModel.ChangeDirection(direction);

                
            }

        }
    }
}