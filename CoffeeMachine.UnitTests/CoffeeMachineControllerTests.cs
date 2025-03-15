using CoffeeMachine.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using NSubstitute;
using CoffeeMachine.Services.Interfaces;
using CoffeeMachine.Services;

namespace CoffeeMachine.UnitTests
{
public class CoffeeMachineControllerTests
{
        [Fact]
        public void BrewCoffee_RequestCountLessThan5_ReturnsOk()
        {
            // Arrange
            var controller = new CoffeeMachineController(new DateTimeProviderService());
            CoffeeMachineController._brewCoffeeRequestCount = 0;

            // Act
            var result = controller.BrewCoffee() as ObjectResult;

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void BrewCoffee_RequestCount5OrMore_ReturnsServiceUnavailable()
        {
            // Arrange
            var controller = new CoffeeMachineController(new DateTimeProviderService());
            CoffeeMachineController._brewCoffeeRequestCount = 5;

            // Act
            var result = controller.BrewCoffee() as StatusCodeResult;

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status503ServiceUnavailable, result.StatusCode);
        }

        [Fact]
        public void BrewCoffee_AprilFoolsDay_ReturnsTeapot()
        {
            // Arrange
            var dateTimeProviderService = NSubstitute.Substitute.For<IDateTimeProviderService>();
            dateTimeProviderService.Now.Returns(new DateTimeOffset(new DateTime(DateTime.Now.Year, 4, 1)));
            var controller = new CoffeeMachineController(dateTimeProviderService);
            CoffeeMachineController._brewCoffeeRequestCount = 0;
          
            // Act
            var result = controller.BrewCoffee() as StatusCodeResult;

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status418ImATeapot, result.StatusCode);
        }
    }
}