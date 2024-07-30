// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the activation claims for NATS authentication.
/// </summary>
public record NatsActivationClaims : JwtClaimsData
{
    /// <summary>
    /// Gets or sets represents the activation information for NATS.
    /// </summary>
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public NatsActivation Activation { get; set; } = new();
}
