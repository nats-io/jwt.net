// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the claims for a NATS operator.
/// </summary>
public record NatsOperatorClaims : JwtClaimsData
{
    /// <summary>
    /// Gets or sets the operator information in the NATS operator claims.
    /// </summary>
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public NatsOperator Operator { get; set; } = new();
}
