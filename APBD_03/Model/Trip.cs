using System.ComponentModel.DataAnnotations;

namespace APBD_03.Model;

public class Trip
{
    [Required] public int IdTrip { get; set; }
    [Required, MaxLength(120)] public String Name  { get; set; }
    [Required, MaxLength(120)] public String Description  { get; set; }
    [Required] public DateTime DateFrom  { get; set; }
    [Required] public DateTime DateTo  { get; set; }
    [Required] public int MaxPeople  { get; set; }
}