// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the information about a NATS client.
/// </summary>
public record NatsClientInformation
{
    /// <summary>
    /// Gets or sets the value representing the host property of the <see cref="NatsClientInformation"/> class.
    /// </summary>
    [JsonPropertyName("host")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Host { get; set; }

    /// <summary>
    /// Gets or sets the value representing the ID property of the <see cref="NatsClientInformation"/> class.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ulong Id { get; set; }

    /// <summary>
    /// Gets or sets the value representing the user property of the <see cref="NatsClientInformation"/> class.
    /// </summary>
    [JsonPropertyName("user")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string User { get; set; }

    /// <summary>
    /// Gets or sets the value representing the name property of the <see cref="NatsClientInformation"/> class.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the value representing the tags property of the <see cref="NatsClientInformation"/> class.
    /// This property represents a list of string tags associated with the client information.
    /// </summary>
    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string> Tags { get; set; }

    /// <summary>
    /// Gets or sets the value representing the name tag property of the <see cref="NatsClientInformation"/> class.
    /// </summary>
    [JsonPropertyName("name_tag")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string NameTag { get; set; }

    /// <summary>
    /// Gets or sets the value representing the kind property of the <see cref="NatsClientInformation"/> class.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Kind { get; set; }

    /// <summary>
    /// Gets or sets the value representing the type property of the <see cref="NatsClientInformation"/> class.
    /// </summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the value representing the MQTT property of the <see cref="NatsClientInformation"/> class.
    /// </summary>
    [JsonPropertyName("mqtt_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Mqtt { get; set; }

    /// <summary>
    /// Gets or sets the value representing the Nonce property of the <see cref="NatsClientInformation"/> class.
    /// </summary>
    [JsonPropertyName("nonce")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Nonce { get; set; }
}
