using FxITransit.Services.NextBus;
using FxITransit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Windows.Devices.Geolocation;
using FxITransit.UWP;
using Windows.UI.Xaml.Controls;
using Windows.Media.SpeechSynthesis;
using System.Threading;

[assembly: Dependency(typeof(DeviceDependencyServiceImplementation))]
namespace FxITransit.UWP
{
    public class DeviceDependencyServiceImplementation : IDeviceDependencyService
    {
        public async Task<Point> GetDeviceCurrentLocationAsync()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();

            try
            {
                // Request permission to access location
                

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:

                        // Get cancellation token
                        var _cts = new CancellationTokenSource();
                        CancellationToken token = _cts.Token;

                        //_rootPage.NotifyUser("Waiting for update...", NotifyType.StatusMessage);

                        // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                        Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };

                        // Carry out the operation
                        Geoposition pos = await geolocator.GetGeopositionAsync().AsTask(token);

                        //UpdateLocationData(pos);
                        //_rootPage.NotifyUser("Location updated.", NotifyType.StatusMessage);
                        break;

                    case GeolocationAccessStatus.Denied:
                        //_rootPage.NotifyUser("Access to location is denied.", NotifyType.ErrorMessage);
                        //LocationDisabledMessage.Visibility = Visibility.Visible;
                        //UpdateLocationData(null);
                        break;

                    case GeolocationAccessStatus.Unspecified:
                        //_rootPage.NotifyUser("Unspecified error.", NotifyType.ErrorMessage);
                        //UpdateLocationData(null);
                        break;
                }
            }
            catch (TaskCanceledException)
            {
                //_rootPage.NotifyUser("Canceled.", NotifyType.StatusMessage);
            }
            catch (Exception ex)
            {
                //_rootPage.NotifyUser(ex.ToString(), NotifyType.ErrorMessage);
            }
            finally
            {
                //_cts = null;
            }

            return new Point(0, 0);
        }

        public async void Speak(string text)
        {
            MediaElement mediaElement = new MediaElement();

            var synth = new SpeechSynthesizer();

            var stream = await synth.SynthesizeTextToStreamAsync(text);

            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }
    }
}
