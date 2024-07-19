using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record NatsPermissions
{
    [JsonPropertyName("pub")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsPermission Pub { get; set; } = new();

    [JsonPropertyName("sub")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsPermission Sub { get; set; } = new();

    [JsonPropertyName("resp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsResponsePermission Resp { get; set; }
}
