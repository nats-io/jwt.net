// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the claims associated with a NATS account.
/// </summary>
public record NatsAccountClaims : JwtClaimsData
{
    /// <summary>
    /// Gets or sets represents an account.
    /// </summary>
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public NatsAccount Account { get; set; } = new();
}
