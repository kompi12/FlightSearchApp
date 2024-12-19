using System.Text.Json;
using FlightSearch.Models;

namespace FlightSearch.Services
{
    public class GetAccessTokenService
    {
        private readonly HttpClient _httpClient;
        private readonly string _tokenUrl;

        public GetAccessTokenService(HttpClient httpClient, string tokenUrl) {
            _httpClient = httpClient;
            _tokenUrl = tokenUrl;
        }


        public  string GetAccesTokenAsync(string clientId, string clientSecret)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _tokenUrl);
                var collection = new List<KeyValuePair<string, string>>();
                collection.Add(new("client_id", "a2SKI3DGmRpjkPxeZe7v0OQoxG"));
                collection.Add(new("client_secret", "1afckTkGtBBxGdFB"));
                collection.Add(new("grant_type", "client_credentials"));
                var content = new FormUrlEncodedContent(collection);
                request.Content = content;
                var responseT = _httpClient.SendAsync(request).GetAwaiter().GetResult();
                responseT.EnsureSuccessStatusCode();
                if (responseT.IsSuccessStatusCode)
                {
                    var responseContent = responseT.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var tokenResponse = JsonSerializer.Deserialize<GetTokenResponse>(responseContent);
                    return tokenResponse!.access_token;
                }
                else {

                    throw new Exception($"Failed to retrieve access token: {responseT.StatusCode}");
                }

                //var simplePayload = new
                //{
                //    client_id = clientId,
                //    client_secret = clientSecret,
                //    grant_type = "client_credentials"
                //};

            //var jsonRequest = JsonContent.Create(simplePayload);

            //Console.WriteLine(jsonRequest);
            //var response =  _httpClient.PostAsync(_tokenUrl, jsonRequest).GetAwaiter().GetResult();
            //Console.WriteLine($"Error: {response}");

            //if (response.IsSuccessStatusCode) {


            //    var responseContent = await response.Content.ReadAsStringAsync();
            //    var tokenResponses = JsonSerializer.Deserialize<GetTokenResponse>(responseContent);
            //    return tokenResponses?.access_token;



            //} else { throw new Exception($"Failed to retrieve access token: {response.StatusCode}"); }


            }
            catch (Exception ex) { throw new Exception($"Error while requesting access token: {ex.Message}", ex); }
        }
    }
}

