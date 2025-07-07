// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Text.Json;
using NATS.Jwt.Models;
using Xunit;

namespace NATS.Jwt.Tests.Models;

public class NatsOperatorClaimsTests
{
    [Fact]
    public void SerializeDeserialize_FullNatsOperatorClaims_ShouldSucceed()
    {
        var claims = new NatsOperatorClaims
        {
            Subject = "OAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "OIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            Name = "Full Test Operator",
            Audience = "test_audience",
            Expires = DateTimeOffset.FromUnixTimeSeconds(1735689600), // 2025-01-01
            IssuedAt = DateTimeOffset.FromUnixTimeSeconds(1609459200), // 2021-01-01
            NotBefore = DateTimeOffset.FromUnixTimeSeconds(1609459200), // 2021-01-01
            Id = "jti_test",
            Operator = new NatsOperator
            {
                Type = "operator",
                Version = 2,
                SigningKeys = new List<string>
                {
                    "SKAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                    "SKBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB",
                },
                AccountServerUrl = "https://account-server.example.com",
                OperatorServiceUrLs = new List<string>
                {
                    "nats://operator1.example.com:4222",
                    "nats://operator2.example.com:4222",
                },
                SystemAccount = "SAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                AssertServerVersion = "2.2.0",
                StrictSigningKeyUsage = true,
                Tags = new NatsTags { "tag1", "tag2", },
            },
        };

        string json = JsonSerializer.Serialize(claims);
        var deserialized = JsonSerializer.Deserialize<NatsOperatorClaims>(json);

        Assert.Equal(claims.Subject, deserialized.Subject);
        Assert.Equal(claims.Issuer, deserialized.Issuer);
        Assert.Equal(claims.Name, deserialized.Name);
        Assert.Equal(claims.Audience, deserialized.Audience);
        Assert.Equal(claims.Expires, deserialized.Expires);
        Assert.Equal(claims.IssuedAt, deserialized.IssuedAt);
        Assert.Equal(claims.NotBefore, deserialized.NotBefore);
        Assert.Equal(claims.Id, deserialized.Id);

        Assert.NotNull(deserialized.Operator);
        Assert.Equal(claims.Operator.Type, deserialized.Operator.Type);
        Assert.Equal(claims.Operator.Version, deserialized.Operator.Version);
        Assert.Equal(claims.Operator.SigningKeys, deserialized.Operator.SigningKeys);
        Assert.Equal(claims.Operator.AccountServerUrl, deserialized.Operator.AccountServerUrl);
        Assert.Equal(claims.Operator.OperatorServiceUrLs, deserialized.Operator.OperatorServiceUrLs);
        Assert.Equal(claims.Operator.SystemAccount, deserialized.Operator.SystemAccount);
        Assert.Equal(claims.Operator.AssertServerVersion, deserialized.Operator.AssertServerVersion);
        Assert.Equal(claims.Operator.StrictSigningKeyUsage, deserialized.Operator.StrictSigningKeyUsage);
        Assert.Equal(claims.Operator.Tags, deserialized.Operator.Tags);
    }

    [Fact]
    public void SerializeDeserialize_MinimalNatsOperatorClaims_ShouldSucceed()
    {
        var claims = new NatsOperatorClaims
        {
            Subject = "OAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "OIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            Operator = new NatsOperator(),
        };

        string json = JsonSerializer.Serialize(claims);
        var deserialized = JsonSerializer.Deserialize<NatsOperatorClaims>(json);

        Assert.Equal(claims.Subject, deserialized.Subject);
        Assert.Equal(claims.Issuer, deserialized.Issuer);
        Assert.NotNull(deserialized.Operator);
    }

    [Fact]
    public void Deserialize_ExtraFields_ShouldIgnore()
    {
        string json = """
        {
            "sub": "OAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            "iss": "OIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            "nats": {},
            "extra_field": "should be ignored"
        }
        """;

        var deserialized = JsonSerializer.Deserialize<NatsOperatorClaims>(json);

        Assert.Equal("OAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", deserialized.Subject);
        Assert.Equal("OIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII", deserialized.Issuer);
        Assert.NotNull(deserialized.Operator);
    }

    [Fact]
    public void Serialize_ShouldOmitDefaultValues()
    {
        var claims = new NatsOperatorClaims
        {
            Subject = "OAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "OIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            Operator = new NatsOperator(),
        };

        string json = JsonSerializer.Serialize(claims);

        Assert.Contains("\"sub\":\"OAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\"", json);
        Assert.Contains("\"iss\":\"OIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII\"", json);
        Assert.Contains("\"nats\":{}", json);
        Assert.DoesNotContain("\"aud\"", json);
        Assert.DoesNotContain("\"exp\"", json);
        Assert.DoesNotContain("\"iat\"", json);
        Assert.DoesNotContain("\"nbf\"", json);
        Assert.DoesNotContain("\"jti\"", json);
    }

    [Fact]
    public void Deserialize_InvalidJson_ShouldThrowException()
    {
        string invalidJson = "{\"sub\": \"OAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\", \"iss\": }";

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<NatsOperatorClaims>(invalidJson));
    }

    [Fact]
    public void Serialize_NullOperator_ShouldSerializeEmptyObject()
    {
        var claims = new NatsOperatorClaims
        {
            Subject = "OAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "OIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            Operator = null,
        };

        string json = JsonSerializer.Serialize(claims);

        Assert.DoesNotContain("\"nats\":null", json);
    }

    [Fact]
    public void Deserialize_NullOperator_ShouldDeserializeNull()
    {
        string json = """
        {
            "sub": "OAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            "iss": "OIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            "nats": null
        }
        """;

        var deserialized = JsonSerializer.Deserialize<NatsOperatorClaims>(json);

        Assert.Null(deserialized.Operator);
    }
}
