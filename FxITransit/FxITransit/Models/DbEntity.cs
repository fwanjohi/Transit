using FxITransit.Helpers;
using SQLite;
using Xamarin.Forms;

namespace FxITransit.Models
{
    public  class DbEntity : ObservableObject
    {
        private bool _isFavorite;

        [PrimaryKey]
        public string Id
        {
            get;
            set;
        }

        [Indexed]
        public string ParentId { get; set; }

        
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                _isFavorite = value;
                OnPropertyChanged("IsFavorite");

            }
        }
       

    }
}