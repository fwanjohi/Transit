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
    public class DestinationViewModel : BaseViewModel
    {
        public DestinationViewModel()
        {
            ToAddress= "Aces";
            Addresses =new  ObservableRangeCollection<GoogleAddress>();
            StopsFound =new ObservableRangeCollection<StopLite>();
        }
        public string ToAddress { get; set; }
        public ObservableRangeCollection<GoogleAddress> Addresses { get; set; }
        public ObservableRangeCollection<StopLite> StopsFound { get; set; }

        public async void SearchAddress()
        {
            var ads = await TrackingHelper.Instance.GetAddressPosition(ToAddress);

            Addresses.ReplaceRange(ads);

        }

        public async  Task<List<StopLite>> SearchStops(GoogleAddress googleAddress)
        {
            var  stops =  Db.SearchStopsNearAddress(googleAddress.Lat, googleAddress.Lon, 0.5, googleAddress.Name);
            return stops;
        }
    }
}
