using FxITransit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxITransit.Models
{
    public class AlertSetttings : ObservableObject
    {
        private int _alertMinsBefore = 3;
        private bool _alert;
        private int _alertInterval = 1;

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

        public bool Speak { get; set; }
        public bool Vibrate { get; set; }

    }

    public class FavoriteSettings :ObservableObject
    {
        public FavoriteSettings()
        {
            FavoriteStops = new ObservableRangeCollection<Stop>();
        }
        public ObservableRangeCollection<Stop> FavoriteStops { get; set; }

    }
}