// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents a subject permissions.
/// </summary>
public record NatsPermission
{
    /// <summary>
    /// Gets or sets values representing allowed subjects.
    /// </summary>
    [JsonPropertyName("allow")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string> Allow { get; set; }

    /// <summary>
    /// Gets or sets values representing denied subjects.
    /// </summary>
    [JsonPropertyName("deny")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string> Deny { get; set; }
}
