using FxITransit.Helpers;
using FxITransit.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FxITransit.ViewModels
{
    public class DirectionsViewModel : BaseViewModel

    {
        
        public Route Route { get; private set; }
        public Command PopulateRouteCommand { get; set; }
        public Action OnPopulateRoutesDone { get; set; }
        public DirectionsViewModel(Route route)
        { 

            Route = route;
            Title = "Directions : " + Route.Title;
            PopulateRouteCommand = new Command(async () => await ExecutePopulateRouteCommandCommand());

        }
        
        async Task ExecutePopulateRouteCommandCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (!Route.IsConfigured)
                {
                    TransitService.PopulateRouteDetails(Route, OnPopulateRoutesDone);
                    //await Task.Factory.StartNew(async () =>
                    // {
                    //     await DataStore.PopulateRouteDetails(Route, OnPopulateRoutesDone);

                    // });
                    //var data = 1;

                }
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load route details.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

