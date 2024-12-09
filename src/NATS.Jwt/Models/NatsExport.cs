// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using NATS.Jwt.Internal;

namespace NATS.Jwt.Models;

/// <summary>
/// Defines the configuration for exporting data from NATS.
/// </summary>
public record NatsExport
{
    /// <summary>
    /// Gets or sets the name property.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the subject property.
    /// </summary>
    [JsonPropertyName("subject")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Subject { get; set; }

    /// <summary>
    /// Gets or sets the type property.
    /// </summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(NatsJsonStringEnumConverter<NatsExportType>))]
    public NatsExportType Type { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a token is required.
    /// </summary>
    [JsonPropertyName("token_req")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool TokenReq { get; set; }

    /// <summary>
    /// Gets or sets mapping of public keys to unix timestamps.
    /// </summary>
    [JsonPropertyName("revocations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Dictionary<string, long> Revocations { get; set; }

    /// <summary>
    /// Gets or sets the response type property.
    /// </summary>
    [JsonPropertyName("response_type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string ResponseType { get; set; }

    /// <summary>
    /// Gets or sets the response threshold property.
    /// </summary>
    [JsonPropertyName("response_threshold")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TimeSpan ResponseThreshold { get; set; }

    /// <summary>
    /// Gets or sets the Latency property.
    /// </summary>
    [JsonPropertyName("service_latency")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsServiceLatency Latency { get; set; }

    /// <summary>
    /// Gets or sets the position of the account token.
    /// </summary>
    [JsonPropertyName("account_token_position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint AccountTokenPosition { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the export should be advertised.
    /// </summary>
    [JsonPropertyName("advertise")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Advertise { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the export allows tracing.
    /// </summary>
    [JsonPropertyName("allow_trace")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool AllowTrace { get; set; }

    /// <summary>
    /// Gets or sets the description property.
    /// </summary>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the InfoUrl property.
    /// </summary>
    [JsonPropertyName("info_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string InfoUrl { get; set; }
}
