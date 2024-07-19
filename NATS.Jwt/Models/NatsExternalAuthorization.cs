using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record NatsExternalAuthorization
{
    [JsonPropertyName("auth_users")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string> AuthUsers { get; set; }

    [JsonPropertyName("allowed_accounts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string> AllowedAccounts { get; set; }

    [JsonPropertyName("xkey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string XKey { get; set; }
}
