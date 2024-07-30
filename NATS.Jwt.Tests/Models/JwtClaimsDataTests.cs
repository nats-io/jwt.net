// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json;
using NATS.Jwt.Models;
using Xunit;

namespace NATS.Jwt.Tests.Models;

public class JwtClaimsDataTests
{
    [Fact]
    public void SerializeDeserialize_FullJwtClaimsData_ShouldSucceed()
    {
        var claims = new JwtClaimsData
        {
            Audience = "test_audience",
            Id = "test_id",
            IssuedAt = 1609459200, // 2021-01-01
            Issuer = "test_issuer",
            Name = "Test JWT",
            Subject = "test_subject",
            Expires = 1735689600, // 2025-01-01
            NotBefore = 1609459200, // 2021-01-01
        };

        string json = JsonSerializer.Serialize(claims);
        var deserialized = JsonSerializer.Deserialize<JwtClaimsData>(json);

        Assert.Equal(claims.Audience, deserialized.Audience);
        Assert.Equal(claims.Id, deserialized.Id);
        Assert.Equal(claims.IssuedAt, deserialized.IssuedAt);
        Assert.Equal(claims.Issuer, deserialized.Issuer);
        Assert.Equal(claims.Name, deserialized.Name);
        Assert.Equal(claims.Subject, deserialized.Subject);
        Assert.Equal(claims.Expires, deserialized.Expires);
        Assert.Equal(claims.NotBefore, deserialized.NotBefore);
    }

    [Fact]
    public void SerializeDeserialize_MinimalJwtClaimsData_ShouldSucceed()
    {
        var claims = new JwtClaimsData
        {
            Subject = "test_subject",
            Issuer = "test_issuer",
        };

        string json = JsonSerializer.Serialize(claims);
        var deserialized = JsonSerializer.Deserialize<JwtClaimsData>(json);

        Assert.Equal(claims.Subject, deserialized.Subject);
        Assert.Equal(claims.Issuer, deserialized.Issuer);
        Assert.Null(deserialized.Audience);
        Assert.Null(deserialized.Id);
        Assert.Equal(0, deserialized.IssuedAt);
        Assert.Null(deserialized.Name);
        Assert.Equal(0, deserialized.Expires);
        Assert.Equal(0, deserialized.NotBefore);
    }

    [Fact]
    public void Deserialize_ExtraFields_ShouldIgnore()
    {
        string json = """
        {
            "sub": "test_subject",
            "iss": "test_issuer",
            "extra_field": "should be ignored"
        }
        """;

        var deserialized = JsonSerializer.Deserialize<JwtClaimsData>(json);

        Assert.Equal("test_subject", deserialized.Subject);
        Assert.Equal("test_issuer", deserialized.Issuer);
        Assert.Null(deserialized.Audience);
        Assert.Null(deserialized.Id);
        Assert.Equal(0, deserialized.IssuedAt);
        Assert.Null(deserialized.Name);
        Assert.Equal(0, deserialized.Expires);
        Assert.Equal(0, deserialized.NotBefore);
    }

    [Fact]
    public void Serialize_ShouldOmitDefaultValues()
    {
        var claims = new JwtClaimsData
        {
            Subject = "test_subject",
            Issuer = "test_issuer",
        };

        string json = JsonSerializer.Serialize(claims);

        Assert.Contains("\"sub\":\"test_subject\"", json);
        Assert.Contains("\"iss\":\"test_issuer\"", json);
        Assert.DoesNotContain("\"aud\"", json);
        Assert.DoesNotContain("\"jti\"", json);
        Assert.DoesNotContain("\"iat\"", json);
        Assert.DoesNotContain("\"name\"", json);
        Assert.DoesNotContain("\"exp\"", json);
        Assert.DoesNotContain("\"nbf\"", json);
    }

    [Fact]
    public void Deserialize_InvalidJson_ShouldThrowException()
    {
        string invalidJson = "{\"sub\": \"test_subject\", \"iss\": }";

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<JwtClaimsData>(invalidJson));
    }
}
