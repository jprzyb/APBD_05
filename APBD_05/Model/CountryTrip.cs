using System.ComponentModel.DataAnnotations;

namespace APBD_05.Model;

public class CountryTrip
{
    [Required] public int IdClient { get; set; }
    [Required] public int IdTrip { get; set; }
    [Required] public int RegisteredAt  { get; set; }
    public int PaymentDate  { get; set; }

}