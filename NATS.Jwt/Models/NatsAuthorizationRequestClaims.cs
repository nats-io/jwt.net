// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the claims for a NATS authorization request.
/// </summary>
public record NatsAuthorizationRequestClaims : JwtClaimsData
{
    /// <summary>
    /// Gets or sets the value representing an authorization request for NATS.
    /// </summary>
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public NatsAuthorizationRequest AuthorizationRequest { get; set; } = new();
}
