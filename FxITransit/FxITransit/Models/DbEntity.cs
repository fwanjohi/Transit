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
                OnPropertyChanged("FavoriteImage");
            }
        }
        [Ignore]
        public string FavoriteImage
        {
            get { return IsFavorite ? Constants.FaveOnIcon : Constants.FaveOffIcon; }
        }
    }

    public class IdItem
    {
        public string Id { get; set; }
    }
}