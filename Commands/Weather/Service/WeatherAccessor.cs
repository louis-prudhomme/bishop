using System;
using System.Threading.Tasks;
using Bishop.Commands.Weather.Domain;
using RestSharp;

namespace Bishop.Commands.Weather.Service;

public class WeatherAccessor
{
    private const string CurrentRoute = "/v1/current.json";
    public const long CacheForSeconds = 14400;

    private static readonly string ApiKey = Environment
        .GetEnvironmentVariable("WEATHER_API_KEY")!;

    private static readonly RestClientOptions Options = new("https://api.weatherapi.com/")
    {
        ThrowOnAnyError = true
    };

    private static readonly RestClient Client = new(Options);

    private static async Task<WeatherEntity> Current(string city)
    {
        var request = new RestRequest(CurrentRoute)
            .AddQueryParameter("key", ApiKey)
            .AddQueryParameter("q", city)
            .AddQueryParameter("aqi", "no");

        var response = await Client.ExecuteAsync<WeatherDTO>(request);

        if (response.Data == null) throw new Exception("weather");

        return WeatherAdapter.FromDtoToEntity(response.Data);
    }

    public static WeatherEntity CurrentSync(string city) => Current(city).Result;
}