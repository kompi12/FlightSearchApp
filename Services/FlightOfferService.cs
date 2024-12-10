using System.Net.Http;
using System.Net.Http.Headers;
using FlightSearch.Models;
using Newtonsoft.Json;

namespace FlightSearch.Services
{
    public class FlightOfferService 
    {

        public  FlightOfferResponse GetFlightResponse(string orgLocCode,string destLocCode,string departureDate, string returnDate, int numOfAdults, string currencyCode)


        {
            try
            {
               
                var url = $"https://test.api.amadeus.com/v2/shopping/flight-offers";
                var parameters = $"?originLocationCode={orgLocCode}&destinationLocationCode={destLocCode}&departureDate={departureDate}&returnDate={returnDate}&adults={numOfAdults}&max=5&currencyCode={currencyCode}";

                HttpClient client = new HttpClient();

                var tokenUrl = "https://test.api.amadeus.com/v1/security/oauth2/token";
                var accessTokenService = new GetAccessTokenService(client, tokenUrl);

                var acces_token = accessTokenService.GetAccesTokenAsync("a2SKI3DGmRpjkPxeZe7v0OQoxGPx6myY", "1afckTkGtBBxGdFB");

                HttpClient _client = new HttpClient();
                Console.WriteLine(acces_token.ToString());
                _client.BaseAddress = new Uri(url);
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", acces_token.ToString());
               
                Console.WriteLine(_client.BaseAddress.ToString() + parameters);
                Console.WriteLine(_client.ToString());


                HttpResponseMessage response = _client.GetAsync(parameters).GetAwaiter().GetResult();
                Console.WriteLine(response.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var jsonString =  response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    Console.WriteLine(jsonString);
                    var flights = JsonConvert.DeserializeObject<FlightOfferResponse>(jsonString);
                    return flights;

                }
                else
                {

                    return null;
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message.ToString());
                throw new Exception(ex.Message);
            }
        }

    }
}
