using Acr.Settings;
using FxITransit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace FxITransit.Models
{
    //https://github.com/aritchie/settings
    public class MySettings : ObservableObject
    {
        private int _alertMinsBefore;
        private bool _alert;
        private int _alertInterval;
        private bool _vibrate;
        private bool _speak;
        private bool _autoRefresh;
        private string _faveStopsString;

        private ObservableRangeCollection<Stop> _favoriteStops;
        private ObservableRangeCollection<Route> _favoriteRoutes;

        public MySettings()
        {
            //Stops = new ObservableRangeCollection<FavoriteStop>();
            //Vibrate = true;
            //Speak = true;
            RefreshInterval = 30; //refresh every second
        }

        private string _agencyCache;
        public string  AgencyCache
        {
            get => _agencyCache;
            set
            {
                _agencyCache = value;
                OnPropertyChanged("AgencyCache");
            }
        }


        public void Update()
        {
            OnPropertyChanged("FavoriteStops");
            //OnPropertyChanged("FavoriteRoutes");

        }
        public ObservableRangeCollection<Stop> FavoriteStops
        {
            get { return _favoriteStops; }
            set
            {
                _favoriteStops = value;
                OnPropertyChanged("FavoriteStops");
            }
        }

        public ObservableRangeCollection<Route> FavoriRoutes
        {
            get { return _favoriteRoutes; }
            set
            {
                _favoriteRoutes = value;
                OnPropertyChanged("FavoriRoutes");
            }
        }




        public int AlertMinsBefore
        {
            get { return _alertMinsBefore; }
            set
            {
                _alertMinsBefore = value;
                OnPropertyChanged("AlertMinsBefore");
            }
        }

        public int AlertInterval
        {
            get { return _alertInterval; }
            set
            {
                _alertInterval = value;
                OnPropertyChanged("AlertInterval");
            }
        }

        public bool Alert
        {
            get { return _alert; }
            set
            {
                _alert = value;
                OnPropertyChanged("Alert");
            }
        }

        public bool AutoRefresh
        {
            get { return _autoRefresh; }
            set
            {
                _autoRefresh = value;
                OnPropertyChanged("AutoRefresh");
            }
        }

        public bool Speak
        {
            get { return _speak; }
            set
            {
                _speak = value;
                OnPropertyChanged("Speak");
            }
        }
        public bool Vibrate
        {
            get { return _vibrate; }
            set
            {
                _vibrate = value;
                OnPropertyChanged("Vibrate");
            }
        }

        public int RefreshInterval { get; private set; }
    }




}