using System.ComponentModel.DataAnnotations;

namespace APBD_05.Model;

public class ClientTrip
{
    [Required] public int IdCountry { get; set; }
    [Required] public int IdTrip { get; set; }
}