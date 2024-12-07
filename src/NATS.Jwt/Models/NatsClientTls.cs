// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the TLS configuration of the NATS client.
/// </summary>
public record NatsClientTls
{
    /// <summary>
    /// Gets or sets hte value representing the version of the NatsClientTls.
    /// </summary>
    /// <value>
    /// The version string of the NatsClientTls.
    /// </value>
    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Version { get; set; }

    /// <summary>
    /// Gets or sets the value representing the cipher used by the NatsClientTls.
    /// </summary>
    /// <value>
    /// The cipher used by the NatsClientTls.
    /// </value>
    [JsonPropertyName("cipher")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Cipher { get; set; }

    /// <summary>
    /// Gets or sets the value representing the certificates used for TLS communication in a NATS client.
    /// </summary>
    /// <remarks>
    /// The Certs property holds a list of strings representing the certificates.
    /// </remarks>
    [JsonPropertyName("certs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string> Certs { get; set; }

    /// <summary>
    /// Gets or sets the value representing the verified chains of the NatsClientTls.
    /// </summary>
    /// <value>
    /// A list of lists of strings representing the verified chains of the NatsClientTls.
    /// Each inner list contains strings representing the elements of the verified chain.
    /// </value>
    [JsonPropertyName("verified_chains")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<List<string>> VerifiedChains { get; set; }
}
