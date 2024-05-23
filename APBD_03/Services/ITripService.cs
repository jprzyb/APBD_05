using APBD_03.Model;

namespace APBD_03.Services;

public interface ITripService
{
        IEnumerable<TripCountryClient> GetTrips();
}