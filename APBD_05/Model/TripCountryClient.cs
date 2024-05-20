using System.ComponentModel.DataAnnotations;

namespace APBD_05.Model;

public class TripCountryClient
{
    [Required] public int IdTrip { get; set; }
    [Required] public int IdCountry { get; set; }
    [Required] public int IdClient { get; set; }
}