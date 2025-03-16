using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using CoffeeMachine.Services;
using CoffeeMachine.Services.Interfaces;
using RestSharp;
using Xunit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CoffeeMachine.IntegrationTests
{
    public class OpenWeatherServiceTests
    {
        private readonly IOpenWeatherService _openWeatherService;

        public OpenWeatherServiceTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var baseUrl = configuration["OpenWeatherApi:BaseUrl"] ?? throw new ArgumentNullException("OpenWeatherApi:BaseUrl");
            var apiKey = configuration["OpenWeatherApi:ApiKey"] ?? throw new ArgumentNullException("OpenWeatherApi:ApiKey");

            var restClient = new RestClient(new Uri(baseUrl));
            restClient.AddDefaultQueryParameter("appid", apiKey);

            var logger = new LoggerFactory().CreateLogger<OpenWeatherService>();

            _openWeatherService = new OpenWeatherService(restClient, logger);
        }

        [Fact]
        public async Task GetGeoCoordinatesAsync_ValidCity_ReturnsCoordinates()
        {
            // Arrange
            var city = "Melbourne";
            var countryCode = "AU";

            // Act
            var result = await _openWeatherService.GetGeoCoordinatesAsync(city, countryCode);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(city, result[0].Name, ignoreCase: true);
        }

        [Fact]
        public async Task GetGeoCoordinatesAsync_InvalidCity_ReturnsEmptyArray()
        {
            // Arrange
            var city = "InvalidCity";
            var countryCode = "GB";

            // Act & Assert

            var result = await _openWeatherService.GetGeoCoordinatesAsync(city, countryCode);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

        }

        [Fact]
        public async Task GetCurrentWeatherAsync_ValidCoordinates_ReturnsWeather()
        {
            // Arrange
            var lat = -37.8142176;
            var lon = 144.9631608;

            // Act
            var result = await _openWeatherService.GetCurrentWeatherAsync(lat, lon);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Melbourne", result.Name, ignoreCase: true);
        }

        [Fact]
        public async Task GetCurrentWeatherAsync_InvalidCoordinates_ThrowsException()
        {
            // Arrange
            var lat = -189.123456;
            var lon = 458.123456;

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _openWeatherService.GetCurrentWeatherAsync(lat, lon));
        }

        [Fact]
        public async Task GetGeoCoordinatesAsync_ApiUnavailable_ThrowsHttpRequestException()
        {
            // Arrange
            var city = "Melbourne";
            var countryCode = "AU";

            var invalidBaseUrl = "http://invalid.url";
            var restClient = new RestClient(new Uri(invalidBaseUrl));

            var logger = new LoggerFactory().CreateLogger<OpenWeatherService>();
            var openWeatherService = new OpenWeatherService(restClient, logger);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => openWeatherService.GetGeoCoordinatesAsync(city, countryCode));
        }

        
        [Fact]
        public async Task GetGeoCoordinatesAsync_InvalidApiKey_ThrowsHttpRequestException()
        {
            // Arrange
            var city = "Melbourne";
            var countryCode = "AU";

          var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var baseUrl = configuration["OpenWeatherApi:BaseUrl"] ?? throw new ArgumentNullException("OpenWeatherApi:BaseUrl");

            var restClient = new RestClient(new Uri(baseUrl));
            restClient.AddDefaultQueryParameter("appid", "invalidApiKey");

            var logger = new LoggerFactory().CreateLogger<OpenWeatherService>();

            var openWeatherService = new OpenWeatherService(restClient, logger);
            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => openWeatherService.GetGeoCoordinatesAsync(city, countryCode));
        }

    }
}
