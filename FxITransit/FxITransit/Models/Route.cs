using FxITransit.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;


namespace FxITransit.Models
{
    public class Route : ObservableObject
    {
        public Route()
        {
            Directions = new ObservableRangeCollection<Direction>();
            Path = new ObservableRangeCollection<Point>();
            IsConfigured = false;
        }
        public string AgencyTag { get; set; }
        public string Tag { get; set; }
        public string Title { get; set; }
        public ObservableRangeCollection<Direction> Directions { get; set; }
        public ObservableRangeCollection<Point> Path { get; set; }
        
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
        public Stop()
        {
            Predictions = new ObservableRangeCollection<Prediction>();
        }
        public string Tag { get; set; }

        public string Title { get; set; }

        public string Lat { get; set; }

        public string Lon { get; set; }

        public string StopId { get; set; }

        public string AgencyTag { get; set; }

        public string RouteTag { get; set; }
        
        public ObservableRangeCollection<Prediction> Predictions { get; set; }

        internal void RefreshTime()
        {
           foreach(var pred in Predictions)
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
        public ObservableRangeCollection<Stop> Stops{ get; set; }
        public string Tag { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string UseForUI { get; set; }
    }

    public class Point
    {
        public string Lat { get; set; }
        public string Lon { get; set; }
    }


}
