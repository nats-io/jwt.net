// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the claims data for a NATS authorization response.
/// </summary>
public record NatsAuthorizationResponseClaims : JwtClaimsData
{
    /// <summary>
    /// Gets or sets the value representing an authorization response for NATS.
    /// </summary>
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public NatsAuthorizationResponse AuthorizationResponse { get; set; } = new();
}
