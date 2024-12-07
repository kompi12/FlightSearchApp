using FlightSearch.Models;
using FlightSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightSearch.Controllers
{
    [ApiController]
    [Route("/")]
    public class FlightSearchController : ControllerBase
    {

      

        // Dependency Injection to inject the FlightOfferService
      
        [HttpPost(Name = "GetFlightsearch")]
        public async Task<IActionResult> SearchFlights([FromBody] FlightSearchRequest request) {

            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }

            var _flightOfferService = new FlightOfferService();

            var flightOffers =  _flightOfferService.Get5Recipies(request.originLocationCode,
                request.destinationLocationCode,
                request.departureDate,
                request.returnDate,
                request.adults, request.currencyCode);
            if (flightOffers == null)
            {
                return NotFound("No flight offers found.");
            }

            return Ok(flightOffers);


        }
    }
}
