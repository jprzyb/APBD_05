using APBD_03.Model;
using APBD_03.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APBD_03.Services;

public class TripService : ITripService
{
    private readonly ITripRepository _tripRepository;

    public TripService(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }
    public IEnumerable<TripCountryClient> GetTrips()
    {
        //Business logic
        return _tripRepository.GetTrips();
    }

    public int DeleteClient(int id)
    {
        return _tripRepository.DeleteClient(id);
    }

    public int AssigneClientToTrip(AssigneClient assigneClient)
    {
        return _tripRepository.AssigneClientToTrip(assigneClient);
    }
}