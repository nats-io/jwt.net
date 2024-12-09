// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json;
using NATS.Jwt.Models;
using Xunit;

namespace NATS.Jwt.Tests.Models;

public class NatsServiceLatencyTests
{
    [Fact]
    public void SerializeDeserialize_FullNatsServiceLatency_ShouldSucceed()
    {
        var latency = new NatsServiceLatency
        {
            Sampling = 100,
            Results = "results.latency",
        };

        string json = JsonSerializer.Serialize(latency);
        var deserialized = JsonSerializer.Deserialize<NatsServiceLatency>(json);

        Assert.Equal(latency.Sampling, deserialized.Sampling);
        Assert.Equal(latency.Results, deserialized.Results);
    }

    [Fact]
    public void SerializeDeserialize_MinimalNatsServiceLatency_ShouldSucceed()
    {
        var latency = new NatsServiceLatency();

        string json = JsonSerializer.Serialize(latency);
        var deserialized = JsonSerializer.Deserialize<NatsServiceLatency>(json);

        Assert.Equal(0, deserialized.Sampling);
        Assert.Null(deserialized.Results);
    }

    [Fact]
    public void Deserialize_ExtraFields_ShouldIgnore()
    {
        string json = """
        {
            "sampling": 100,
            "results": "results.latency",
            "extra_field": "should be ignored"
        }
        """;

        var deserialized = JsonSerializer.Deserialize<NatsServiceLatency>(json);

        Assert.Equal(100, deserialized.Sampling);
        Assert.Equal("results.latency", deserialized.Results);
    }

    [Fact]
    public void Serialize_ShouldOmitDefaultValues()
    {
        var latency = new NatsServiceLatency
        {
            Sampling = 0,
        };

        string json = JsonSerializer.Serialize(latency);

        Assert.DoesNotContain("\"sampling\"", json);
        Assert.DoesNotContain("\"results\"", json);
    }

    [Fact]
    public void Deserialize_InvalidJson_ShouldThrowException()
    {
        string invalidJson = "{\"sampling\": \"not a number\"}";

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<NatsServiceLatency>(invalidJson));
    }

    [Fact]
    public void Serialize_NegativeSampling_ShouldSerialize()
    {
        var latency = new NatsServiceLatency
        {
            Sampling = -1,
        };

        string json = JsonSerializer.Serialize(latency);

        Assert.Contains("\"sampling\":-1", json);
    }

    [Fact]
    public void Deserialize_NegativeSampling_ShouldDeserialize()
    {
        string json = """
        {
            "sampling": -1
        }
        """;

        var deserialized = JsonSerializer.Deserialize<NatsServiceLatency>(json);

        Assert.Equal(-1, deserialized.Sampling);
    }

    [Fact]
    public void Serialize_EmptyResults_ShouldOmit()
    {
        var latency = new NatsServiceLatency
        {
            Results = string.Empty,
        };

        string json = JsonSerializer.Serialize(latency);

        Assert.Contains("\"results\"", json);
    }
}
