// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json;
using NATS.Jwt.Models;
using Xunit;

namespace NATS.Jwt.Tests.Models;

public class NatsServerIdTests
{
    [Fact]
    public void TestNatsServerIdSerializationDeserialization()
    {
        var natsServerId = new NatsServerId
        {
            Name = "TestServer",
            Host = "localhost",
            Id = "ABCDEFG",
            Version = "2.9.0",
            Cluster = "TestCluster",
            Tags = ["tag1", "tag2", "tag3"],
            XKey = "XKEY123456",
        };

        string json = JsonSerializer.Serialize(natsServerId);

        string expectedJson = "{\"name\":\"TestServer\",\"host\":\"localhost\",\"id\":\"ABCDEFG\",\"version\":\"2.9.0\",\"cluster\":\"TestCluster\",\"tags\":[\"tag1\",\"tag2\",\"tag3\"],\"xkey\":\"XKEY123456\"}";

        Assert.Equal(expectedJson, json);

        var deserializedNatsServerId = JsonSerializer.Deserialize<NatsServerId>(json);

        Assert.NotNull(deserializedNatsServerId);
        Assert.Equal(natsServerId.Name, deserializedNatsServerId.Name);
        Assert.Equal(natsServerId.Host, deserializedNatsServerId.Host);
        Assert.Equal(natsServerId.Id, deserializedNatsServerId.Id);
        Assert.Equal(natsServerId.Version, deserializedNatsServerId.Version);
        Assert.Equal(natsServerId.Cluster, deserializedNatsServerId.Cluster);
        Assert.Equal(natsServerId.Tags, deserializedNatsServerId.Tags);
        Assert.Equal(natsServerId.XKey, deserializedNatsServerId.XKey);
    }
}
