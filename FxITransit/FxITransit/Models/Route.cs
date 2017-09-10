using FxITransit.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FxITransit.Models
{
    public class Route : ObservableObject
    {
        public Route()
        {
            Directions = new ObservableRangeCollection<Direction>();
            Path = new ObservableRangeCollection<GeoPoint>();
            IsConfigured = false;
        }
        public string AgencyTag { get; set; }
        public string Tag { get; set; }
        public string Title { get; set; }
        public ObservableRangeCollection<Direction> Directions { get; set; }
        public ObservableRangeCollection<GeoPoint> Path { get; set; }

        public string Color { get; set; }

        public string OppositeColor { get; set; }

        public string LatMin { get; set; }

        public string LatMax { get; set; }

        public string LonMin { get; set; }

        public string LonMax { get; set; }

        public bool IsConfigured { get; set; }
    }

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

    public class Direction : ObservableObject
    {
        public Direction()
        {
            Stops = new ObservableRangeCollection<Stop>();
        }
        public ObservableRangeCollection<Stop> Stops { get; set; }
        public string Tag { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string UseForUI { get; set; }
        

        
    }

   


}
