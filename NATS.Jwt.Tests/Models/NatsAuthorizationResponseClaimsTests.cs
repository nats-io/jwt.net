// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json;
using NATS.Jwt.Models;
using Xunit;

namespace NATS.Jwt.Tests.Models;

public class NatsAuthorizationResponseClaimsTests
{
    [Fact]
    public void SerializeDeserialize_FullNatsAuthorizationResponseClaims_ShouldSucceed()
    {
        var claims = new NatsAuthorizationResponseClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            Name = "Full Test Authorization Response",
            Audience = "test_audience",
            Expires = 1735689600,
            IssuedAt = 1609459200,
            NotBefore = 1609459200,
            Id = "jti_test",
            AuthorizationResponse = new NatsAuthorizationResponse
            {
                Jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpJVCL9...",
                Error = string.Empty,
                IssuerAccount = "ACCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC",
                Type = "authorization_response",
                Version = 2,
                Tags = ["tag1", "tag2"],
            },
        };

        string json = JsonSerializer.Serialize(claims);
        var deserialized = JsonSerializer.Deserialize<NatsAuthorizationResponseClaims>(json);

        Assert.Equal(claims.Subject, deserialized.Subject);
        Assert.Equal(claims.Issuer, deserialized.Issuer);
        Assert.Equal(claims.Name, deserialized.Name);
        Assert.Equal(claims.Audience, deserialized.Audience);
        Assert.Equal(claims.Expires, deserialized.Expires);
        Assert.Equal(claims.IssuedAt, deserialized.IssuedAt);
        Assert.Equal(claims.NotBefore, deserialized.NotBefore);
        Assert.Equal(claims.Id, deserialized.Id);

        Assert.NotNull(deserialized.AuthorizationResponse);
        Assert.Equal(claims.AuthorizationResponse.Jwt, deserialized.AuthorizationResponse.Jwt);
        Assert.Equal(claims.AuthorizationResponse.Error, deserialized.AuthorizationResponse.Error);
        Assert.Equal(claims.AuthorizationResponse.IssuerAccount, deserialized.AuthorizationResponse.IssuerAccount);
        Assert.Equal(claims.AuthorizationResponse.Type, deserialized.AuthorizationResponse.Type);
        Assert.Equal(claims.AuthorizationResponse.Version, deserialized.AuthorizationResponse.Version);
        Assert.Equal(claims.AuthorizationResponse.Tags, deserialized.AuthorizationResponse.Tags);
    }

    [Fact]
    public void SerializeDeserialize_MinimalNatsAuthorizationResponseClaims_ShouldSucceed()
    {
        var claims = new NatsAuthorizationResponseClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            AuthorizationResponse = new NatsAuthorizationResponse
            {
                Jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                Error = string.Empty,
                IssuerAccount = "ACCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC",
            },
        };

        string json = JsonSerializer.Serialize(claims);
        var deserialized = JsonSerializer.Deserialize<NatsAuthorizationResponseClaims>(json);

        Assert.Equal(claims.Subject, deserialized.Subject);
        Assert.Equal(claims.Issuer, deserialized.Issuer);
        Assert.NotNull(deserialized.AuthorizationResponse);
        Assert.Equal(claims.AuthorizationResponse.Jwt, deserialized.AuthorizationResponse.Jwt);
        Assert.Equal(claims.AuthorizationResponse.Error, deserialized.AuthorizationResponse.Error);
        Assert.Equal(claims.AuthorizationResponse.IssuerAccount, deserialized.AuthorizationResponse.IssuerAccount);
    }

    [Fact]
    public void Deserialize_ExtraFields_ShouldIgnore()
    {
        string json = """
        {
            "sub": "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            "iss": "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            "nats": {
                "jwt": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                "error": "",
                "issuer_account": "ACCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC"
            },
            "extra_field": "should be ignored"
        }
        """;

        var deserialized = JsonSerializer.Deserialize<NatsAuthorizationResponseClaims>(json);

        Assert.Equal("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", deserialized.Subject);
        Assert.Equal("IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII", deserialized.Issuer);
        Assert.NotNull(deserialized.AuthorizationResponse);
        Assert.Equal("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...", deserialized.AuthorizationResponse.Jwt);
        Assert.Equal(string.Empty, deserialized.AuthorizationResponse.Error);
        Assert.Equal("ACCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC", deserialized.AuthorizationResponse.IssuerAccount);
    }

    [Fact]
    public void Serialize_ShouldOmitDefaultValues()
    {
        var claims = new NatsAuthorizationResponseClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            AuthorizationResponse = new NatsAuthorizationResponse
            {
                Jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                Error = string.Empty,
                IssuerAccount = "ACCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC",
            },
        };

        string json = JsonSerializer.Serialize(claims);

        Assert.Contains("\"sub\":\"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\"", json);
        Assert.Contains("\"iss\":\"IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII\"", json);
        Assert.Contains("\"nats\":{", json);
        Assert.Contains("\"jwt\":\"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\"", json);
        Assert.Contains("\"error\":\"\"", json);
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

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<NatsAuthorizationResponseClaims>(invalidJson));
    }

    [Fact]
    public void Serialize_NullAuthorizationResponse_ShouldSerializeEmptyObject()
    {
        var claims = new NatsAuthorizationResponseClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            AuthorizationResponse = null,
        };

        string json = JsonSerializer.Serialize(claims);

        Assert.DoesNotContain("\"nats\":null", json);
    }

    [Fact]
    public void Deserialize_NullAuthorizationResponse_ShouldDeserializeNull()
    {
        string json = """
        {
            "sub": "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            "iss": "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            "nats": null
        }
        """;

        var deserialized = JsonSerializer.Deserialize<NatsAuthorizationResponseClaims>(json);

        Assert.Null(deserialized.AuthorizationResponse);
    }
}
