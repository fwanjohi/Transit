using FxITransit.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FxITransit.Models
{
    public class Stop : DbEntity
    {

        private double _distance;
        private bool _isFavorited;

        private string _routeTag;
        private string _agencyTag;
        private string _stopTag;
        private string _display;
        private string _fullTitle;

        private string _agencyTitle;
        private string _directionTitle;
        private string _routeTitle;
        private Prediction _prediction1;
        private Prediction _prediction2;
        private Prediction _prediction3;
        private ObservableRangeCollection<Position> _otherStops;
        private string _directionTag;

        public Stop()
        {
            Predictions = new ObservableRangeCollection<Prediction>();
            _otherStops = new ObservableRangeCollection<Position>();
        }

        public static implicit operator Point(Stop stop)
        {
            return new Point { X = stop.Lat, Y = stop.Lon };
        }

        public static implicit operator GeoPoint(Stop stop)
        {
            return new GeoPoint { Lat = stop.Lat, Lon = stop.Lon };
        }

        public string RouteTag
        {
            get { return _routeTag; }
            set { _routeTag = value; OnPropertyChanged("RouteTag"); }
        }

        public string AgencyTag
        {
            get { return _agencyTag; }
            set { _agencyTag = value; OnPropertyChanged("AgencyTag"); }
        }

        public string DirectionTag
        {
            get { return _directionTag; }
            set { _directionTag = value; OnPropertyChanged("DirectionTag"); }
        }

        public string Tag
        {
            get { return _stopTag; }
            set { _stopTag = value; OnPropertyChanged("Tag"); }
        }

        [Ignore]
        public string FullTitle
        {
            get { return $"{AgencyTitle} - {Title}"; }
            //set
            //{
            //    _fullTitle = value;
            //    OnPropertyChanged("FullTitle");
            //}
        }
        [JsonIgnore]
        [Ignore]
        public string MediumTitle
        {
            get { return $"{RouteTitle}, {DirectionTitle }, {Title}"; }
        }

        
        public string RouteTitle
        {
            get => _routeTitle;
            set
            {
                _routeTitle = value;
                OnPropertyChanged("RouteTitle");
                OnPropertyChanged("FullTitle");
                OnPropertyChanged("MediumTitle");
            }
        }

        public string AgencyTitle
        {
            get => _agencyTitle;
            set
            {
                _agencyTitle = value;
                OnPropertyChanged("AgencyTitle");
                OnPropertyChanged("FullTitle");
                OnPropertyChanged("MediumTitle");
            }
        }

        public string DirectionTitle
        {
            get => _directionTitle;
            set
            {
                _directionTitle = value;
                OnPropertyChanged("AgencyTitle");
                OnPropertyChanged("FullTitle");
                OnPropertyChanged("MediumTitle");
            }
        }

        public string Title { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }

        public string StopId { get; set; }

        //		Message	"Self referencing loop detected with type 'FxITransit.Models.Stop'. Path '[0].Direction.Stops'."	string



        [JsonIgnore]
        [Ignore]
        public Position Postion
        {
            get
            {
                return new Position(Lat, Lon);
            }
        }

        [Ignore]
        public double Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                OnPropertyChanged("Distance");
            }
        }
        [Ignore]
        public string Display
        {
            get
            {
                var dist = Distance.ToString("0.##0");
                return $"{StopId} - ({dist} Miles away)";
            }
        }

        [Ignore]
        public string TitleDisplay
        {
            get
            {
                var dist = Distance.ToString("0.##0");
                return $"{Title} - ({dist} Miles away)";
            }
        }

        [JsonIgnore]
        [Ignore]
        public Prediction Prediction1
        {
            get => _prediction1;
            set
            {
                _prediction1 = value; 
                UpdateDiaplay();
            }
        }
        [JsonIgnore]
        [Ignore]
        public Prediction Prediction2
        {
            get => _prediction2;
            set
            {
                _prediction2 = value;
                UpdateDiaplay();
            }
        }
        [JsonIgnore]
        [Ignore]
        public Prediction Prediction3
        {
            get => _prediction3;
            set
            {
                _prediction3 = value;
                UpdateDiaplay();
            }
        }
        [JsonIgnore]
        [Ignore]
        public ObservableRangeCollection<Prediction> Predictions { get; set; }

        [JsonIgnore]
        [Ignore]
        public ObservableRangeCollection<Position> OtherStops
        {
            get => _otherStops;
            set
            {
                _otherStops = value;
                OnPropertyChanged("OtherStops");
                
            }
        }
        internal void UpdateDiaplay()
        {
            foreach (var pred in Predictions)
            {
                pred.LocalTime = new DateTime(pred.LocalTime.Value.Ticks);
                
                OnPropertyChanged("Display");
                OnPropertyChanged("TitleDisplay");
                pred.UpdatePreditionDisplay();

                
                //var isArriving 

            }
            OnPropertyChanged("Prediction1");
            OnPropertyChanged("Prediction2");
            OnPropertyChanged("Prediction3");
        }
    }
}
