using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record NatsGenericFieldsClaims : JwtClaimsData
{
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public NatsGenericFields GenericFields { get; set; } = new();
}
