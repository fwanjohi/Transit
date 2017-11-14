using Acr.Settings;
using FxITransit.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace FxITransit.Models
{
    //https://github.com/aritchie/settings
    public class Preference : DbEntity
    {
        private int _alertMinsBefore;
        private bool _alert;
        private int _alertInterval;
        private bool _vibrate;
        private bool _speak;
        private bool _autoRefresh;

        private bool _favoriteAgenciesOnly;
        private bool _favoriteRoutesOnly;
        private bool _favoriteStopsOnly;

        private ObservableRangeCollection<Stop> _favoriteStops;

        public Preference()
        {
            Id = "user";
            RefreshInterval = 30;
            _favoriteStops = new ObservableRangeCollection<Stop>();
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

        public bool FavoriteAgenciesOnly
        {
            get { return _favoriteAgenciesOnly; }
            set
            {
                _favoriteAgenciesOnly = value;
                
                OnPropertyChanged("FavoriteAgenciesOnly");
                
            }
        }

        public bool FavoriteRoutesOnly
        {
            get { return _favoriteRoutesOnly; }
            set
            {
                _favoriteRoutesOnly = value;
                
                OnPropertyChanged("FavoriteRoutesOnly");
            }
        }

        public bool FavoriteStopsOnly
        {
            get { return _favoriteStopsOnly; }
            set
            {
                _favoriteStopsOnly = value;
                
                OnPropertyChanged("FavoriteStopsOnly");
            }
        }

        
    }




}