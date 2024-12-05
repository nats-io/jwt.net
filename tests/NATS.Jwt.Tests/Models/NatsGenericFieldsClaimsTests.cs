// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Text.Json;
using NATS.Jwt.Models;
using Xunit;

namespace NATS.Jwt.Tests.Models;

public class NatsGenericFieldsClaimsTests
{
    [Fact]
    public void SerializeDeserialize_FullNatsGenericFieldsClaims_ShouldSucceed()
    {
        var claims = new NatsGenericFieldsClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            Name = "Full Test Generic Fields",
            Audience = "test_audience",
            Expires = DateTimeOffset.FromUnixTimeSeconds(1735689600),
            IssuedAt = DateTimeOffset.FromUnixTimeSeconds(1609459200),
            NotBefore = DateTimeOffset.FromUnixTimeSeconds(1609459200),
            Id = "jti_test",
            GenericFields = new NatsGenericFields
            {
                Type = "generic_type",
                Version = 2,
                Tags = ["tag1", "tag2"],
            },
        };

        string json = JsonSerializer.Serialize(claims);
        var deserialized = JsonSerializer.Deserialize<NatsGenericFieldsClaims>(json);

        Assert.Equal(claims.Subject, deserialized.Subject);
        Assert.Equal(claims.Issuer, deserialized.Issuer);
        Assert.Equal(claims.Name, deserialized.Name);
        Assert.Equal(claims.Audience, deserialized.Audience);
        Assert.Equal(claims.Expires, deserialized.Expires);
        Assert.Equal(claims.IssuedAt, deserialized.IssuedAt);
        Assert.Equal(claims.NotBefore, deserialized.NotBefore);
        Assert.Equal(claims.Id, deserialized.Id);

        Assert.NotNull(deserialized.GenericFields);
        Assert.Equal(claims.GenericFields.Type, deserialized.GenericFields.Type);
        Assert.Equal(claims.GenericFields.Version, deserialized.GenericFields.Version);
        Assert.Equal(claims.GenericFields.Tags, deserialized.GenericFields.Tags);
    }

    [Fact]
    public void SerializeDeserialize_MinimalNatsGenericFieldsClaims_ShouldSucceed()
    {
        var claims = new NatsGenericFieldsClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            GenericFields = new NatsGenericFields(),
        };

        string json = JsonSerializer.Serialize(claims);
        var deserialized = JsonSerializer.Deserialize<NatsGenericFieldsClaims>(json);

        Assert.Equal(claims.Subject, deserialized.Subject);
        Assert.Equal(claims.Issuer, deserialized.Issuer);
        Assert.NotNull(deserialized.GenericFields);
    }

    [Fact]
    public void Deserialize_ExtraFields_ShouldIgnore()
    {
        string json = """
        {
            "sub": "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            "iss": "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            "nats": {
                "type": "generic_type",
                "version": 2,
                "tags": ["tag1", "tag2"]
            },
            "extra_field": "should be ignored"
        }
        """;

        var deserialized = JsonSerializer.Deserialize<NatsGenericFieldsClaims>(json);

        Assert.Equal("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", deserialized.Subject);
        Assert.Equal("IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII", deserialized.Issuer);
        Assert.NotNull(deserialized.GenericFields);
        Assert.Equal("generic_type", deserialized.GenericFields.Type);
        Assert.Equal(2, deserialized.GenericFields.Version);
        Assert.Equal(["tag1", "tag2"], deserialized.GenericFields.Tags);
    }

    [Fact]
    public void Serialize_ShouldOmitDefaultValues()
    {
        var claims = new NatsGenericFieldsClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            GenericFields = new NatsGenericFields
            {
                Type = "generic_type",
                Version = 2,
            },
        };

        string json = JsonSerializer.Serialize(claims);

        Assert.Contains("\"sub\":\"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\"", json);
        Assert.Contains("\"iss\":\"IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII\"", json);
        Assert.Contains("\"nats\":{", json);
        Assert.Contains("\"type\":\"generic_type\"", json);
        Assert.Contains("\"version\":2", json);
        Assert.DoesNotContain("\"tags\"", json);
        Assert.DoesNotContain("\"aud\"", json);
        Assert.DoesNotContain("\"exp\"", json);
        Assert.DoesNotContain("\"iat\"", json);
        Assert.DoesNotContain("\"nbf\"", json);
        Assert.DoesNotContain("\"jti\"", json);
    }

    [Fact]
    public void Deserialize_MissingRequiredFields_ShouldNotThrowException()
    {
        string json = """
        {
            "sub": "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            "iss": "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            "nats": {}
        }
        """;

        var deserialized = JsonSerializer.Deserialize<NatsGenericFieldsClaims>(json);
        Assert.NotNull(deserialized);
    }

    [Fact]
    public void Deserialize_InvalidJson_ShouldThrowException()
    {
        string invalidJson = "{\"sub\": \"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\", \"iss\": }";

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<NatsGenericFieldsClaims>(invalidJson));
    }

    [Fact]
    public void Serialize_NullGenericFields_ShouldSerializeEmptyObject()
    {
        var claims = new NatsGenericFieldsClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            GenericFields = null,
        };

        string json = JsonSerializer.Serialize(claims);

        Assert.DoesNotContain("\"nats\":null", json);
    }

    [Fact]
    public void Deserialize_NullGenericFields_ShouldDeserializeNull()
    {
        string json = """
        {
            "sub": "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            "iss": "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            "nats": null
        }
        """;

        var deserialized = JsonSerializer.Deserialize<NatsGenericFieldsClaims>(json);

        Assert.Null(deserialized.GenericFields);
    }
}
