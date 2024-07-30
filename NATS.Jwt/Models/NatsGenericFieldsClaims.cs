// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the claims data for the NatsGenericFieldsClaims.
/// </summary>
public record NatsGenericFieldsClaims : JwtClaimsData
{
    /// <summary>
    /// Gets or sets a value representing the generic fields property for NatsGenericFieldsClaims.
    /// </summary>
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public NatsGenericFields GenericFields { get; set; } = new();
}
