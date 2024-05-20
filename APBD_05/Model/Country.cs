using System.ComponentModel.DataAnnotations;

namespace APBD_05.Model;

public class Country
{
    [Required] public int IdCountry { get; set; }
    [Required, MaxLength(120)] public String Name  { get; set; }
}