using System;
using CoffeeMachine.Services.Interfaces;
using CoffeeMachine.Services.Models;
using Microsoft.Extensions.Configuration;

namespace CoffeeMachine.Services;

public class CoffeeMachineService(IDateTimeProviderService dateTimeProviderService, IOpenWeatherService openWeatherService, IConfiguration configuration) : ICoffeeMachineService
{
    private readonly IDateTimeProviderService dateTimeProviderService = dateTimeProviderService;
    private readonly IOpenWeatherService openWeatherService = openWeatherService;

    private readonly IConfiguration configuration = configuration;

    public async Task<BrewCoffeeResult> BrewCoffeeAsync()
    {

        
        try
        {

            // check if it is April 1st
            var now = dateTimeProviderService.Now;
            if (now.Date.Month == 4 && now.Date.Day == 1)
            {
                return new BrewCoffeeResult
                {
                    IsSuccess = false,
                    ResultType = ResultType.AprilFools
                };
            }

            //in a real app you would probabaly want to get the geolocation from a user or some other source like location services
            var city = configuration["OpenWeatherApi:City"] ?? throw new ArgumentNullException("OpenWeatherApi:City");
            var countryCode = configuration["OpenWeatherApi:CountryCode"] ?? throw new ArgumentNullException("OpenWeatherApi:CountryCode");

            var geoCoordinates = await openWeatherService.GetGeoCoordinatesAsync(city, countryCode);

            if (geoCoordinates == null || geoCoordinates.Length == 0)
            {
                return new BrewCoffeeResult
                {
                    IsSuccess = false,
                    ResultType = ResultType.Error,
                    ErrorMessage = "Failed to get geo coordinates for the city."
                };
            }
            // Get current weather
            var currentWeather = await openWeatherService.GetCurrentWeatherAsync(geoCoordinates[0].Lat, geoCoordinates[0].Lon);

            var coffeeType = currentWeather.Main.Temp > 30 ? CoffeeType.Iced : CoffeeType.PipingHot;

            return new BrewCoffeeResult
            {
                IsSuccess = true,
                ResultType = ResultType.Success,
                CoffeeType = coffeeType,
                SuccessMessage = $"Your {coffeeType} coffee is ready",
                PreparedAt = now
            };
        }
        catch (Exception ex)
        {
            return new BrewCoffeeResult
            {
                IsSuccess = false,
                ResultType = ResultType.Error,
                ErrorMessage = ex.Message,
                Exception = ex
            };
        }


    }

}

