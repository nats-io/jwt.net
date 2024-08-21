// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Text.Json;
using NATS.Jwt.Models;
using Xunit;

namespace NATS.Jwt.Tests.Models;

public class NatsExportTests
{
    [Fact]
    public void TestNatsExportSerializationDeserialization()
    {
        var natsExport = new NatsExport
        {
            Name = "TestExport",
            Subject = "test.subject",
            Type = NatsExportType.Service,
            TokenReq = true,
            Revocations = new() { { "key1", 123456789L }, { "key2", 987654321L }, },
            ResponseType = "Stream",
            ResponseThreshold = TimeSpan.FromSeconds(5),
            Latency = new NatsServiceLatency { Sampling = 50, Results = "results.subject", },
            AccountTokenPosition = 2,
            Advertise = true,
            AllowTrace = true,
            Description = "Test Description",
            InfoUrl = "https://example.com/info",
        };

        string json = JsonSerializer.Serialize(natsExport);

        string expectedJson = "{\"name\":\"TestExport\",\"subject\":\"test.subject\",\"type\":\"service\",\"token_req\":true,\"revocations\":{\"key1\":123456789,\"key2\":987654321},\"response_type\":\"Stream\",\"response_threshold\":\"00:00:05\",\"service_latency\":{\"sampling\":50,\"results\":\"results.subject\"},\"account_token_position\":2,\"advertise\":true,\"allow_trace\":true,\"description\":\"Test Description\",\"info_url\":\"https://example.com/info\"}";

        Assert.Equal(expectedJson, json);

        var deserializedNatsExport = JsonSerializer.Deserialize<NatsExport>(json);

        Assert.NotNull(deserializedNatsExport);
        Assert.Equal(natsExport.Name, deserializedNatsExport.Name);
        Assert.Equal(natsExport.Subject, deserializedNatsExport.Subject);
        Assert.Equal(natsExport.Type, deserializedNatsExport.Type);
        Assert.Equal(natsExport.TokenReq, deserializedNatsExport.TokenReq);
        Assert.Equal(natsExport.Revocations, deserializedNatsExport.Revocations);
        Assert.Equal(natsExport.ResponseType, deserializedNatsExport.ResponseType);
        Assert.Equal(natsExport.ResponseThreshold, deserializedNatsExport.ResponseThreshold);
        Assert.Equal(natsExport.Latency.Sampling, deserializedNatsExport.Latency.Sampling);
        Assert.Equal(natsExport.Latency.Results, deserializedNatsExport.Latency.Results);
        Assert.Equal(natsExport.AccountTokenPosition, deserializedNatsExport.AccountTokenPosition);
        Assert.Equal(natsExport.Advertise, deserializedNatsExport.Advertise);
        Assert.Equal(natsExport.AllowTrace, deserializedNatsExport.AllowTrace);
        Assert.Equal(natsExport.Description, deserializedNatsExport.Description);
        Assert.Equal(natsExport.InfoUrl, deserializedNatsExport.InfoUrl);
    }

    [Theory]
    [InlineData(NatsExportType.Unknown, "unknown")]
    [InlineData(NatsExportType.Stream, "stream")]
    [InlineData(NatsExportType.Service, "service")]
    public void TestExportTypeSerializationDeserialization(NatsExportType type, string jsonString)
    {
        var export = new NatsExport { Type = type };

        string json = JsonSerializer.Serialize(export);

        string expectedJson = type == NatsExportType.Unknown ? "{}" : $"{{\"type\":\"{jsonString}\"}}";

        Assert.Equal(expectedJson, json);

        var deserializedNatsExportType = JsonSerializer.Deserialize<NatsExport>(json);

        Assert.Equal(type, deserializedNatsExportType.Type);
    }

    [Fact]
    public void TestExportTypeExceptionsInSerializationDeserialization()
    {
        var export = new NatsExport { Type = (NatsExportType)42 };
        Assert.Throws<InvalidOperationException>(() => JsonSerializer.Serialize(export));

        string json = "{\"type\":\"not-a-valid-value\"}";
        Assert.Throws<InvalidOperationException>(() => JsonSerializer.Deserialize<NatsExport>(json));

        string json2 = "{\"type\":\"unknown\"}";
        var deserializedNatsExportType = JsonSerializer.Deserialize<NatsExport>(json2);
        Assert.Equal(NatsExportType.Unknown, deserializedNatsExportType.Type);

        string json3 = "{\"type\":1}";
        Assert.Throws<InvalidOperationException>(() => JsonSerializer.Deserialize<NatsExport>(json3));
    }
}
