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
        private readonly ICoffeeMachineService _coffeeMachineService;
        private readonly CoffeeMachineController _controller;

        public CoffeeMachineControllerTests()
        {
            _coffeeMachineService = Substitute.For<ICoffeeMachineService>();
            _controller = new CoffeeMachineController(_coffeeMachineService);
        }

        [Fact]
        public async Task BrewCoffee_RequestIsNotFifth_ReturnsOk()
        {
            // Arrange
            var brewCoffeeResult = new BrewCoffeeResult { 
                IsSuccess = true, 
                ResultType = ResultType.Success,
                CoffeeType = CoffeeType.PipingHot, 
                SuccessMessage = "Your PipingHot coffee is ready", 
                PreparedAt = new DateTimeOffset(new DateTime(DateTime.Now.Year, 3, 31)) };

            _coffeeMachineService.BrewCoffeeAsync().Returns(brewCoffeeResult);
            RequestTracker.SetBrewCoffeeRequestCount(0);

            // Act
            var result = await _controller.BrewCoffee() as ObjectResult;

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task BrewCoffee_RequestIsFifth_ReturnsServiceUnavailable()
        {
            // Arrange
            
            var brewCoffeeResult = new BrewCoffeeResult { 
                IsSuccess = true, 
                ResultType = ResultType.Success,
                CoffeeType = CoffeeType.PipingHot, 
                SuccessMessage = "Your PipingHot coffee is ready", 
                PreparedAt = new DateTimeOffset(new DateTime(DateTime.Now.Year, 3, 31)) };

            _coffeeMachineService.BrewCoffeeAsync().Returns(brewCoffeeResult);
           
             RequestTracker.SetBrewCoffeeRequestCount(4);

            // Act
            var result = await _controller.BrewCoffee() as StatusCodeResult;

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status503ServiceUnavailable, result.StatusCode);
        }

        [Fact]
        public async Task BrewCoffee_ReturnsServiceUnavailableOnFifthCall()
        {
            // Arrange
            var brewCoffeeResult = new BrewCoffeeResult { 
                IsSuccess = true, 
                ResultType = ResultType.Success,
                CoffeeType = CoffeeType.PipingHot, 
                SuccessMessage = "Your PipingHot coffee is ready", 
                PreparedAt = new DateTimeOffset(new DateTime(DateTime.Now.Year, 3, 31)) };

            _coffeeMachineService.BrewCoffeeAsync().Returns(brewCoffeeResult);

            RequestTracker.SetBrewCoffeeRequestCount(0);
            

            // Act
            for (int i = 0; i < 4; i++)
            {
                var result = await _controller.BrewCoffee() as ObjectResult;
                Assert.IsType<ObjectResult>(result);
                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            }

            // assert
            var finalResult = await _controller.BrewCoffee() as StatusCodeResult;
            Assert.IsType<StatusCodeResult>(finalResult);
            Assert.Equal(StatusCodes.Status503ServiceUnavailable, finalResult.StatusCode);
        }
        
            [Fact]
        public async Task BrewCoffee_AprilFoolsDay_ReturnsTeapot()
        {
            // Arrange
            var brewCoffeeResult = new BrewCoffeeResult
            {
                IsSuccess = false,
                ResultType = ResultType.AprilFools,
               
            };

            _coffeeMachineService.BrewCoffeeAsync().Returns(brewCoffeeResult);
            RequestTracker.SetBrewCoffeeRequestCount(0);
            // Act
            var result = await _controller.BrewCoffee() as StatusCodeResult;

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status418ImATeapot, result.StatusCode);
        }

        [Fact]
        public async Task BrewCoffee_BrewCoffeeAsyncReturnsError_ReturnsInternalServerError()
        {
            // Arrange
            var brewCoffeeResult = new BrewCoffeeResult
            {
                IsSuccess = false,
                ResultType = ResultType.Error,
                ErrorMessage = "Failed to get geo coordinates for the city."
            };

            _coffeeMachineService.BrewCoffeeAsync().Returns(brewCoffeeResult);
            RequestTracker.SetBrewCoffeeRequestCount(0);
            // Act
            var result = await _controller.BrewCoffee() as ObjectResult;

            // Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

    }
}