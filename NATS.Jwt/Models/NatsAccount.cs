using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record NatsAccount : NatsGenericFields
{
    [JsonPropertyName("imports")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<NatsImport>? Imports { get; set; }

    [JsonPropertyName("exports")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<NatsExport>? Exports { get; set; }

    [JsonPropertyName("limits")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsOperatorLimits Limits { get; set; } = new();

    [JsonPropertyName("signing_keys")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string> SigningKeys { get; set; }

    [JsonPropertyName("revocations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Dictionary<string, long> Revocations { get; set; }

    [JsonPropertyName("default_permissions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsPermissions DefaultPermissions { get; set; } = new();

    [JsonPropertyName("mappings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Dictionary<string, NatsWeightedMapping> Mappings { get; set; }

    [JsonPropertyName("authorization")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsExternalAuthorization Authorization { get; set; } = new();

    [JsonPropertyName("trace")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsMsgTrace Trace { get; set; }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Description { get; set; }

    [JsonPropertyName("info_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string InfoUrl { get; set; }
}
