// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the message tracing details for a NATS message.
/// </summary>
public record NatsMsgTrace
{
    /// <summary>
    /// Gets or sets destination is the subject the server will send message traces to
    /// if the inbound message contains the "traceparent" header and has
    /// its sampled field indicating that the trace should be triggered.
    /// </summary>
    [JsonPropertyName("dest")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Destination { get; set; }

    /// <summary>
    /// Gets or sets sampling which is used to set the probability sampling, that is, the
    /// server will get a random number between 1 and 100 and trigger
    /// the trace if the number is lower than this Sampling value.
    /// The valid range is [1..100]. If the value is not set, Validate()
    /// will set the value to 100.
    /// </summary>
    [JsonPropertyName("sampling")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Sampling { get; set; }
}
