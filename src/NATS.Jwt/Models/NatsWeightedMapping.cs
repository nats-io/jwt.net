// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents a weighted mapping for NATS subjects.
/// </summary>
public record NatsWeightedMapping
{
    /// <summary>
    /// Gets or sets the subject of the weighted mapping.
    /// </summary>
    [JsonPropertyName("subject")]
    public string Subject { get; set; }

    /// <summary>
    /// Gets or sets the weight of the weighted mapping.
    /// </summary>
    /// <value>The weight value.</value>
    [JsonPropertyName("weight")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public byte Weight { get; set; }

    /// <summary>
    /// Gets or sets the cluster property of the NatsWeightedMapping class.
    /// </summary>
    [JsonPropertyName("cluster")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Cluster { get; set; }
}
