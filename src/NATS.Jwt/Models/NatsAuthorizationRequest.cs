// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents a NATS authorization request.
/// </summary>
public record NatsAuthorizationRequest : NatsGenericFields
{
    /// <summary>
    /// Gets or sets the value representing a NATS server.
    /// </summary>
    [JsonPropertyName("server_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public NatsServerId NatsServer { get; set; } = new();

    /// <summary>
    /// Gets or sets the value representing the user NKey in an NatsAuthorizationRequest object.
    /// </summary>
    [JsonPropertyName("user_nkey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string UserNKey { get; set; }

    /// <summary>
    /// Gets or sets the value representing the client information used in the NATS authorization request.
    /// </summary>
    [JsonPropertyName("client_info")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public NatsClientInformation NatsClientInformation { get; set; } = new();

    /// <summary>
    /// Gets or sets the value representing the options for connecting to NATS server.
    /// </summary>
    [JsonPropertyName("connect_opts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public NatsConnectOptions NatsConnectOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the value representing the TLS configuration for the NATS client.
    /// </summary>
    [JsonPropertyName("client_tls")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsClientTls Tls { get; set; }

    /// <summary>
    /// Gets or sets the nonce value used in the NATS authorization request.
    /// </summary>
    [JsonPropertyName("request_nonce")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string RequestNonce { get; set; }
}
