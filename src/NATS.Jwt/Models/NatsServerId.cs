// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the identification information of a NATS server.
/// </summary>
public record NatsServerId
{
    /// <summary>
    /// Gets or sets the name of the server.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the host of the NATS server.
    /// </summary>
    [JsonPropertyName("host")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Host { get; set; }

    /// <summary>
    /// Gets or sets the ID of the server.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the version of the server.
    /// </summary>
    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Version { get; set; }

    /// <summary>
    /// Gets or sets the cluster of the NATS server.
    /// </summary>
    [JsonPropertyName("cluster")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Cluster { get; set; }

    /// <summary>
    /// Gets or sets the tags associated with the server.
    /// </summary>
    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string> Tags { get; set; }

    /// <summary>
    /// Gets or sets the XKey of the server.
    /// </summary>
    [JsonPropertyName("xkey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string XKey { get; set; }
}
