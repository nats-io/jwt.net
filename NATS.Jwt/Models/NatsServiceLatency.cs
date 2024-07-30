// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the latency settings for a NATS service.
/// </summary>
public record NatsServiceLatency
{
    /// <summary>
    /// Gets or sets the sampling property for a NATS service latency.
    /// </summary>
    [JsonPropertyName("sampling")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Sampling { get; set; }

    /// <summary>
    /// Gets or sets the results property for a NATS service latency.
    /// </summary>
    [JsonPropertyName("results")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Results { get; set; }
}
