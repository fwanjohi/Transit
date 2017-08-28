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
    public partial class RouteListPage : ContentPage
    {
        RoutesViewModel viewModel;
        public RouteListPage()
        {
            InitializeComponent();
        }
        public RouteListPage(Agency agency)
        {
            InitializeComponent();
            BindingContext = this.viewModel = new RoutesViewModel(agency);
        }


        private async void OnRouteSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var route = args.SelectedItem as Route;
            if (route == null)
                return;

            await Navigation.PushAsync(new DirectionsListPage(route));

            // Manually deselect item
            RoutesListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Agency.Routes.Count == 0)
                viewModel.LoadRoutessCommand.Execute(null);
        }
    }
}