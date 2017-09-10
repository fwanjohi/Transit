using FxITransit.Helpers;
using FxITransit.Models;
using FxITransit.Services;
using FxITransit.Services.NextBus;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FxITransit.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        /// <summary>
        /// Get the azure service instance
        /// </summary>
        public ITransitService DataStore { get; private set; }

        public BaseViewModel()
        {
            DataStore = new NextBusService();
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        /// <summary>
        /// Private backing field to hold the title
        /// </summary>
        string title = string.Empty;
        /// <summary>
        /// Public property to set and get the title of the item
        /// </summary>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public Xamarin.Forms.Point DeviceLocation
        {
            get; set;
        }

        public static async Task<GeoPoint> GetDeviceLocationAsync()
        {

            var point = await DependencyService.Get<IDeviceDependencyService>().GetDeviceCurrentLocationAsync();
            return point; 
        }

      
    }
}

