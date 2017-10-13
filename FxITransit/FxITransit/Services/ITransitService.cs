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

        Task GetStopPredictions(Stop stop);
        //Task  PopulateRouteList(Agency agency);
        Task PopulateRouteDetails(Route route, Action callBack);

        Position LastPosition { get; }

        void UpdatePredictions(IList<Stop> stop);

    }
}