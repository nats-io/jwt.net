// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents a generic set of fields commonly used in NATS related classes.
/// </summary>
public record NatsGenericFields
{
    /// <summary>
    /// Gets or sets the value representing the tags property.
    /// </summary>
    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(NatsTagsConverter))]
    public NatsTags Tags { get; set; }

    /// <summary>
    /// Gets or sets the value representing a generic field in the NatsGenericFields class.
    /// </summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the value representing the Version property.
    /// </summary>
    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Version { get; set; }
}
