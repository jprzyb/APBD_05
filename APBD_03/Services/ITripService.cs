using APBD_03.Model;
using Microsoft.AspNetCore.Mvc;

namespace APBD_03.Services;

public interface ITripService
{
        IEnumerable<TripCountryClient> GetTrips();
        public int DeleteClient(int id);
        public int AssigneClientToTrip(AssigneClient assigneClient);
}