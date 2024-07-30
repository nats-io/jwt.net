// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Text.Json.Serialization;
using NATS.Jwt.Internal;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the response permission for NATS messages.
/// </summary>
public record NatsResponsePermission
{
    /// <summary>
    /// Gets or sets a value representing the maximum number of messages allowed for NATS response permissions.
    /// </summary>
    [JsonPropertyName("max")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int MaxMsgs { get; set; }

    /// <summary>
    /// Gets or sets a value representing the expiration time for the NATS response permission.
    /// </summary>
    [JsonPropertyName("ttl")]
    [JsonConverter(typeof(NatsJsJsonNanosecondsConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TimeSpan Expires { get; set; }
}
