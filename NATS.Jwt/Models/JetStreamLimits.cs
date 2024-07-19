using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record JetStreamLimits
{
    /// <summary>
    /// Max number of bytes stored in memory across all streams. (0 means disabled)
    /// </summary>
    [JsonPropertyName("mem_storage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long MemoryStorage { get; set; }

    /// <summary>
    /// Max number of bytes stored on disk across all streams. (0 means disabled)
    /// </summary>
    [JsonPropertyName("disk_storage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long DiskStorage { get; set; }

    /// <summary>
    /// Max number of streams
    /// </summary>
    [JsonPropertyName("streams")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Streams { get; set; }

    /// <summary>
    /// Max number of consumers
    /// </summary>
    [JsonPropertyName("consumer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Consumer { get; set; }

    /// <summary>
    /// Max ack pending of a Stream
    /// </summary>
    [JsonPropertyName("max_ack_pending")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long MaxAckPending { get; set; }

    /// <summary>
    /// Max bytes a memory backed stream can have. (0 means disabled/unlimited)
    /// </summary>
    [JsonPropertyName("mem_max_stream_bytes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long MemoryMaxStreamBytes { get; set; }

    /// <summary>
    /// Max bytes a disk backed stream can have. (0 means disabled/unlimited)
    /// </summary>
    [JsonPropertyName("disk_max_stream_bytes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long DiskMaxStreamBytes { get; set; }

    /// <summary>
    /// Max bytes required by all Streams
    /// </summary>
    [JsonPropertyName("max_bytes_required")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool MaxBytesRequired { get; set; }
}
