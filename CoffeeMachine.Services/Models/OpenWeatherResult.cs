using System;
using System.Text.Json.Serialization;

namespace CoffeeMachine.Services.Models;

public class OpenWeatherResult
{

        [JsonPropertyName("coord")]
        public required Coord Coord { get; set; }

        [JsonPropertyName("main")]
        public required Main Main { get; set; }


        [JsonPropertyName("timezone")]
        public required int Timezone { get; set; }

        [JsonPropertyName("id")]
        public required int Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

}


    public class Coord
    {
        [JsonPropertyName("lon")]
        public required double Lon { get; set; }

        [JsonPropertyName("lat")]
        public required double Lat { get; set; }
    }

    public class Main
    {
        [JsonPropertyName("temp")]
        public double? Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public double? FeelsLike { get; set; }

        [JsonPropertyName("temp_min")]
        public double? TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        public double? TempMax { get; set; }

        [JsonPropertyName("pressure")]
        public int? Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public int? Humidity { get; set; }

        [JsonPropertyName("sea_level")]
        public int? SeaLevel { get; set; }

        [JsonPropertyName("grnd_level")]
        public int? GrndLevel { get; set; }
    }



