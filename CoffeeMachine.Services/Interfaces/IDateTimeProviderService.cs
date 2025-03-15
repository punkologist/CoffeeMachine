using System;

namespace CoffeeMachine.Services.Interfaces;

public interface IDateTimeProviderService
{
    public DateTimeOffset Now { get; }
}
