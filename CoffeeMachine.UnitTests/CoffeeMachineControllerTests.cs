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
        public void BrewCoffee_RequestIsNotFifth_ReturnsOk()
        {
            // Arrange
            // still sub for DateTimeProviderService incase today is actually april 1st
            var dateTimeProviderService = NSubstitute.Substitute.For<IDateTimeProviderService>();
            dateTimeProviderService.Now.Returns(new DateTimeOffset(new DateTime(DateTime.Now.Year, 3, 31)));
            var controller = new CoffeeMachineController(new DateTimeProviderService());
            //CoffeeMachineController._brewCoffeeRequestCount = 0;
            RequestTracker.SetBrewCoffeeRequestCount(0);
            // Act
            var result = controller.BrewCoffee() as ObjectResult;

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void BrewCoffee_RequestIsFifth_ReturnsServiceUnavailable()
        {
            // Arrange
            // date is irrelevant for this test
            var controller = new CoffeeMachineController(new DateTimeProviderService());
            //CoffeeMachineController._brewCoffeeRequestCount = 4;
            RequestTracker.SetBrewCoffeeRequestCount(4);
            // Act
            var result = controller.BrewCoffee() as StatusCodeResult;

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status503ServiceUnavailable, result.StatusCode);
        }

        [Fact]
        public void BrewCoffee_ReturnsServiceUnavailableOnFifthCall()
        {
            // Arrange
            // still sub for DateTimeProviderService incase today is actually april 1st
            var dateTimeProviderService = NSubstitute.Substitute.For<IDateTimeProviderService>();
            dateTimeProviderService.Now.Returns(new DateTimeOffset(new DateTime(DateTime.Now.Year, 3, 31)));
            var controller = new CoffeeMachineController(dateTimeProviderService);
            RequestTracker.SetBrewCoffeeRequestCount(0);
            // Act
            for (int i = 0; i < 4; i++)
            {
                var result = controller.BrewCoffee() as ObjectResult;
                Assert.IsType<ObjectResult>(result);
                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            }

            // assert
            var finalResult = controller.BrewCoffee() as StatusCodeResult;
            Assert.IsType<StatusCodeResult>(finalResult);
            Assert.Equal(StatusCodes.Status503ServiceUnavailable, finalResult.StatusCode);
        }
        
        [Fact]
        public void BrewCoffee_ReturnsServiceUnavailableOnSecondFifthCall()
        {
            // Arrange
            // still sub for DateTimeProviderService incase today is actually april 1st
            var dateTimeProviderService = NSubstitute.Substitute.For<IDateTimeProviderService>();
            dateTimeProviderService.Now.Returns(new DateTimeOffset(new DateTime(DateTime.Now.Year, 3, 31)));
            var controller = new CoffeeMachineController(dateTimeProviderService);
            // start at 5 so we can get to the second 5th call
            //CoffeeMachineController._brewCoffeeRequestCount = 5;
            RequestTracker.SetBrewCoffeeRequestCount(5);
            // Act
            for (int i = 0; i < 4; i++)
            {
                var result = controller.BrewCoffee() as ObjectResult;
                Assert.IsType<ObjectResult>(result);
                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            }

            // assert
            var finalResult = controller.BrewCoffee() as StatusCodeResult;
            Assert.IsType<StatusCodeResult>(finalResult);
            Assert.Equal(StatusCodes.Status503ServiceUnavailable, finalResult.StatusCode);
        }

        [Fact]
        public void BrewCoffee_AprilFoolsDay_RequestIsNotFifth_ReturnsTeapot()
        {
            // Arrange
            var dateTimeProviderService = NSubstitute.Substitute.For<IDateTimeProviderService>();
            dateTimeProviderService.Now.Returns(new DateTimeOffset(new DateTime(DateTime.Now.Year, 4, 1)));
            var controller = new CoffeeMachineController(dateTimeProviderService);
            //CoffeeMachineController._brewCoffeeRequestCount = 0;
            RequestTracker.SetBrewCoffeeRequestCount(0);
            // Act
            var result = controller.BrewCoffee() as StatusCodeResult;

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status418ImATeapot, result.StatusCode);
        }

         [Fact]
        public void BrewCoffee_NonAprilFoolsDay_RequestIsNotFifth_Returns200()
        {
            // Arrange
            var dateTimeProviderService = NSubstitute.Substitute.For<IDateTimeProviderService>();
            dateTimeProviderService.Now.Returns(new DateTimeOffset(new DateTime(DateTime.Now.Year, 3, 31)));
            var controller = new CoffeeMachineController(dateTimeProviderService);
            //CoffeeMachineController._brewCoffeeRequestCount = 0;
            RequestTracker.SetBrewCoffeeRequestCount(0);
            // Act
            var result = controller.BrewCoffee() as ObjectResult;

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.NotEqual(StatusCodes.Status418ImATeapot, result.StatusCode);
        }

                 [Fact]
        public void BrewCoffee_AprilFoolsDay_RequestIsFifth_ReturnsServiceUnavailable()
        {
            // Arrange
            var dateTimeProviderService = NSubstitute.Substitute.For<IDateTimeProviderService>();
            dateTimeProviderService.Now.Returns(new DateTimeOffset(new DateTime(DateTime.Now.Year, 4, 1)));
            var controller = new CoffeeMachineController(dateTimeProviderService);
            //CoffeeMachineController._brewCoffeeRequestCount = 9;
            RequestTracker.SetBrewCoffeeRequestCount(9);
            // Act
            var result = controller.BrewCoffee() as StatusCodeResult;

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status503ServiceUnavailable, result.StatusCode);
        }
    }
}