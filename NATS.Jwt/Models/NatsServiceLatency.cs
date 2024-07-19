using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record NatsServiceLatency
{
    [JsonPropertyName("sampling")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Sampling { get; set; }

    [JsonPropertyName("results")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Results { get; set; }
}
