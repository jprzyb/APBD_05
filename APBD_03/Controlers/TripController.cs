using APBD_03.Model;
using APBD_03.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_03.Controllers;

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
    [HttpGet("api/trips")]
    
    public IEnumerable<TripCountryClient> GetTrips()
    {
        var tripCountryClients = _tripService.GetTrips();
        return tripCountryClients;
    }
    
    /// <summary>
    /// Endpoint used to delete a client.
    /// </summary>
    /// <param name="id">Id of a client</param>
    /// <returns>204 No Content</returns>
    [HttpDelete("api/clients/{idClient:int}")]
    public IActionResult DeleteClient(int idClient)
    {
        var affectedCount = _tripService.DeleteClient(idClient);
        return NoContent();
    }
    
    /// <summary>
    /// Endpoint used to assigne client to a trip.
    /// </summary>
    /// <param name="id">Id of a trip</param>
    /// <param name="client">Client data</param>
    /// <returns>204 No Content</returns>
    [HttpPost("/api/trips/{idTrip:int}/clients")]
    public IActionResult AssigneClient(AssigneClient assigneClient)
    {
        var affectedCount = _tripService.AssigneClientToTrip(assigneClient);
        return StatusCode(StatusCodes.Status201Created);
    }
}