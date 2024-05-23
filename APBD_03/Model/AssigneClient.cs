using System.ComponentModel.DataAnnotations;

namespace APBD_03.Model;

public class AssigneClient
{
    [Required, MaxLength(120)] public string FirstName { set; get; }
    [Required, MaxLength(120)] public string LastName { set; get; }
    [Required, MaxLength(120), EmailAddress] public string Email { set; get; }
    [Required, MaxLength(120)] public string Telephone { set; get; }
    [Required, MaxLength(120)] public string Pesel { set; get; }
    [Required, MaxLength(120)] public int IdTrip { set; get; }
    [Required, MaxLength(120)] public string TripName { set; get; }
    [Required, MaxLength(120)] public DateTime PaymentDate { set; get; }
}