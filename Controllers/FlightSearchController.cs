using FlightSearch.Models;
using FlightSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightSearch.Controllers
{
    [ApiController]
    [Route("/")]
    public class FlightSearchController : ControllerBase
    {
        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Append("Access-Control-Allow-Origin", "http://localhost:3000");
            Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");
            return Ok();
        }


        // Dependency Injection to inject the FlightOfferService

        [HttpPost(Name = "GetFlightsearch")]
        public async Task<IActionResult> SearchFlights([FromBody] FlightSearchRequest request) {

            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }

            var _flightOfferService = new FlightOfferService();

            var flightOffers =  _flightOfferService.GetFlightResponse(request.originLocationCode,
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
