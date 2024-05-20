using APBD_05.Model;

namespace APBD_05.Services;

public interface ITripService
{
    public interface IAnimalService
    {
        IEnumerable<TripCountryClient> GetAnimals();
    }
}