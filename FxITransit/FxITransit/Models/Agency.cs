using FxITransit.Helpers;

namespace FxITransit.Models
{
    public class Agency
    {
        public Agency()
        {
            Routes = new ObservableRangeCollection<Route>();
        }
        public string Tag { get; set; }
        public string Title { get; set; }
        public string RegionTitle { get; set; }
        public string ShortTitle { get; set; }

        public ObservableRangeCollection<Route> Routes { get; set; }

    }

}
