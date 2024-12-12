using System.Buffers.Text;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using System;
using FlightSearch.Models;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlightSearch.Services
{
    public class AirportDataService
    {
        private static readonly string baseUrl = "https://en.wikipedia.org/wiki/List_of_airports_by_IATA_airport_code:_";

        public async Task GetAirportData()
        {
            Console.WriteLine("Accessing GetAirportData()");

            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var airportList = new List<AirportData>();

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(10); // Set a timeout for each request
                foreach (var letter in alphabet)
                {
                    var url = $"{baseUrl}{letter}";

                    try
                    {
                        Console.WriteLine($"Fetching data for letter {letter}...");
                        var response = await client.GetStringAsync(url);

                        // Check if the response contains HTML
                        if (response.Contains("404 Not Found"))
                        {
                            Console.WriteLine($"404 Error: Page not found for {letter}");
                            continue;
                        }

                        // Parse the HTML content and extract IATA codes
                        var iataCodes = ExtractIATACodes(response);
                        airportList.AddRange(iataCodes);

                        // Optional: Add delay to avoid rate-limiting issues (429 error)
                        await Task.Delay(1000); // 1 second delay

                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"HTTP error occurred for {letter}: {ex.Message}");
                    }
                    catch (TaskCanceledException)
                    {
                        Console.WriteLine($"Request for {letter} timed out.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error fetching or parsing data for {letter}: {ex.Message}");
                    }
                }
            }

            Console.WriteLine($"Fetched {airportList.Count} airports" + airportList.ToString());
            foreach( var air in airportList ) {
                Console.WriteLine(air.IATA );
            }
        }

        private static List<AirportData> ExtractIATACodes(string htmlContent)
        {
            var airports = new List<AirportData>();
            var lines = htmlContent.Split(new[] { "<tr>" }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                if (line.Contains("<td>"))
                {
                    try
                    {
                        var cells = line.Split(new[] { "<td>" }, StringSplitOptions.None);
                        var airport = new AirportData
                        {
                            IATA = GetInnerText(cells[1]),
                            ICAO = GetInnerText(cells[2]),
                            Name = GetInnerText(cells[3]),
                            Location = GetInnerText(cells[4])
                        };
                        airports.Add(airport);
                    }
                    catch { /* Ignore parsing errors */ }
                }
            }

            return airports;
        }

        private static string GetInnerText(string htmlCell)
        {
            var startTagEnd = htmlCell.IndexOf('>');
            var endTagStart = htmlCell.LastIndexOf('<');

            if (startTagEnd >= 0 && endTagStart > startTagEnd)
            {
                return htmlCell.Substring(startTagEnd + 1, endTagStart - startTagEnd - 1).Trim();
            }
            return string.Empty;
        }
    }
}
