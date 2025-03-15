using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoffeeMachine.Services;
using CoffeeMachine.Services.Interfaces;

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



        [HttpGet, Route("brew-coffee")]
        public IActionResult BrewCoffee(){

            _brewCoffeeRequestCount++;
            var now = _dateTimeProviderService.Now;
            // if the request count is greater than or equal to 5, return 503
            if(_brewCoffeeRequestCount >= 5){
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
            // it's april fools day, return 418
            if(now.Date.Month == 4 && now.Date.Day == 1){
                return StatusCode(StatusCodes.Status418ImATeapot);
            }

            
            return StatusCode(StatusCodes.Status200OK, new 
            {
                message = "Your piping hot coffee is ready", 
                prepared = now,
                requestCount = _brewCoffeeRequestCount
            });
        }

    }

}