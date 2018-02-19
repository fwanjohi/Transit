using Acr.UserDialogs;
using FxITransit.Helpers;
using FxITransit.Models;
using FxITransit.Services.NextBus;
using FxITransit.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FxITransit.Layouts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FxITransit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgencyListView : ContentPage
    {
        AgenciesViewModel viewModel;

        public AgencyListView()
        {
            InitializeComponent();
            BindingContext = viewModel = new AgenciesViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var agency = args.SelectedItem as Agency;
            if (agency == null)
                return;

            await Navigation.PushAsync(new RouteListPage(agency));
            ItemsListView.SelectedItem = null;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                UtilsHelper.Instance.Log("OnAppearing()");
                TrackingHelper.Instance.InitializeGeoLocator();
            }
            catch (Exception ex)
            {
                UtilsHelper.Instance.Log("Error after InitializeGeoLocator()" + ex.Message);

            }

            if (viewModel.Agencies.Count == 0)
            {

                viewModel.LoadAgenciesCommand.Execute(null);
            }
            
        }

        private void OnFavoriteChanged(object sender, EventArgs e)
        {
            var agency = (sender as Button).BindingContext as Agency;

            if (agency != null)
            {
                agency.IsFavorite = !agency.IsFavorite;
                DbHelper.Instance.SaveEntity(agency);
            }
        }

        private void ViewCell_BindingContextChanged(object sender, EventArgs e)
        {
            try
            {
                var cell = sender as ViewCell;
                if (cell != null)
                {
                    var data = cell.BindingContext as Agency;
                    var index = viewModel.Filtered.IndexOf(data);
                    cell.View.BackgroundColor = index % 2 == 0 ? Color.White : Color.LightGray;

                }
            } catch (Exception ex)
            {
                var m = ex.Message;
            }
            
        }
    }
}