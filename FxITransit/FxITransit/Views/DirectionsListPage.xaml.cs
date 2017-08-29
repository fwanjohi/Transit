using FxITransit.Models;
using FxITransit.ViewModels;
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
    public partial class DirectionsListPage : ContentPage
    {
        DirectionsViewModel _viewModel;
        Route _route;


        public DirectionsListPage()
        {
            InitializeComponent();
        }
        public DirectionsListPage(Route route)
        {
            InitializeComponent();
            _route = route;
            BindingContext = this._viewModel = new DirectionsViewModel(route);

        }

        async void OnDirectionSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var direction = args.SelectedItem as Direction;
            if (direction == null)
                return;

            await Navigation.PushAsync(new StopsPage(direction));

            // Manually deselect item
            DirectionsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!_viewModel.Route.IsConfigured)
                _viewModel.PopulateRouteCommand.Execute(null);

            
        }
    }
}