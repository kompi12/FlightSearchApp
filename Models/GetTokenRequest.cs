namespace FlightSearch.Models
{
    public class GetTokenRequest
    {

        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string grant_type { get; set; }
    }
}
