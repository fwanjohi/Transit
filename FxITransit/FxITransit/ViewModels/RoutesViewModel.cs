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
using System.Collections.ObjectModel;

namespace FxITransit.ViewModels
{
    public class RoutesViewModel : BaseViewModel
    {

        public Command LoadRoutessCommand { get; set; }
        
        public ObservableCollection<Route> RouteList { get; private set; }
        public Agency Agency { get; set; }

        private string _filter;
        private ObservableCollection<Route> _filteredRoutes;

        public RoutesViewModel(Agency agency)
        {
            Agency = agency;
            Title = "Routes - " + Agency.Title;
            _filteredRoutes = new ObservableRangeCollection<Route>();

            LoadRoutessCommand = new Command(async () => await ExecuteLoadRoutesCommand());
            
            
        }

        public bool ShowFavoritesOnly
        {
            get => Settings.Preference.FavoriteRoutesOnly;
            set
            {
                Settings.Preference.FavoriteRoutesOnly = value;
                Db.SavePrerefence(Settings.Preference);
                OnPropertyChanged("Filtered");
            }
        }
        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                OnPropertyChanged("Filtered");
            }
        }

        public ObservableCollection<Route> Filtered
        {
            get
            {
                var items = new List<Route>();
                if (string.IsNullOrEmpty(_filter))
                {
                    items =new List<Route>(Agency.Routes);//(Agency.Routes);
                }
                else
                {
                     items = new List<Route>(Agency.Routes.Where(x => x.Title.ToLower().Contains(_filter.ToLower())));
                }
                if (ShowFavoritesOnly)
                {
                    items = _filteredRoutes.Where(x => x.IsFavorite == true).ToList();
                }
                _filteredRoutes = new ObservableCollection<Route>(items);
                return _filteredRoutes;
            }
            set
            {
                _filteredRoutes = value;
                //RouteList = value;
                //OnPropertyChanged("RouteList");
            }

        }

        private  async Task ShowStops(Route route)
        {
            
        }
        private async Task ExecuteLoadRoutesCommand()
        {
            try
            {
                var start = DateTime.Now;
                Utils.Log("------------start get and config routes-----");
                var routes = await TransitService.GetRouteList(Agency);

                Agency.Routes.ReplaceRange(routes);

                var end = DateTime.Now;
                var secs = end.Subtract(start).TotalSeconds;
                OnPropertyChanged("Filtered");
                Utils.Log($"-----{secs}-------END get and config routes-----");
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
            
        }
    }
}