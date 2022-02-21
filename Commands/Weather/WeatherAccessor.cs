using System;
using System.Threading.Tasks;
using RestSharp;

namespace Bishop.Commands.Weather;

public class WeatherAccessor
{
    public static string _apiKey { private get; set; } = null!;

    public const string CurrentRoute = "/v1/current.json";

    private static readonly RestClientOptions Options = new("https://api.weatherapi.com/")
    {
        ThrowOnAnyError = true
    };

    private readonly RestClient _client = new(Options);

    public async Task<WeatherEntity> Current(string city)
    {
        var request = new RestRequest(CurrentRoute)
            .AddQueryParameter("key", _apiKey)
            .AddQueryParameter("q", city)
            .AddQueryParameter("aqi", "no");
        
        var response = await _client.ExecuteAsync<WeatherDTO>(request);

        if (response.Data == null) throw new Exception("weather");
        
        return WeatherAdapter.FromDtoToEntity(response.Data);
    }
}