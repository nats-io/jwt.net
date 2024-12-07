// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the header of a JSON Web Token (JWT).
/// </summary>
public record JwtHeader
{
    /// <summary>
    /// Gets or sets the value representing the header of a JSON Web Token (JWT).
    /// </summary>
    [JsonPropertyName("typ")]
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the value representing the algorithm used for signing or encrypting a JSON Web Token (JWT).
    /// </summary>
    [JsonPropertyName("alg")]
    public string Algorithm { get; set; }
}
