using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record NatsUser : NatsGenericFields
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

    [JsonPropertyName("src")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string[] Src { get; set; }

    [JsonPropertyName("times")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TimeRange[] Times { get; set; }

    [JsonPropertyName("times_location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Locale { get; set; }

    [JsonPropertyName("subs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Subs { get; set; } = NatsJwt.NoLimit;

    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Data { get; set; } = NatsJwt.NoLimit;

    [JsonPropertyName("payload")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Payload { get; set; } = NatsJwt.NoLimit;

    [JsonPropertyName("bearer_token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool BearerToken { get; set; }

    [JsonPropertyName("allowed_connection_types")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string[] AllowedConnectionTypes { get; set; }

    [JsonPropertyName("issuer_account")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string IssuerAccount { get; set; }
}
