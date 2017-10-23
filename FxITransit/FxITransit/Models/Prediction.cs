using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using FxITransit.Helpers;
using Xamarin.Forms;

namespace FxITransit.Models
{
    public class Prediction : ObservableObject
    {
        private string _display;
        DateTime? _localTime;
        private bool _isArriving;


        public string EpochTime { get; set; }
        public string Seconds { get; set; }
        public string Minutes { get; set; }
        public string IsDeparture { get; set; }
        public string DirTag { get; set; }
        public string Vehicle { get; set; }
        public string Block { get; set; }
        public string TripTag { get; set; }
        public string AffectedByLayover { get; set; }
        public string VehiclesInConsist { get; set; }

        public DateTime? LocalTime
        {
            get { return _localTime; }
            set
            {
                _localTime = value;
                UpdatePreditionDisplay();
            }
        }
        public string Display
        {
            get { return GetTime(); }
            set
            {
                _display = value;
                UpdatePreditionDisplay();
            }
        }

       
        public bool IsArriving
        {
            get { return _isArriving; }
            set
            {
                _isArriving = value;
                UpdatePreditionDisplay();
            }
        }

        public Color ForeColor { get => IsArriving ? Color.Red : Color.Blue; }

        private string GetTime()
        {
            if (LocalTime.HasValue)
            {
                var totalSecs = LocalTime.Value.Subtract(DateTime.Now).TotalSeconds;
                var secs = (int)totalSecs % 60;
                var mins = (int)(totalSecs / 60);

                return $"{mins} Mins, {secs} Secs";
            }
            else
            {
                return "-";

            }
        }

        internal void UpdatePreditionDisplay()
        {
            OnPropertyChanged("IsArriving");
            OnPropertyChanged("Display");
            OnPropertyChanged("ForeColor");

        }
    }


    public class PredictionMessage
    {
        public string Text { get; set; }
        public string Priority { get; set; }
    }

    public class Predictions
    {
        public Direction Direction { get; set; }
        public List<PredictionMessage> Messages { get; set; }
        public string AgencyTitle { get; set; }
        public string RouteTitle { get; set; }
        public string RouteTag { get; set; }
        public string StopTitle { get; set; }
        public string StopTag { get; set; }
    }



}