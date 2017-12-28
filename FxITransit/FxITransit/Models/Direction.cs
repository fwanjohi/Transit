using FxITransit.Helpers;
using SQLite;

namespace FxITransit.Models
{
    public class Direction : DbEntity
    {
        public bool _isSelected;
        public Direction()
        {
            Stops = new ObservableRangeCollection<Stop>();
            Path = new ObservableRangeCollection<GeoPoint>();
        }
        [Ignore]
        public ObservableRangeCollection<Stop> Stops { get; set; }

        [Ignore]
        public ObservableRangeCollection<GeoPoint> Path { get; set; }

        public string RouteTag { get; set; }
        public string Tag { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string UseForUI { get; set; }
        //public string PathData { get; set; }
        public bool IsSeleted
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        
    }
}