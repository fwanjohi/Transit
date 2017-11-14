using Acr.UserDialogs;
using FxITransit.Models;
using FxITransit.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FxITransit.Helpers;
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

        private void OnFavoriteChanged(object sender, EventArgs e)
        {
            var context = (sender as Button).BindingContext as DbEntity;

            if (context != null)
            {
                context.IsFavorite = !context.IsFavorite;
                DbHelper.Instance.SaveEntity(context);
            }
        }

        
        private async void OnRouteSelected(object sender, ItemTappedEventArgs e)
        {
            var route = e.Item as Route;
            if (route != null)
            {
                await Navigation.PushAsync(new StopsPage(route));

            }
        }
    }
}