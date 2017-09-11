using FxITransit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FxITransit.Models
{
    public class Stop : ObservableObject
    {

        private double _distance;
        public Stop()
        {
            Predictions = new ObservableRangeCollection<Prediction>();
        }

        public static implicit operator Point(Stop stop)
        {
            return new Point { X = stop.Lat, Y = stop.Lon };
        }

        public static implicit operator GeoPoint(Stop stop)
        {
            return new GeoPoint { Lat = stop.Lat, Lon = stop.Lon };
        }

        public string Tag { get; set; }

        public string Title { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }

        public string StopId { get; set; }

        public string AgencyTag { get; set; }

        public string RouteTag { get; set; }

        public Position Postion
        {
            get
            {
                return new Position(Lat, Lon);
            }
        }

        public double Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                OnPropertyChanged("Display");
                OnPropertyChanged("TitleDisplay");
            }
        }

        public string Display
        {
            get
            {
                var dist = Distance.ToString("0.##0");
                return $"{StopId} - ({dist} Miles away)";
            }
        }

        public string TitleDisplay
        {
            get
            {
                var dist = Distance.ToString("0.##0");
                return $"{Title} - ({dist} Miles away)";
            }
        }

        public ObservableRangeCollection<Prediction> Predictions { get; set; }
        public Direction Direction { get; set; }
        internal void RefreshTime()
        {
            foreach (var pred in Predictions)
            {
                pred.LocalTime = new DateTime(pred.LocalTime.Value.Ticks);
            }
        }
    }
}
