using FxITransit.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Diagnostics;

namespace FxITransit.Models
{
    [DebuggerDisplay("Id={Id}, Name={Display}")]

    public class Route : DbEntity
    {
        public Route()
        {
            Directions = new ObservableRangeCollection<Direction>();
            Stops = new ObservableRangeCollection<Stop>();
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
        public ObservableRangeCollection<Stop> Stops { get; set; }

        public string Color { get; set; }

        public string OppositeColor { get; set; }

        public double StartLat { get; set; }

        public double EndLat { get; set; }

        public double StartLon { get; set; }

        public double EndLon { get; set; }

        public bool IsConfigured { get; set; }
        public string AgencyTitle { get; internal set; }
        //public string PathData { get; internal set; }
    }
}
