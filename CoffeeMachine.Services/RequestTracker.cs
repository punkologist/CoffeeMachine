using System;

namespace CoffeeMachine.Services;

// this could also be done by injecting a singleton, however I learnt reacently that a static class performs better (it's in the nanoseconds)
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
