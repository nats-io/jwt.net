// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the response received after authorizing with NATS.
/// </summary>
public record NatsAuthorizationResponse : NatsGenericFields
{
    /// <summary>
    /// Gets or sets the value representing a JSON Web Token (JWT) used for authorization.
    /// </summary>
    [JsonPropertyName("jwt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string Jwt { get; set; }

    /// <summary>
    /// Gets or sets the value representing an error message in a NatsAuthorizationResponse object.
    /// </summary>
    [JsonPropertyName("error")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string Error { get; set; }

    /// <summary>
    /// Gets or sets the value for issuerAccount which stores the public key for the account the issuer represents.
    /// When set, the claim was issued by a signing key.
    /// </summary>
    [JsonPropertyName("issuer_account")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string IssuerAccount { get; set; }
}
