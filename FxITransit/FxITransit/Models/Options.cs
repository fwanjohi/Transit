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
    public class Alerts : ObservableObject
    {
        private int _alertMinsBefore;
        private bool _alert;
        private int _alertInterval;
        private bool _vibrate;
        private bool _speak;
        private bool _autoRefresh;
        private ObservableRangeCollection<Stop> _favoriteStops;




        public Alerts()
        {
            //Stops = new ObservableRangeCollection<FavoriteStop>();
            //Vibrate = true;
            //Speak = true;
        }

        public void Update()
        {
            //OnPropertyChanged("Stops");
        }
        public ObservableRangeCollection<Stop> Stops
        {
            get => _favoriteStops;
            set
            {
                _favoriteStops = value;
                OnPropertyChanged("Stops");
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
                AutoRefresh = true;
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



    }

    


}