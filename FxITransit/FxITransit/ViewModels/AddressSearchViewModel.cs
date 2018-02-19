using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FxITransit.Helpers;
using FxITransit.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FxITransit.ViewModels
{
    public class AddressSearchViewModel : BaseViewModel
    {
        private bool _useMyLocation;
        private GoogleAddress _myLocation;
        private GoogleAddress _selectedFromAddress;

        public AddressSearchViewModel()
        {
            ToAddress = "Sapphire";
            Addresses = new ObservableRangeCollection<GoogleAddress>();

            _myLocation = new GoogleAddress { FormattedAddress = "My Current location", Name = "My Current Location" };
            UseMyLocation = true;
        }
        public string ToAddress { get; set; }
        public string FromAddress { get; set; }

        public string SearchTitle { get; set; }
        public string FromLabel { get; set; }

        public ObservableRangeCollection<GoogleAddress> Addresses { get; set; }

        public GoogleAddress SelectedFromAddress
        {
            get { return _selectedFromAddress; }
            set
            {
                _selectedFromAddress = value;
                OnPropertyChanged("SelectedFromAddress");
            }
        }
        public GoogleAddress SelectedToAddress { get; set; }

        public bool IsCanceled { get; set; }

        public async Task<List<GoogleAddress>> SearchAddress(string address)
        {
            UserDialogs.Instance.ShowLoading($"Searching address for {address}", MaskType.Black);
            await Task.Delay(5);
            var adds = await TrackingHelper.Instance.GetAddressPosition(address);

             UserDialogs.Instance.HideLoading();

            return adds;
        }

        public bool CustomFromAddress { get { return !UseMyLocation; } }
        public bool UseMyLocation
        {
            get { return _useMyLocation; }
            set
            {
                _useMyLocation = value;
                if (value)
                {
                    SelectedFromAddress = _myLocation;
                }

                FromLabel = value ? "Use my location" : "Enter a location below";
                
                OnPropertyChanged("UseMyLocation");
                OnPropertyChanged("CustomFromAddress");
                OnPropertyChanged("SelectedFromAddress");
                OnPropertyChanged("FromLabel");
            }
        }

        public async Task<Destination> SearchDestinationRoutesTo(GoogleAddress fromGoogleAddress, GoogleAddress toGoogleAddress)
        {
            var destStop = new Stop
            {
                Title = toGoogleAddress.RequestedAddress,
                Lat = toGoogleAddress.Lat,
                Lon = toGoogleAddress.Lon,
                //Distance = TrackingHelper.Instance.CalculateDistance(add.Geometry.Location.Lat, add.Geometry.Location.Lng),

            };

            Stop fromStop =   new Stop
            {

                Title = fromGoogleAddress.RequestedAddress,
                Lat = fromGoogleAddress.Lat,
                Lon = fromGoogleAddress.Lon,
                //Distance = TrackingHelper.Instance.CalculateDistance(add.Geometry.Location.Lat, add.Geometry.Location.Lng),

            };

            //var stops = Db.SearchStopsNearAddress(googleAddress.Lat, googleAddress.Lon, 0.3, googleAddress.Name);
            UserDialogs.Instance.ShowLoading($"Calculating possible routes from {fromGoogleAddress.Name}  to {toGoogleAddress.Name}", MaskType.Black);
            await Task.Delay(1);
            var destination = await Db.SearchDestinationRoutesTo(fromStop, destStop, fromGoogleAddress.Name, toGoogleAddress.FormattedAddress);
            UserDialogs.Instance.HideLoading();
            return destination;
        }

        public void UpdateProperties()
        {
            OnPropertyChanged("SelectedFromAddress");
            OnPropertyChanged("SelectedToAddress");
        }
    }
}
