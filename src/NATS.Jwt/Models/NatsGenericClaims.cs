// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the generic claims for NATS.
/// </summary>
public record NatsGenericClaims : JwtClaimsData
{
    /// <summary>
    /// Gets or sets a value representing the generic claims data for Nats messages.
    /// </summary>
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public Dictionary<string, JsonNode> Data { get; set; } = new();
}
