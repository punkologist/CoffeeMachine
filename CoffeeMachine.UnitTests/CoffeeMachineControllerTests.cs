using CoffeeMachine.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using NSubstitute;
using CoffeeMachine.Services.Interfaces;
using CoffeeMachine.Services;
using System.Threading.Tasks;
using CoffeeMachine.Services.Models;
using NSubstitute.ExceptionExtensions;

namespace CoffeeMachine.UnitTests
{
public class CoffeeMachineControllerTests
{
        [Fact]
        public async Task BrewCoffee_RequestIsNotFifth_ReturnsOk()
        {
            // Arrange
            var coffeeMachineService = NSubstitute.Substitute.For<ICoffeeMachineService>(); 
            var brewCoffeeResult = new BrewCoffeeResult { 
                IsSuccess = true, 
                ResultType = ResultType.Success,
                CoffeeType = CoffeeType.PipingHot, 
                SuccessMessage = "Your PipingHot coffee is ready", 
                PreparedAt = new DateTimeOffset(new DateTime(DateTime.Now.Year, 3, 31)) };

            coffeeMachineService.BrewCoffeeAsync().Returns(brewCoffeeResult);
            var controller = new CoffeeMachineController(coffeeMachineService);

            CoffeeMachineController._coffeeOrders = 0;

            // Act
            var result = await controller.BrewCoffee() as ObjectResult;

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task BrewCoffee_RequestIsFifth_ReturnsServiceUnavailable()
        {
            // Arrange
            var coffeeMachineService = NSubstitute.Substitute.For<ICoffeeMachineService>(); 
            var brewCoffeeResult = new BrewCoffeeResult { 
                IsSuccess = true, 
                ResultType = ResultType.Success,
                CoffeeType = CoffeeType.PipingHot, 
                SuccessMessage = "Your PipingHot coffee is ready", 
                PreparedAt = new DateTimeOffset(new DateTime(DateTime.Now.Year, 3, 31)) };

            coffeeMachineService.BrewCoffeeAsync().Returns(brewCoffeeResult);
            var controller = new CoffeeMachineController(coffeeMachineService);
            CoffeeMachineController._coffeeOrders = 4;

            // Act
            var result = await controller.BrewCoffee() as StatusCodeResult;

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status503ServiceUnavailable, result.StatusCode);
        }

        [Fact]
        public async Task BrewCoffee_ReturnsServiceUnavailableOnFifthCall()
        {
            // Arrange
            // still sub for DateTimeProviderService incase today is actually april 1st
             var coffeeMachineService = NSubstitute.Substitute.For<ICoffeeMachineService>(); 
            var brewCoffeeResult = new BrewCoffeeResult { 
                IsSuccess = true, 
                ResultType = ResultType.Success,
                CoffeeType = CoffeeType.PipingHot, 
                SuccessMessage = "Your PipingHot coffee is ready", 
                PreparedAt = new DateTimeOffset(new DateTime(DateTime.Now.Year, 3, 31)) };

            coffeeMachineService.BrewCoffeeAsync().Returns(brewCoffeeResult);
            var controller = new CoffeeMachineController(coffeeMachineService);
            CoffeeMachineController._coffeeOrders = 0;
            

            // Act
            for (int i = 0; i < 4; i++)
            {
                var result = await controller.BrewCoffee() as ObjectResult;
                Assert.IsType<ObjectResult>(result);
                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            }

            // assert
            var finalResult = await controller.BrewCoffee() as StatusCodeResult;
            Assert.IsType<StatusCodeResult>(finalResult);
            Assert.Equal(StatusCodes.Status503ServiceUnavailable, finalResult.StatusCode);
        }
        
            [Fact]
        public async Task BrewCoffee_AprilFoolsDay_ReturnsTeapot()
        {
            // Arrange
            var coffeeMachineService = Substitute.For<ICoffeeMachineService>();
            var brewCoffeeResult = new BrewCoffeeResult
            {
                IsSuccess = false,
                ResultType = ResultType.AprilFools,
               
            };

            coffeeMachineService.BrewCoffeeAsync().Returns(brewCoffeeResult);
            var controller = new CoffeeMachineController(coffeeMachineService);
            CoffeeMachineController._coffeeOrders = 0;

            // Act
            var result = await controller.BrewCoffee() as StatusCodeResult;

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status418ImATeapot, result.StatusCode);
        }

        [Fact]
        public async Task BrewCoffee_BrewCoffeeAsyncReturnsError_ReturnsInternalServerError()
        {
            // Arrange
            var coffeeMachineService = Substitute.For<ICoffeeMachineService>();
            var brewCoffeeResult = new BrewCoffeeResult
            {
                IsSuccess = false,
                ResultType = ResultType.Error,
                ErrorMessage = "Failed to get geo coordinates for the city."
            };

            coffeeMachineService.BrewCoffeeAsync().Returns(brewCoffeeResult);
            var controller = new CoffeeMachineController(coffeeMachineService);

            // Act
            var result = await controller.BrewCoffee() as ObjectResult;

            // Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

    }
}