using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record NatsWeightedMapping
{
    [JsonPropertyName("subject")]
    public string Subject { get; set; }

    [JsonPropertyName("weight")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public byte Weight { get; set; }

    [JsonPropertyName("cluster")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Cluster { get; set; }
}
