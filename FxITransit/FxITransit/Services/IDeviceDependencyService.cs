using FxITransit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FxITransit.Services
{
    public interface IDeviceDependencyService
    {
        void Speak(string text);
        Task<GeoPoint> GetDeviceCurrentLocationAsync();
        //event EventHandler<LocationEventArgs> OnlocationObtained;
    }

   

    public class LocationEventArgs : GeoPoint
    {
    }
}
