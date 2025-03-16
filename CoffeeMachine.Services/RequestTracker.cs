using System;

namespace CoffeeMachine.Services;

public static class RequestTracker
{
    public static int BrewCoffeeRequestCount { get; private set; }

    public static void IncrementBrewCoffeeRequestCount()
    {
        BrewCoffeeRequestCount++;
    }

    public static void SetBrewCoffeeRequestCount(int count)
    {
        BrewCoffeeRequestCount = count;
    }
    
}
