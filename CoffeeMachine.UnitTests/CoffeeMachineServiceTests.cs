using System;
using System.Threading.Tasks;
using CoffeeMachine.Services;
using CoffeeMachine.Services.Interfaces;
using CoffeeMachine.Services.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace CoffeeMachine.UnitTests
{
    public class CoffeeMachineServiceTests
    {
        IDateTimeProviderService _dateTimeProviderService;
        IOpenWeatherService _openWeatherService;
        IConfiguration _configuration;
        CoffeeMachineService _coffeeMachineService;
        ILogger<CoffeeMachineService> _logger;

        public CoffeeMachineServiceTests()
        {
            _dateTimeProviderService = Substitute.For<IDateTimeProviderService>();
            _openWeatherService = Substitute.For<IOpenWeatherService>();
            _configuration = Substitute.For<IConfiguration>();
            _logger = Substitute.For<ILogger<CoffeeMachineService>>();
            _coffeeMachineService = new CoffeeMachineService(_dateTimeProviderService, _openWeatherService, _configuration, _logger);
        }


        [Fact]
        public async Task BrewCoffeeAsync_ReturnsAprilFools_OnAprilFirst()
        {
            // Arrange
            _dateTimeProviderService.Now.Returns(new DateTimeOffset(new DateTime(2025, 4, 1)));

            // Act
            var result = await _coffeeMachineService.BrewCoffeeAsync();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultType.AprilFools, result.ResultType);
        }

        [Fact]
        public async Task BrewCoffeeAsync_ReturnsError_WhenGeoCoordinatesNotFound()
        {
            // Arrange
            _dateTimeProviderService.Now.Returns(new DateTimeOffset(new DateTime(2025, 3, 16)));
            _configuration["OpenWeatherApi:City"].Returns("InvalidCity");
            _configuration["OpenWeatherApi:CountryCode"].Returns("GB");
            _openWeatherService.GetGeoCoordinatesAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(Task.FromResult<GeoLocation[]>([]));

            // Act
            var result = await _coffeeMachineService.BrewCoffeeAsync();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultType.Error, result.ResultType);
            Assert.Equal("Failed to get geo coordinates for the city.", result.ErrorMessage);
        }

        [Fact]
        public async Task BrewCoffeeAsync_ReturnsSuccess_WhenWeatherIsHot()
        {
            // Arrange
            _dateTimeProviderService.Now.Returns(new DateTimeOffset(new DateTime(2025, 3, 16)));
            _configuration["OpenWeatherApi:City"].Returns("Melbourne");
            _configuration["OpenWeatherApi:CountryCode"].Returns("AU");
            _openWeatherService.GetGeoCoordinatesAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(Task.FromResult(new[] { new GeoLocation { Lat = -37.814, Lon = 144.96332, Name = "Melbourne", Country = "AU" } }));
            _openWeatherService.GetCurrentWeatherAsync(Arg.Any<double>(), Arg.Any<double>()).Returns(Task.FromResult(new OpenWeatherResult
            {
                Main = new Main { Temp = 35 },
                Coord = new Coord { Lat = -37.814, Lon = 144.96332 },
                Timezone = 36000,
                Id = 2158177,
                Name = "Melbourne"
            }));

            // Act
            var result = await _coffeeMachineService.BrewCoffeeAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResultType.Success, result.ResultType);
            Assert.Equal(CoffeeType.Iced.ToString(), result.CoffeeType.ToString());
            Assert.Equal($"Your {CoffeeType.Iced} coffee is ready", result.SuccessMessage);
        }

        [Fact]
        public async Task BrewCoffeeAsync_ReturnsSuccess_WhenWeatherIsCold()
        {
            // Arrange
            _dateTimeProviderService.Now.Returns(new DateTimeOffset(new DateTime(2025, 3, 16)));
            _configuration["OpenWeatherApi:City"].Returns("Melbourne");
            _configuration["OpenWeatherApi:CountryCode"].Returns("AU");
            _openWeatherService.GetGeoCoordinatesAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(Task.FromResult(new[] { new GeoLocation { Lat = -37.814, Lon = 144.96332, Name = "Melbourne", Country = "AU" } }));
            _openWeatherService.GetCurrentWeatherAsync(Arg.Any<double>(), Arg.Any<double>()).Returns(Task.FromResult(new OpenWeatherResult {
                Main = new Main { Temp = 15 },
                Coord = new Coord { Lat = -37.814, Lon = 144.96332 },
                Timezone = 36000,
                Id = 2158177,
                Name = "Melbourne"
            }));

            // Act
            var result = await _coffeeMachineService.BrewCoffeeAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ResultType.Success, result.ResultType);
            Assert.Equal(CoffeeType.PipingHot.ToString(), result.CoffeeType.ToString());
            Assert.Equal($"Your {CoffeeType.PipingHot} coffee is ready", result.SuccessMessage);
        }
    }
}
