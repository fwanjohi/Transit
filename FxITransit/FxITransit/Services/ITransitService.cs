using System.Collections.Generic;
using FxITransit.Models;
using System.Threading.Tasks;
using System;
using Plugin.Geolocator.Abstractions;

namespace FxITransit.Services
{
    public interface ITransitService
    {
        Task<IEnumerable<Agency>> GetAgencyList();

        Task<IEnumerable<Route>> GetRouteList(Agency agency);

        
        //Task  PopulateRouteList(Agency agency);
        Task GetRouteDetailsFromService(Route route, Action callBack);

        Position LastPosition { get; }

        void GetPredictionsFromService(IList<Stop> stop);

    }
}