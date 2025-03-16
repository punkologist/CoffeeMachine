# CoffeeMachine API

## Overview
The CoffeeMachine API is a simple web API that allows you to brew coffee based on the current weather conditions. The API depends on the OpenWeatherMap service to get the current weather data.

## Prerequisites
To run this project, you need to have a local `appsettings.json` file in the root directory of the `CoffeeMachine.Api` project and the `CoffeeMachine.IntegrationTests` project. This file should contain the necessary configuration settings, including the API key for OpenWeatherMap.

## appsettings.json
Here is a sample `appsettings.json` file. Please note that the value of the API key is omitted. You need to replace `"your_api_key_here"` with your actual API key from [OpenWeatherMap](https://openweathermap.org/).

```Json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "OpenWeatherApi": {
    "BaseUrl": "http://api.openweathermap.org",
    "ApiKey":"your_api_key_here",
    "City": "Melbourne",
    "Country": "AU"
  }
}
```

## Dependencies
This project depends on the following external service:
- OpenWeatherMap: Used to get the current weather data.

## Debugging
To debug this project, you need an API key from OpenWeatherMap. You can sign up for a free API key on their website. Once you have the API key, add it to the `appsettings.json` file as shown above.

## Integration Tests
The integration tests for this project also require a local `appsettings.json` file in the root directory of the `CoffeeMachine.IntegrationTests` project. The file should have the same structure as the one shown above.

## Running the Project
To run the project, follow these steps:
1. Ensure you have the .NET SDK installed.
2. Add the `appsettings.json` file to the root directory of the `CoffeeMachine.Api` project and the `CoffeeMachine.IntegrationTests` project.
3. Replace `"your_api_key_here"` with your actual API key from OpenWeatherMap.
4. Build and run the project using your preferred IDE or the command line.

## Building and Running the Project
To build and run the project, use the following commands:

dotnet build
dotnet run --project CoffeeMachine.Api

## Running the Tests
To run the tests, use the following command:

dotnet test