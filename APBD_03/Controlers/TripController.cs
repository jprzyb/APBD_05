using APBD_03.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_03.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripController : ControllerBase
{
    private ITripService _tripService;
    
    public TripController(ITripService tripService)
    {
        _tripService = tripService;
    }
    
    /// <summary>
    /// Endpoint used to return list of trips sorted descending from trip DateFrom.
    /// </summary>
    /// <returns>List of trips</returns>
    [HttpGet]
    public IActionResult GetTrips()
    {
        var tripCountryClients = _tripService.GetTrips();
        return Ok(tripCountryClients);
    }
}