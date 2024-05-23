using System.ComponentModel.DataAnnotations;

namespace APBD_03.Model;

public class Client
{
    [Required] public int IdClient { get; set; }
    [Required, MaxLength(120)] public String FirstName  { get; set; }
    [Required, MaxLength(120)] public String LastName  { get; set; }
    [Required, MaxLength(120)] public String Email  { get; set; }
    [Required, MaxLength(120)] public String Telephone  { get; set; }
    [Required, MaxLength(120)] public String Pesel  { get; set; }
}