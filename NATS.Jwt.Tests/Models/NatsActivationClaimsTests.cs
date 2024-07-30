// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json;
using NATS.Jwt.Models;
using Xunit;

namespace NATS.Jwt.Tests.Models;

public class NatsActivationClaimsTests
{
    [Fact]
    public void SerializeDeserialize_FullNatsActivationClaims_ShouldSucceed()
    {
        var claims = new NatsActivationClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            Name = "Full Test Activation",
            Audience = "test_audience",
            Expires = 1735689600, // 2025-01-01
            IssuedAt = 1609459200, // 2021-01-01
            NotBefore = 1609459200, // 2021-01-01
            Id = "jti_test",
            Activation = new NatsActivation
            {
                ImportSubject = "import.>",
                ImportType = 1,
                IssuerAccount = "ACCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC",
                Type = "activation",
                Version = 2,
                Tags =
                [
                    "tag1",
                    "tag2"
                ],
            },
        };

        string json = JsonSerializer.Serialize(claims);
        var deserialized = JsonSerializer.Deserialize<NatsActivationClaims>(json);

        Assert.Equal(claims.Subject, deserialized.Subject);
        Assert.Equal(claims.Issuer, deserialized.Issuer);
        Assert.Equal(claims.Name, deserialized.Name);
        Assert.Equal(claims.Audience, deserialized.Audience);
        Assert.Equal(claims.Expires, deserialized.Expires);
        Assert.Equal(claims.IssuedAt, deserialized.IssuedAt);
        Assert.Equal(claims.NotBefore, deserialized.NotBefore);
        Assert.Equal(claims.Id, deserialized.Id);

        Assert.NotNull(deserialized.Activation);
        Assert.Equal(claims.Activation.ImportSubject, deserialized.Activation.ImportSubject);
        Assert.Equal(claims.Activation.ImportType, deserialized.Activation.ImportType);
        Assert.Equal(claims.Activation.IssuerAccount, deserialized.Activation.IssuerAccount);
        Assert.Equal(claims.Activation.Type, deserialized.Activation.Type);
        Assert.Equal(claims.Activation.Version, deserialized.Activation.Version);
        Assert.Equal(claims.Activation.Tags, deserialized.Activation.Tags);
    }

    [Fact]
    public void SerializeDeserialize_MinimalNatsActivationClaims_ShouldSucceed()
    {
        var claims = new NatsActivationClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            Activation = new NatsActivation
            {
                ImportSubject = "import.>",
                ImportType = 1,
                IssuerAccount = "ACCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC",
            },
        };

        string json = JsonSerializer.Serialize(claims);
        var deserialized = JsonSerializer.Deserialize<NatsActivationClaims>(json);

        Assert.Equal(claims.Subject, deserialized.Subject);
        Assert.Equal(claims.Issuer, deserialized.Issuer);
        Assert.NotNull(deserialized.Activation);
        Assert.Equal(claims.Activation.ImportSubject, deserialized.Activation.ImportSubject);
        Assert.Equal(claims.Activation.ImportType, deserialized.Activation.ImportType);
        Assert.Equal(claims.Activation.IssuerAccount, deserialized.Activation.IssuerAccount);
    }

    [Fact]
    public void Deserialize_ExtraFields_ShouldIgnore()
    {
        string json = """
        {
            "sub": "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            "iss": "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            "nats": {
                "subject": "import.>",
                "kind": 1,
                "issuer_account": "ACCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC"
            },
            "extra_field": "should be ignored"
        }
        """;

        var deserialized = JsonSerializer.Deserialize<NatsActivationClaims>(json);

        Assert.Equal("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", deserialized.Subject);
        Assert.Equal("IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII", deserialized.Issuer);
        Assert.NotNull(deserialized.Activation);
        Assert.Equal("import.>", deserialized.Activation.ImportSubject);
        Assert.Equal(1, deserialized.Activation.ImportType);
        Assert.Equal("ACCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC", deserialized.Activation.IssuerAccount);
    }

    [Fact]
    public void Serialize_ShouldOmitDefaultValues()
    {
        var claims = new NatsActivationClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            Activation = new NatsActivation
            {
                ImportSubject = "import.>",
                ImportType = 1,
                IssuerAccount = "ACCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC",
            },
        };

        string json = JsonSerializer.Serialize(claims);

        Assert.Contains("\"sub\":\"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\"", json);
        Assert.Contains("\"iss\":\"IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII\"", json);
        Assert.Contains("\"nats\":{", json);
        Assert.Contains("\"subject\":\"import.\\u003E\"", json);
        Assert.Contains("\"kind\":1", json);
        Assert.Contains("\"issuer_account\":\"ACCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC\"", json);
        Assert.DoesNotContain("\"aud\"", json);
        Assert.DoesNotContain("\"exp\"", json);
        Assert.DoesNotContain("\"iat\"", json);
        Assert.DoesNotContain("\"nbf\"", json);
        Assert.DoesNotContain("\"jti\"", json);
    }

    [Fact]
    public void Deserialize_InvalidJson_ShouldThrowException()
    {
        string invalidJson = "{\"sub\": \"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\", \"iss\": }";

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<NatsActivationClaims>(invalidJson));
    }
}
