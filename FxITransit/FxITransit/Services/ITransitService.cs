using System.Collections.Generic;
using FxITransit.Models;
using System.Threading.Tasks;

namespace FxITransit.Services
{
    public interface ITransitService
    {
        Task<IEnumerable<Agency>> GetAgencyList();
       
        Task<IEnumerable<Route>> GetRouteList(string agencyTag);
        Task GetStopPredictions(Stop stop);
        //Task  PopulateRouteList(Agency agency);
        Task PopulateRouteDetails(Route route);
    }
}