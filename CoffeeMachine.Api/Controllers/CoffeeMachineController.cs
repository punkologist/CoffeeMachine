using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoffeeMachine.Services;
using CoffeeMachine.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using CoffeeMachine.Services.Models;

namespace CoffeeMachine.Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class CoffeeMachineController(ICoffeeMachineService coffeeMachineService) : ControllerBase
    {
        private readonly ICoffeeMachineService coffeeMachineService = coffeeMachineService;
        public static int _coffeeOrders = 0;

        /// <summary>
        /// Brews a cup of coffee.
        /// </summary>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        /// <response code="200">Ok</response>
        /// <response code="503">Service Unavailable</response>
        /// <response code="418">I’m a Teapot</response>
        [HttpGet, Route("brew-coffee")]
        [SwaggerOperation(Summary = "Brews a cup of coffee", Description = "Brews a cup of coffee. If the weather is greater than 30°C the coffee will be iced coffe. On every 5th call the coffee runs out and needs to be refilled. Will return 503 when coffee has run out. If the date is April 1st will return 418")]
        [SwaggerResponse(StatusCodes.Status200OK, "200 OK")]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "503 Service Unavailable")]
        [SwaggerResponse(StatusCodes.Status418ImATeapot, "418 I'm a Teapot")]
        public async Task<IActionResult> BrewCoffee()
        {


            _coffeeOrders++;

            // check for stock level (every 5th order there is no coffee)
            if (_coffeeOrders % 5 == 0)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            var result = await coffeeMachineService.BrewCoffeeAsync();
            if (result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = result.SuccessMessage,
                    prepared = result.PreparedAt
                });
            }
            if (result.ResultType == ResultType.Error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = result.ErrorMessage
                });
            }
            if (result.ResultType == ResultType.AprilFools)
            {
                return StatusCode(StatusCodes.Status418ImATeapot);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);


        }

    }

}