// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json;
using NATS.Jwt.Models;
using Xunit;

namespace NATS.Jwt.Tests.Models;

public class NatsImportTests
{
    [Fact]
    public void TestNatsImportSerializationDeserialization()
    {
        var natsImport = new NatsImport
        {
            Name = "TestImport",
            Subject = "test.subject",
            Account = "ACC123",
            Token = "TOKEN456",
            LocalSubject = "local.subject",
            Type = NatsExportType.Service,
            Share = true,
            AllowTrace = true,
            To = "to.subject",
        };

        string json = JsonSerializer.Serialize(natsImport);

        string expectedJson = "{\"name\":\"TestImport\",\"subject\":\"test.subject\",\"account\":\"ACC123\",\"token\":\"TOKEN456\",\"to\":\"to.subject\",\"local_subject\":\"local.subject\",\"type\":\"service\",\"share\":true,\"allow_trace\":true}";

        Assert.Equal(expectedJson, json);

        var deserializedNatsImport = JsonSerializer.Deserialize<NatsImport>(json);

        Assert.NotNull(deserializedNatsImport);
        Assert.Equal(natsImport.Name, deserializedNatsImport.Name);
        Assert.Equal(natsImport.Subject, deserializedNatsImport.Subject);
        Assert.Equal(natsImport.Account, deserializedNatsImport.Account);
        Assert.Equal(natsImport.Token, deserializedNatsImport.Token);
        Assert.Equal(natsImport.LocalSubject, deserializedNatsImport.LocalSubject);
        Assert.Equal(natsImport.Type, deserializedNatsImport.Type);
        Assert.Equal(natsImport.Share, deserializedNatsImport.Share);
        Assert.Equal(natsImport.AllowTrace, deserializedNatsImport.AllowTrace);
    }
}
