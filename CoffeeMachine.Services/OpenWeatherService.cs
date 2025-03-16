using System;
using CoffeeMachine.Services.Interfaces;
using RestSharp;
using Microsoft.Extensions.Configuration;
using CoffeeMachine.Services.Models;
using Microsoft.Extensions.Logging;

namespace CoffeeMachine.Services;

public class OpenWeatherService(RestClient restClient, ILogger<OpenWeatherService> logger) : IOpenWeatherService
{

    private readonly RestClient restClient = restClient;
    private readonly ILogger<OpenWeatherService> logger = logger;

    public async Task<OpenWeatherResult> GetCurrentWeatherAsync(double lat, double lon)
    {
       var request = new RestRequest("/data/2.5/weather", Method.Get);
        request.AddQueryParameter("lat", lat.ToString());
        request.AddQueryParameter("lon", lon.ToString());
        request.AddQueryParameter("units", "metric");
        try
        {
            var result = await restClient.ExecuteGetAsync<OpenWeatherResult>(request);
            if (result != null && result.IsSuccessStatusCode && result.Data is OpenWeatherResult)
            {
                return result.Data;
            }
            if(result != null && !result.IsSuccessStatusCode){
                var statusCode = result.StatusCode;
                var responseMessage = result.Content;

                throw new InvalidOperationException($"Failed to get current weather from OpenWeather API. Status code: {statusCode}, Response: {responseMessage}");

            }
            
            throw new InvalidOperationException("Failed to get current weather from OpenWeather API.");
        }
        catch(InvalidOperationException ex){
            logger.LogError(ex, "Failed to get current weather from OpenWeather API.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get current weather from OpenWeather API.");
            throw new InvalidOperationException("Failed to get current weather from OpenWeather API.", ex);
        }
    }

    public async Task<GeoLocation[]> GetGeoCoordinatesAsync(string city, string countryCode, string? state = null)
    {
      
        var request = new RestRequest("/geo/1.0/direct", Method.Get);
        if (state != null)
        {
            request.AddQueryParameter("q", $"{city},{state},{countryCode}");
        }
        else
        {
            request.AddQueryParameter("q", $"{city},{countryCode}");
        }

        try
        {
            var result = await restClient.ExecuteGetAsync<GeoLocation[]>(request);
            if (result != null && result.IsSuccessStatusCode && result.Data is GeoLocation[])
            {
                
                if (result.Data != null && result.Data.Length > 0)
                {
                    return result.Data;

                }


            }
            // If no geo coordinates are found, return an empty array
           return [];

        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to get geo coordinates from OpenWeather API.", ex);
        }


    }


}
