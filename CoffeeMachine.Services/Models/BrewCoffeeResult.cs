using System;

namespace CoffeeMachine.Services.Models;

public class BrewCoffeeResult
{
    public bool IsSuccess { get; set; }
    public DateTimeOffset? PreparedAt { get; set; }
    public required ResultType ResultType { get; set; }
    public CoffeeType? CoffeeType { get; set; }

    public string? ErrorMessage { get; set; }
    public Exception? Exception { get; set; }

    public string? SuccessMessage { get; set; }

}


public class CoffeeType
{
    private CoffeeType(string value) { Value = value; }

    public string Value { get; private set; }

    public static CoffeeType PipingHot   { get { return new CoffeeType("piping hot"); } }
    public static CoffeeType Iced   { get { return new CoffeeType("refreshing iced"); } }

    public override string ToString()
    {
        return Value;
    }
}

public enum ResultType
{
    Success,
    AprilFools,
    Error
}