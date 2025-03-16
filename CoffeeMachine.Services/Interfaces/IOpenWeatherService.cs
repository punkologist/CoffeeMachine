using System;
using CoffeeMachine.Services.Models;

namespace CoffeeMachine.Services.Interfaces;

public interface IOpenWeatherService
{
   Task<OpenWeatherResult> GetCurrentWeatherAsync(double lat, double lon);
   Task<GeoLocation[]> GetGeoCoordinatesAsync(string city, string countryCode,string? state = null);
}