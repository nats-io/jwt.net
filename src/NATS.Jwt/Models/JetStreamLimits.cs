// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the limits for a JetStream server.
/// </summary>
public record JetStreamLimits
{
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
}
