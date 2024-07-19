using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record JwtHeader
{
    [JsonPropertyName("typ")]
    public string Type { get; set; }

    [JsonPropertyName("alg")]
    public string Algorithm { get; set; }
}
