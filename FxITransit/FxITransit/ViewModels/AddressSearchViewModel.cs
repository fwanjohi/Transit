using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FxITransit.Helpers;
using FxITransit.Models;
using Xamarin.Forms.Maps;

namespace FxITransit.ViewModels
{
    public class AddressSearchViewModel : BaseViewModel
    {
        public AddressSearchViewModel()
        {
            ToAddress= "Sapphire";
            Addresses =new  ObservableRangeCollection<GoogleAddress>();
            StopsFound =new ObservableRangeCollection<Stop>();
        }
        public string ToAddress { get; set; }
        public ObservableRangeCollection<GoogleAddress> Addresses { get; set; }
        public ObservableRangeCollection<Stop> StopsFound { get; set; }

        public async void SearchAddress()
        {
            var ads = await TrackingHelper.Instance.GetAddressPosition(ToAddress);

            Addresses.ReplaceRange(ads);

        }

        public async Task<Destination> SearchDestinationRoutesTo(GoogleAddress googleAddress)
        {
            var stop = new Stop
            {
                Title = googleAddress.RequestedAddress,
                Lat = googleAddress.Lat,
                Lon = googleAddress.Lon,
                //Distance = TrackingHelper.Instance.CalculateDistance(add.Geometry.Location.Lat, add.Geometry.Location.Lng),

            };

            //var stops = Db.SearchStopsNearAddress(googleAddress.Lat, googleAddress.Lon, 0.3, googleAddress.Name);
            var destination = await Db.SearchDestinationRoutesTo(stop);
            
            return destination;
        }
    }
}
