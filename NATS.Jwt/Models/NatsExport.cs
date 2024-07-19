using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record NatsExport
{
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Name { get; set; }

    [JsonPropertyName("subject")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Subject { get; set; }

    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Type { get; set; }

    [JsonPropertyName("token_req")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool TokenReq { get; set; }

    /// <summary>
    /// Mapping of public keys to unix timestamps
    /// </summary>
    [JsonPropertyName("revocations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Dictionary<string, long> Revocations { get; set; }

    [JsonPropertyName("response_type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string ResponseType { get; set; }

    [JsonPropertyName("response_threshold")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TimeSpan ResponseThreshold { get; set; }

    [JsonPropertyName("service_latency")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsServiceLatency Latency { get; set; }

    [JsonPropertyName("account_token_position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint AccountTokenPosition { get; set; }

    [JsonPropertyName("advertise")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Advertise { get; set; }

    [JsonPropertyName("allow_trace")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool AllowTrace { get; set; }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Description { get; set; }

    [JsonPropertyName("info_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string InfoUrl { get; set; }
}
