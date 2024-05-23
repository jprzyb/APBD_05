using System.ComponentModel.DataAnnotations;

namespace APBD_03.Model;

public class TripCountryClient
{ 
    [Required, MaxLength(120)] public String Name  { get; set; }
    [Required, MaxLength(120)] public String Description  { get; set; }
    [Required] public DateTime DateFrom  { get; set; }
    [Required] public DateTime DateTo  { get; set; }
    [Required] public int MaxPeople  { get; set; }
    [Required] public List<Country> Countries { get; set; }
    [Required] public List<Client> Clients { get; set; }
}