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

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new NewItemPage());
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

        private void BtnSpeedMeUp_Clicked(object sender, EventArgs e)
        {
            try
            {
                var count = 0;
                var agencyCount = 0;
                var routeCount = 0;
                var start = DateTime.Now;
                UserDialogs.Instance.ShowLoading("Getting the universe...", MaskType.Black);
                Task.Run(async () =>
                {
                    var svc = NextBusService.Instance;
                    var agencies = await svc.GetAgencyList();
                    foreach (var agency in agencies)
                    {
                        agencyCount++;
                        var routes = await svc.GetRouteList(agency, true);
                        //foreach (var route in routes)
                        //{
                        //    //routeCount++;
                        //    //await svc.GetRouteDetails(route);
                        //    //foreach (var dir in route.Directions)
                        //    //{
                        //    //    foreach (var stop in dir.Stops)
                        //    //    {
                        //    //        UtilsHelper.Instance.Log($"-----we have {stop.FullTitle}");
                        //    //        count++;
                        //    //    }
                        //    //}
                        //}
                    }

                    //var agencies = await TransitService.GetAgencyList();
                    //Agencies.ReplaceRange(agencies);
                    //_filteredAgencies.ReplaceRange(Agencies);
                    //OnPropertyChanged("FilteredAgencies");

                }).ContinueWith(result => Device.BeginInvokeOnMainThread(() =>
                {
                    var end = DateTime.Now;
                    var span = start.Subtract(end).Seconds;
                    UtilsHelper.Instance.Log($"****it took me {span} to get agencies= {agencyCount}; Routes = {routeCount}, Stops= {count}");
                    UserDialogs.Instance.HideLoading();

                }));
            }
            catch (Exception ex)
            {
                UtilsHelper.Instance.Log("ERROR " + ex.Message);
            }
        }

        public void OnRoutesPoulated()
        {

        }
    }
}