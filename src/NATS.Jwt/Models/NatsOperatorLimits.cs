// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the limits for a NATS operator.
/// </summary>
public record NatsOperatorLimits
{
    /// <summary>
    /// Gets or sets max number of subscriptions.
    /// </summary>
    [JsonPropertyName("subs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Subs { get; set; } = NatsJwt.NoLimit;

    /// <summary>
    /// Gets or sets max number of bytes.
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
    /// Gets or sets the number of imports.
    /// </summary>
    [JsonPropertyName("imports")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Imports { get; set; } = NatsJwt.NoLimit;

    /// <summary>
    /// Gets or sets max number of exports.
    /// </summary>
    [JsonPropertyName("exports")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Exports { get; set; } = NatsJwt.NoLimit;

    /// <summary>
    /// Gets or sets a value indicating whether wildcards are allowed in exports.
    /// </summary>
    [JsonPropertyName("wildcards")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool WildcardExports { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether user JWT can't be bearer token.
    /// </summary>
    [JsonPropertyName("disallow_bearer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool DisallowBearer { get; set; }

    /// <summary>
    /// Gets or sets max number of active connections.
    /// </summary>
    [JsonPropertyName("conn")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Conn { get; set; } = NatsJwt.NoLimit;

    /// <summary>
    /// Gets or sets max number of active leaf node connections.
    /// </summary>
    [JsonPropertyName("leaf")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long LeafNodeConn { get; set; } = NatsJwt.NoLimit;

    /// <summary>
    /// Gets or sets max number of bytes stored in memory across all streams. (0 means disabled).
    /// </summary>
    [JsonPropertyName("mem_storage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long MemoryStorage { get; set; }

    /// <summary>
    /// Gets or sets max number of bytes stored on disk across all streams. (0 means disabled).
    /// </summary>
    [JsonPropertyName("disk_storage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long DiskStorage { get; set; }

    /// <summary>
    /// Gets or sets max number of streams.
    /// </summary>
    [JsonPropertyName("streams")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Streams { get; set; }

    /// <summary>
    /// Gets or sets max number of consumers.
    /// </summary>
    [JsonPropertyName("consumer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Consumer { get; set; }

    /// <summary>
    /// Gets or sets max ack pending of a Stream.
    /// </summary>
    [JsonPropertyName("max_ack_pending")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long MaxAckPending { get; set; }

    /// <summary>
    /// Gets or sets max bytes a memory backed stream can have. (0 means disabled/unlimited).
    /// </summary>
    [JsonPropertyName("mem_max_stream_bytes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long MemoryMaxStreamBytes { get; set; }

    /// <summary>
    /// Gets or sets max bytes a disk backed stream can have. (0 means disabled/unlimited).
    /// </summary>
    [JsonPropertyName("disk_max_stream_bytes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long DiskMaxStreamBytes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether max bytes required by all Streams.
    /// </summary>
    [JsonPropertyName("max_bytes_required")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool MaxBytesRequired { get; set; }

    /// <summary>
    /// Gets or sets tiered JetStream limits.
    /// </summary>
    [JsonPropertyName("tiered_limits")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Dictionary<string, JetStreamLimits> JetStreamTieredLimits { get; set; }
}
