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
            ToAddress= "Union Square Sports Bar";
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

        public async  Task<List<Stop>> SearchStops(GoogleAddress googleAddress)
        {
            var  stops =  Db.SearchStopsNearAddress(googleAddress.Lat, googleAddress.Lon, 0.5, googleAddress.Name);
            foreach(var stop in stops)
            {
                stop.IsDestinationStart = true;
            }
            return stops;
        }
    }
}
