using System;
using CoffeeMachine.Services.Interfaces;

namespace CoffeeMachine.Services;

public class DateTimeProviderService : IDateTimeProviderService
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}