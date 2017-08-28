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
    public partial class DirectionsListPage : ContentPage
    {
        DirectionsViewModel viewModel;
        
        public DirectionsListPage()
        {
            InitializeComponent();
        }
        public DirectionsListPage(Route route)
        {
            InitializeComponent();

            BindingContext = this.viewModel = new DirectionsViewModel(route);
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

            if (!viewModel.Route.IsConfigured)
                viewModel.PopulateRouteCommand.Execute(null);
        }
    }
}