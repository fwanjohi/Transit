using Acr.Settings;
using FxITransit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxITransit.Models
{
    //https://github.com/aritchie/settings
    public class AlertSettings : ObservableObject
    {
        private int _alertMinsBefore;
        private bool _alert;
        private int _alertInterval;
        private bool _vibrate;
        private bool _speak;
        private bool _autoRefresh;

        public AlertSettings()
        {

            //Vibrate = true;
            //Speak = true;
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

    public class FavoriteSettings : ObservableObject
    {
        private ObservableRangeCollection<Stop> _stops;

        public FavoriteSettings()
        {
            Stops = new ObservableRangeCollection<Stop>();

            var stops = new List<Stop>();
            stops.Add(new Stop { Title = "Test 1", Distance = 1, StopId = "1", Tag = "10001" });
            stops.Add(new Stop { Title = "Test 2", Distance = 1, StopId = "1", Tag = "10001" });
            stops.Add(new Stop { Title = "Test 3", Distance = 1, StopId = "1", Tag = "10001" });
            stops.Add(new Stop { Title = "Test 4", Distance = 1, StopId = "1", Tag = "10001" });

            Stops.AddRange(stops);
        }
        public ObservableRangeCollection<Stop> Stops
        {
            get { return _stops; }
            set
            {
                _stops = value;
                OnPropertyChanged("Stops");
            }
        }

    }
}