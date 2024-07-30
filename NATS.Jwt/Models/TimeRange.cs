// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents a time range with a start and end time.
/// </summary>
public record TimeRange
{
    /// <summary>
    /// Gets or sets the start time of a time range formatted as HH:mm:ss representing time of the day.
    /// </summary>
    [JsonPropertyName("start")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Start { get; set; }

    /// <summary>
    /// Gets or sets the end time of a time range formatted as HH:mm:ss representing time of the day.
    /// </summary>
    [JsonPropertyName("end")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string End { get; set; }
}
