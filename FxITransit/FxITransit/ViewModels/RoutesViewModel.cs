using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FxITransit.Helpers;
using FxITransit.Models;
using FxITransit.Views;

using Xamarin.Forms;
using FxITransit.Services.NextBus;
using System.Collections.Generic;
using System.Linq;

namespace FxITransit.ViewModels
{
    public class RoutesViewModel : BaseViewModel
    {

        public Command LoadRoutessCommand { get; set; }

        public Agency Agency { get; set; }

        private string _filter;
        private ObservableRangeCollection<Route> _filteredRoutes;
        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                OnPropertyChanged("FilteredRoutes");
            }
        }

        public ObservableRangeCollection<Route> FilteredRoutes
        {
            get
            {
                if (string.IsNullOrEmpty(_filter))
                {
                    _filteredRoutes.ReplaceRange(Agency.Routes);
                }
                else
                {
                    var items = new List<Route>(Agency.Routes.Where(x => x.Title.ToLower().Contains(_filter.ToLower())));

                    _filteredRoutes.ReplaceRange(items);
                }
                return _filteredRoutes;
            }

        }

        public RoutesViewModel(Agency agency)
        {
            Agency = agency;
            Title = "Routes - " + Agency.Title;
            _filteredRoutes = new ObservableRangeCollection<Route>();

            LoadRoutessCommand = new Command(async () => await ExecuteLoadRoutesCommand());


            //MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            //{
            //    var _item = item as Item;
            //    Items.Add(_item);
            //    await DataStore.AddItemAsync(_item);
            //});
        }

        async Task ExecuteLoadRoutesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var routes = await TransitService.GetRouteList(Agency);

                Agency.Routes.ReplaceRange(routes);
                _filteredRoutes.ReplaceRange(routes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load routes.",
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