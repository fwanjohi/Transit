using FxITransit.Helpers;
using SQLite;

namespace FxITransit.Models
{
    public class Agency : DbEntity
    {
        public Agency()
        {
            Routes = new ObservableRangeCollection<Route>();
        }

        public string Tag { get; set; }

        [Indexed]
        public string Title { get; set; }

        public string RegionTitle { get; set; }

        public string ShortTitle { get; set; }

        [Ignore]
        public ObservableRangeCollection<Route> Routes { get; set; }

        
    }

    public  class DbEntity : ObservableObject
    {
        [PrimaryKey]
        public string Id
        {
            get;
            set;
        }

        public string ParentId { get; set; }

    }


}
