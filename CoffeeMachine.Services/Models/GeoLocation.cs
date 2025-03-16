using System.Text.Json.Serialization;

namespace CoffeeMachine.Services.Models;

public class GeoLocation
{
    [JsonPropertyName("name")]
    public required string Name {get; set;}
    [JsonPropertyName("state")]
    public string? State { get; set; }
    [JsonPropertyName("lat")]
    public required double Lat { get; set; }
    [JsonPropertyName("lon")]
    public required double Lon { get; set; }

    [JsonPropertyName("local_names")]
    public object? LocalNames { get; set; }
    [JsonPropertyName("country")]
    public required string Country { get; set; }
    
}
