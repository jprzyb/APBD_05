using APBD_03.Model;
using Microsoft.AspNetCore.Mvc;

namespace APBD_03.Repositories;

public interface ITripRepository
{
    IEnumerable<TripCountryClient> GetTrips();
    int DeleteClient(int id);
    int AssigneClientToTrip(AssigneClient assigneClient);
}