using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record NatsAccountClaims : JwtClaimsData
{
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public NatsAccount Account { get; set; } = new();
}
