using System.ComponentModel.DataAnnotations;

namespace APBD_03.Model;

public class ClientTrip
{
    [Required] public int IdClient { get; set; }
    [Required] public int IdTrip { get; set; }
    [Required] public DateTime RegisteredAt { get; set; }
    public DateTime PaymentDate { get; set; }
}