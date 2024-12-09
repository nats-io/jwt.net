// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json;
using NATS.Jwt.Models;
using Xunit;

namespace NATS.Jwt.Tests.Models;

public class JetStreamLimitsTests
{
    [Fact]
    public void SerializeJetStreamLimits_AllPropertiesSet()
    {
        var limits = new JetStreamLimits
        {
            MemoryStorage = 1024,
            DiskStorage = 2048,
            Streams = 10,
            Consumer = 5,
            MaxAckPending = 100,
            MemoryMaxStreamBytes = 512,
            DiskMaxStreamBytes = 1024,
            MaxBytesRequired = true,
        };

        var json = JsonSerializer.Serialize(limits);
        var expected = "{\"mem_storage\":1024,\"disk_storage\":2048,\"streams\":10,\"consumer\":5,\"max_ack_pending\":100,\"mem_max_stream_bytes\":512,\"disk_max_stream_bytes\":1024,\"max_bytes_required\":true}";

        Assert.Equal(expected, json);
    }

    [Fact]
    public void SerializeJetStreamLimits_DefaultValues()
    {
        var limits = new JetStreamLimits();

        var json = JsonSerializer.Serialize(limits);
        var expected = "{}";

        Assert.Equal(expected, json);
    }

    [Fact]
    public void DeserializeJetStreamLimits_AllPropertiesSet()
    {
        var json = "{\"mem_storage\":1024,\"disk_storage\":2048,\"streams\":10,\"consumer\":5,\"max_ack_pending\":100,\"mem_max_stream_bytes\":512,\"disk_max_stream_bytes\":1024,\"max_bytes_required\":true}";

        var limits = JsonSerializer.Deserialize<JetStreamLimits>(json);

        Assert.NotNull(limits);
        Assert.Equal(1024, limits.MemoryStorage);
        Assert.Equal(2048, limits.DiskStorage);
        Assert.Equal(10, limits.Streams);
        Assert.Equal(5, limits.Consumer);
        Assert.Equal(100, limits.MaxAckPending);
        Assert.Equal(512, limits.MemoryMaxStreamBytes);
        Assert.Equal(1024, limits.DiskMaxStreamBytes);
        Assert.True(limits.MaxBytesRequired);
    }

    [Fact]
    public void DeserializeJetStreamLimits_PartialProperties()
    {
        var json = "{\"mem_storage\":1024,\"streams\":10,\"max_bytes_required\":true}";

        var limits = JsonSerializer.Deserialize<JetStreamLimits>(json);

        Assert.NotNull(limits);
        Assert.Equal(1024, limits.MemoryStorage);
        Assert.Equal(0, limits.DiskStorage);
        Assert.Equal(10, limits.Streams);
        Assert.Equal(0, limits.Consumer);
        Assert.Equal(0, limits.MaxAckPending);
        Assert.Equal(0, limits.MemoryMaxStreamBytes);
        Assert.Equal(0, limits.DiskMaxStreamBytes);
        Assert.True(limits.MaxBytesRequired);
    }

    [Fact]
    public void DeserializeJetStreamLimits_EmptyJson()
    {
        var json = "{}";

        var limits = JsonSerializer.Deserialize<JetStreamLimits>(json);

        Assert.NotNull(limits);
        Assert.Equal(0, limits.MemoryStorage);
        Assert.Equal(0, limits.DiskStorage);
        Assert.Equal(0, limits.Streams);
        Assert.Equal(0, limits.Consumer);
        Assert.Equal(0, limits.MaxAckPending);
        Assert.Equal(0, limits.MemoryMaxStreamBytes);
        Assert.Equal(0, limits.DiskMaxStreamBytes);
        Assert.False(limits.MaxBytesRequired);
    }
}
