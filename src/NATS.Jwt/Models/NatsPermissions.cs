// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the permissions for publishing, subscribing, and responding to NATS messages.
/// </summary>
public record NatsPermissions
{
    /// <summary>
    /// Gets or sets the publishing permission in NatsPermissions.
    /// </summary>
    [JsonPropertyName("pub")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsPermission Pub { get; set; } = new();

    /// <summary>
    /// Gets or sets the subscription permission in NatsPermissions.
    /// </summary>
    [JsonPropertyName("sub")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsPermission Sub { get; set; } = new();

    /// <summary>
    /// Gets or sets the response permission in NatsPermissions.
    /// </summary>
    [JsonPropertyName("resp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsResponsePermission Resp { get; set; }
}
