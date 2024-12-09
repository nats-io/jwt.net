// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents a user in the NATS system.
/// </summary>
/// <remarks>
/// The NatsUser class is used to store user information in the NATS system.
/// It contains various properties that define the permissions, limitations, and other details of a user.
/// </remarks>
public record NatsUser : NatsGenericFields
{
    /// <summary>
    /// Gets or sets the publishing permissions of the NATS user.
    /// </summary>
    [JsonPropertyName("pub")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsPermission Pub { get; set; } = new();

    /// <summary>
    /// Gets or sets the subscription permissions of the NATS user.
    /// </summary>
    [JsonPropertyName("sub")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsPermission Sub { get; set; } = new();

    /// <summary>
    /// Gets or sets the response permission of the NATS user.
    /// </summary>
    [JsonPropertyName("resp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsResponsePermission Resp { get; set; }

    /// <summary>
    /// Gets or sets the sources for the NATS user.
    /// </summary>
    [JsonPropertyName("src")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string> Src { get; set; }

    /// <summary>
    /// Gets or sets the list of time ranges when the NATS user is allowed to connect.
    /// </summary>
    [JsonPropertyName("times")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<TimeRange> Times { get; set; }

    /// <summary>
    /// Gets or sets the locale of the NATS user.
    /// </summary>
    [JsonPropertyName("times_location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Locale { get; set; }

    /// <summary>
    /// Gets or sets the number of subscriptions allowed for the NATS user.
    /// </summary>
    [JsonPropertyName("subs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Subs { get; set; } = NatsJwt.NoLimit;

    /// <summary>
    /// Gets or sets the max number of bytes.
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Data { get; set; } = NatsJwt.NoLimit;

    /// <summary>
    /// Gets or sets max message payload.
    /// </summary>
    [JsonPropertyName("payload")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Payload { get; set; } = NatsJwt.NoLimit;

    /// <summary>
    /// Gets or sets a value indicating whether a bearer token is used for the NATS user.
    /// </summary>
    [JsonPropertyName("bearer_token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool BearerToken { get; set; }

    /// <summary>
    /// Gets or sets the allowed connection types for the NATS user.
    /// </summary>
    [JsonPropertyName("allowed_connection_types")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string> AllowedConnectionTypes { get; set; }

    /// <summary>
    /// Gets or sets the issuer account of the NATS user.
    /// </summary>
    [JsonPropertyName("issuer_account")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string IssuerAccount { get; set; }
}
