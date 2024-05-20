using APBD_05.Model;

namespace APBD_05.Repositories;

public interface ITripRepository
{
    IEnumerable<TripCountryClient> GetTrips();
}