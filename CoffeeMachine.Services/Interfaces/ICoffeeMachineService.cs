using System;
using CoffeeMachine.Services.Models;

namespace CoffeeMachine.Services.Interfaces;

public interface ICoffeeMachineService
{
  public Task<BrewCoffeeResult> BrewCoffeeAsync();
  
}
