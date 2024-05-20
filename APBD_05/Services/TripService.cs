using APBD_05.Model;
using APBD_05.Repositories;

namespace APBD_05.Services;

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
}