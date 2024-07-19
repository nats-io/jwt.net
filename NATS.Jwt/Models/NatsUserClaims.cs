using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record NatsUserClaims : JwtClaimsData
{
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public NatsUser User { get; set; } = new();
}
