using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record NatsOperatorClaims : JwtClaimsData
{
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public NatsOperator Operator { get; set; } = new();
}
