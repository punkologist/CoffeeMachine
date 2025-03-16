using CoffeeMachine.Services.Interfaces;
using CoffeeMachine.Services;
using Swashbuckle.AspNetCore.Annotations;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Coffee Machine - V1",
            Version = "v1"
        }
     );

    
});

builder.Services.AddLogging(l=> l.AddConsole());

builder.Services.AddScoped<IDateTimeProviderService, DateTimeProviderService>();
builder.Services.AddScoped<IOpenWeatherService, OpenWeatherService>();
builder.Services.AddScoped<ICoffeeMachineService, CoffeeMachineService>();

var configuration = builder.Configuration;

var baseUrl = configuration["OpenWeatherApi:BaseUrl"] ?? throw new ArgumentNullException("OpenWeatherApi:BaseUrl");
var apiKey = configuration["OpenWeatherApi:ApiKey"] ?? throw new ArgumentNullException("OpenWeatherApi:ApiKey");

builder.Services.AddScoped(
    p => {
        var client = new RestClient(new Uri(baseUrl));
        client.AddDefaultQueryParameter("appid", apiKey);
        return client;
    }
   );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
