// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the claims of a NATS user in a JSON Web Token (JWT).
/// </summary>
public record NatsUserClaims : JwtClaimsData
{
    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public NatsUser User { get; set; } = new();
}
