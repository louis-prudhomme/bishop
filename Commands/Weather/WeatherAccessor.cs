using System.Threading.Tasks;
using RestSharp;

namespace Bishop.Commands.Weather;

public class WeatherAccessor
{
    public static string _apiKey { private get; set; } = null!;
    public static string _currentRoute { private get; set; } = "/v1/current.json";

    private static readonly RestClientOptions Options = new("https://api.weatherapi.com/")
    {
        ThrowOnAnyError = true
    };

    private readonly RestClient _client = new(Options);

    public async Task Current()
    {
        var request = new RestRequest()
            .AddQueryParameter("key", _apiKey)
            .AddQueryParameter("q", "Paris")
            .AddQueryParameter("aqi", "no");


        var response = await _client.ExecuteAsync(request);
    }
}