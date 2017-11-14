using FxITransit.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FxITransit.Models
{
    public class Route : DbEntity
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

        [Ignore]
        public string Display
        {
            get { return $"{AgencyTitle} - {Title}"; }
        }

        [Ignore]
        public ObservableRangeCollection<Direction> Directions { get; set; }

        [Ignore]
        public ObservableRangeCollection<GeoPoint> Path { get; set; }

        public string Color { get; set; }

        public string OppositeColor { get; set; }

        public string LatMin { get; set; }

        public string LatMax { get; set; }

        public string LonMin { get; set; }

        public string LonMax { get; set; }

        public bool IsConfigured { get; set; }
        public string AgencyTitle { get; internal set; }
    }
}
