using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record NatsGenericClaims : JwtClaimsData
{
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public Dictionary<string, JsonObject> Data { get; set; } = new();
}
