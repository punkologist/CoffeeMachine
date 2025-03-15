using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoffeeMachine.Services;
using CoffeeMachine.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace CoffeeMachine.Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class CoffeeMachineController : ControllerBase
    {
        public static int _brewCoffeeRequestCount = 0;
        private readonly IDateTimeProviderService _dateTimeProviderService;

        public CoffeeMachineController(IDateTimeProviderService dateTimeProviderService)
        {
           _dateTimeProviderService = dateTimeProviderService;
        }

        /// <summary>
        /// Brews a cup of coffee.
        /// </summary>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        /// <response code="200">Ok</response>
        /// <response code="503">Service Unavailable</response>
        /// <response code="418">I’m a Teapot</response>
        [HttpGet, Route("brew-coffee")]
        [SwaggerOperation(Summary = "Brews a cup of coffee", Description ="Brews a cup of coffee. On every 5th call the coffee runs out and needs to be refilled. Will return 503 when coffee has run out. If the date is April 1st will return 418") ]
        [SwaggerResponse(StatusCodes.Status200OK, "200 OK")]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "503 Service Unavailable")]
        [SwaggerResponse(StatusCodes.Status418ImATeapot, "418 I'm a Teapot")]
        public IActionResult BrewCoffee(){

            _brewCoffeeRequestCount++;
            var now = _dateTimeProviderService.Now;
            // if the request count is the fith call, return 503
            if(_brewCoffeeRequestCount > 0 && _brewCoffeeRequestCount % 5 == 0){
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
            // it's april fools day, return 418
            if(now.Date.Month == 4 && now.Date.Day == 1){
                return StatusCode(StatusCodes.Status418ImATeapot);
            }
            
            return StatusCode(StatusCodes.Status200OK, new 
            {
                message = "Your piping hot coffee is ready", 
                prepared = now
            });
        }

    }

}