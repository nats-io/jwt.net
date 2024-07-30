// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents a NATS activation record.
/// </summary>
public record NatsActivation : NatsGenericFields
{
    /// <summary>
    /// Gets or sets ImportSubject which represents the subject of the import.
    /// </summary>
    [JsonPropertyName("subject")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string ImportSubject { get; set; }

    /// <summary>
    /// Gets or sets the ImportType which represents the type of the import.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public int ImportType { get; set; }

    /// <summary>
    /// Gets or sets issuerAccount stores the public key for the account the issuer represents.
    /// When set, the claim was issued by a signing key.
    /// </summary>
    [JsonPropertyName("issuer_account")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string IssuerAccount { get; set; }
}
