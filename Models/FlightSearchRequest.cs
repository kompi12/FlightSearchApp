namespace FlightSearch.Models
{
    public class FlightSearchRequest
    {

        public string originLocationCode { get; set; }
        public string destinationLocationCode { get; set; }
        public string departureDate { get; set; }
        public string returnDate { get; set; }
        public int adults { get; set; }

        public string currencyCode { get; set;  }
    }
}
