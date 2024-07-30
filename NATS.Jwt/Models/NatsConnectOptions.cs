// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the options for connecting to the NATS server.
/// </summary>
public record NatsConnectOptions
{
    /// <summary>
    /// Gets or sets the value representing the JSON Web Token (JWT) property in the NatsConnectOptions class.
    /// </summary>
    [JsonPropertyName("jwt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Jwt { get; set; }

    /// <summary>
    /// Gets or sets the value representing the NKey property in the NatsConnectOptions class.
    /// </summary>
    [JsonPropertyName("nkey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string NKey { get; set; }

    /// <summary>
    /// Gets or sets the value representing the Signed Nonce property in the NatsConnectOptions class.
    /// </summary>
    [JsonPropertyName("sig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string SignedNonce { get; set; }

    /// <summary>
    /// Gets or sets the value representing the Token property in the NatsConnectOptions class.
    /// </summary>
    /// <value>
    /// The Token property is used to store the authentication token to be used when connecting to NATS.
    /// </value>
    [JsonPropertyName("auth_token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the value representing the username property in the NatsConnectOptions class.
    /// </summary>
    [JsonPropertyName("user")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the value representing the password property in the NatsConnectOptions class.
    /// </summary>
    [JsonPropertyName("pass")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the value representing the Name property in the NatsConnectOptions class.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the value representing the Lang property in the NatsConnectOptions class.
    /// </summary>
    [JsonPropertyName("lang")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Lang { get; set; }

    /// <summary>
    /// Gets or sets the value representing the Version property in the NatsConnectOptions class.
    /// </summary>
    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Version { get; set; }

    /// <summary>
    /// Gets or sets the value representing the protocol property in the NatsConnectOptions class.
    /// </summary>
    [JsonPropertyName("protocol")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public int Protocol { get; set; }
}
