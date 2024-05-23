using APBD_03.Model;

namespace APBD_03.Repositories;

public interface ITripRepository
{
    IEnumerable<TripCountryClient> GetTrips();
}