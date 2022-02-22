using System;
using System.Threading.Tasks;
using RestSharp;

namespace Bishop.Commands.Weather;

public class WeatherAccessor
{
    private static readonly string ApiKey = Environment
        .GetEnvironmentVariable("WEATHER_API_KEY")!;

    private const string CurrentRoute = "/v1/current.json";

    private static readonly RestClientOptions Options = new("https://api.weatherapi.com/")
    {
        ThrowOnAnyError = true
    };

    private readonly RestClient _client = new(Options);

    public async Task<WeatherEntity> Current(string city)
    {
        var request = new RestRequest(CurrentRoute)
            .AddQueryParameter("key", ApiKey)
            .AddQueryParameter("q", city)
            .AddQueryParameter("aqi", "no");
        
        var response = await _client.ExecuteAsync<WeatherDTO>(request);

        if (response.Data == null) throw new Exception("weather");
        
        return WeatherAdapter.FromDtoToEntity(response.Data);
    }
}