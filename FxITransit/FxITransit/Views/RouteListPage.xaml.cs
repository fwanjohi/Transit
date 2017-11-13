using Acr.UserDialogs;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Agency.Routes.Count == 0)
                viewModel.LoadRoutessCommand.Execute(null);

        }

        private void BtnFaveRoute_OnClicked(object sender, EventArgs e)
        {
            var route = (sender as Button).BindingContext as Route;
            if (route != null)
            {
                route.IsFavorite = !route.IsFavorite;
            }
        }

        private async void BtnRouteSelected_OnClicked(object sender, EventArgs e)
        {
            var route = (sender as Button).BindingContext as Route;
            if (route != null)
            {
                await Navigation.PushAsync(new StopsPage(route));

            }
        }
    }
}