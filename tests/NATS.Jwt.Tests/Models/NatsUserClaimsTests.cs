// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Text.Json;
using NATS.Jwt.Models;
using Xunit;

namespace NATS.Jwt.Tests.Models;

public class NatsUserClaimsTests
{
    [Fact]
    public void SerializeDeserialize_FullNatsUserClaims_ShouldSucceed()
    {
        var claims = new NatsUserClaims
        {
            Subject = "UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            Name = "Full Test User",
            Audience = "test_audience",
            Expires = DateTimeOffset.FromUnixTimeSeconds(1735689600),
            IssuedAt = DateTimeOffset.FromUnixTimeSeconds(1609459200),
            NotBefore = DateTimeOffset.FromUnixTimeSeconds(1609459200),
            Id = "jti_test",
            User = new NatsUser
            {
                Type = "user",
                Version = 2,
                Pub = new NatsPermission { Allow = ["allowed.>"], Deny = ["denied.>"], },
                Sub = new NatsPermission { Allow = ["allowed.>"], Deny = ["denied.>"], },
                Resp = new NatsResponsePermission { MaxMsgs = 100, Expires = TimeSpan.FromSeconds(60), },
                Src = ["192.168.1.0/24"],
                Times = [new TimeRange { Start = "09:00:00", End = "17:00:00", }],
                Locale = "America/New_York",
                Subs = 1000,
                Data = 10_000_000,
                Payload = 1024,
                BearerToken = true,
                AllowedConnectionTypes = ["STANDARD", "WEBSOCKET"],
                IssuerAccount = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                Tags = ["tag1", "tag2"],
            },
        };

        string json = JsonSerializer.Serialize(claims);
        var deserialized = JsonSerializer.Deserialize<NatsUserClaims>(json);

        Assert.Equal(claims.Subject, deserialized.Subject);
        Assert.Equal(claims.Issuer, deserialized.Issuer);
        Assert.Equal(claims.Name, deserialized.Name);
        Assert.Equal(claims.Audience, deserialized.Audience);
        Assert.Equal(claims.Expires, deserialized.Expires);
        Assert.Equal(claims.IssuedAt, deserialized.IssuedAt);
        Assert.Equal(claims.NotBefore, deserialized.NotBefore);
        Assert.Equal(claims.Id, deserialized.Id);

        Assert.NotNull(deserialized.User);
        Assert.Equal(claims.User.Type, deserialized.User.Type);
        Assert.Equal(claims.User.Version, deserialized.User.Version);
        Assert.Equal(claims.User.Pub.Allow, deserialized.User.Pub.Allow);
        Assert.Equal(claims.User.Pub.Deny, deserialized.User.Pub.Deny);
        Assert.Equal(claims.User.Sub.Allow, deserialized.User.Sub.Allow);
        Assert.Equal(claims.User.Sub.Deny, deserialized.User.Sub.Deny);
        Assert.Equal(claims.User.Resp.MaxMsgs, deserialized.User.Resp.MaxMsgs);
        Assert.Equal(claims.User.Resp.Expires, deserialized.User.Resp.Expires);
        Assert.Equal(claims.User.Src, deserialized.User.Src);
        Assert.Equal(claims.User.Times[0].Start, deserialized.User.Times[0].Start);
        Assert.Equal(claims.User.Times[0].End, deserialized.User.Times[0].End);
        Assert.Equal(claims.User.Locale, deserialized.User.Locale);
        Assert.Equal(claims.User.Subs, deserialized.User.Subs);
        Assert.Equal(claims.User.Data, deserialized.User.Data);
        Assert.Equal(claims.User.Payload, deserialized.User.Payload);
        Assert.Equal(claims.User.BearerToken, deserialized.User.BearerToken);
        Assert.Equal(claims.User.AllowedConnectionTypes, deserialized.User.AllowedConnectionTypes);
        Assert.Equal(claims.User.IssuerAccount, deserialized.User.IssuerAccount);
        Assert.Equal(claims.User.Tags, deserialized.User.Tags);
    }

    [Fact]
    public void SerializeDeserialize_MinimalNatsUserClaims_ShouldSucceed()
    {
        var claims = new NatsUserClaims
        {
            Subject = "UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            User = new NatsUser(),
        };

        string json = JsonSerializer.Serialize(claims);
        var deserialized = JsonSerializer.Deserialize<NatsUserClaims>(json);

        Assert.Equal(claims.Subject, deserialized.Subject);
        Assert.Equal(claims.Issuer, deserialized.Issuer);
        Assert.NotNull(deserialized.User);
    }

    [Fact]
    public void Deserialize_ExtraFields_ShouldIgnore()
    {
        string json = """
        {
            "sub": "UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            "iss": "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            "nats": {
                "type": "user",
                "version": 2
            },
            "extra_field": "should be ignored"
        }
        """;

        var deserialized = JsonSerializer.Deserialize<NatsUserClaims>(json);

        Assert.Equal("UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", deserialized.Subject);
        Assert.Equal("IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII", deserialized.Issuer);
        Assert.NotNull(deserialized.User);
        Assert.Equal("user", deserialized.User.Type);
        Assert.Equal(2, deserialized.User.Version);
    }

    [Fact]
    public void Serialize_ShouldOmitDefaultValues()
    {
        var claims = new NatsUserClaims
        {
            Subject = "UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            User = new NatsUser
            {
                Type = "user",
                Version = 2,
            },
        };

        string json = JsonSerializer.Serialize(claims);

        Assert.Contains("\"sub\":\"UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\"", json);
        Assert.Contains("\"iss\":\"IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII\"", json);
        Assert.Contains("\"nats\":{", json);
        Assert.Contains("\"type\":\"user\"", json);
        Assert.Contains("\"version\":2", json);
        Assert.Contains("\"pub\":{}", json);
        Assert.Contains("\"sub\":{}", json);
        Assert.DoesNotContain("\"resp\"", json);
        Assert.DoesNotContain("\"src\"", json);
        Assert.DoesNotContain("\"times\"", json);
        Assert.DoesNotContain("\"times_location\"", json);
        Assert.Contains("\"subs\":-1", json);
        Assert.Contains("\"data\":-1", json);
        Assert.Contains("\"payload\":-1", json);
        Assert.DoesNotContain("\"bearer_token\"", json);
        Assert.DoesNotContain("\"allowed_connection_types\"", json);
        Assert.DoesNotContain("\"issuer_account\"", json);
        Assert.DoesNotContain("\"tags\"", json);
    }

    [Fact]
    public void Deserialize_MissingRequiredFields_ShouldNotThrowException()
    {
        string json = """
        {
            "sub": "UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            "iss": "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            "nats": {}
        }
        """;

        var deserialized = JsonSerializer.Deserialize<NatsUserClaims>(json);
        Assert.NotNull(deserialized);
    }

    [Fact]
    public void Deserialize_InvalidJson_ShouldThrowException()
    {
        string invalidJson = "{\"sub\": \"UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\", \"iss\": }";

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<NatsUserClaims>(invalidJson));
    }

    [Fact]
    public void Serialize_NullUser_ShouldSerializeEmptyObject()
    {
        var claims = new NatsUserClaims
        {
            Subject = "UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            User = null,
        };

        string json = JsonSerializer.Serialize(claims);

        Assert.DoesNotContain("\"nats\":null", json);
    }

    [Fact]
    public void Deserialize_NullUser_ShouldDeserializeNull()
    {
        string json = """
        {
            "sub": "UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            "iss": "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            "nats": null
        }
        """;

        var deserialized = JsonSerializer.Deserialize<NatsUserClaims>(json);

        Assert.Null(deserialized.User);
    }

    [Fact]
    public void TestSetScoped()
    {
        var initUser = (NatsUser user) =>
        {
            user.Pub = new NatsPermission { Allow = ["allowed.>"], Deny = ["denied.>"], };
            user.Sub = new NatsPermission { Allow = ["allowed.>"], Deny = ["denied.>"], };
            user.Resp = new NatsResponsePermission { MaxMsgs = 100, Expires = TimeSpan.FromSeconds(60), };
            user.Src = ["192.168.1.0/24"];
            user.Times = [new TimeRange { Start = "09:00:00", End = "17:00:00", }];
            user.Locale = "America/New_York";
            user.Subs = 1000;
            user.Data = 10_000_000;
            user.Payload = 1024;
            user.BearerToken = true;
            user.AllowedConnectionTypes = ["STANDARD", "WEBSOCKET"];
        };

        var claims = new NatsUserClaims();
        initUser(claims.User);
        claims.SetScoped(true);

        Assert.Equal(claims.User.Pub, new NatsPermission());
        Assert.Equal(claims.User.Sub, new NatsPermission());
        Assert.Equal(claims.User.Resp, default);
        Assert.Equal(claims.User.Src, default);
        Assert.Equal(claims.User.Times, default);
        Assert.Equal(claims.User.Locale, default);
        Assert.Equal(claims.User.Subs, 0);
        Assert.Equal(claims.User.Data, 0);
        Assert.Equal(claims.User.Payload, 0);
        Assert.Equal(claims.User.BearerToken, default);
        Assert.Equal(claims.User.AllowedConnectionTypes, default);

        initUser(claims.User);
        claims.SetScoped(false);

        Assert.NotNull(claims.User.Pub);
        Assert.NotNull(claims.User.Pub.Allow);
        Assert.Equal(claims.User.Pub.Allow.Count, 1);
        Assert.Equal(claims.User.Pub.Allow[0], "allowed.>");
        Assert.NotNull(claims.User.Pub.Deny);
        Assert.Equal(claims.User.Pub.Deny.Count, 1);
        Assert.Equal(claims.User.Pub.Deny[0], "denied.>");
        Assert.NotNull(claims.User.Sub);
        Assert.NotNull(claims.User.Sub.Allow);
        Assert.Equal(claims.User.Sub.Allow.Count, 1);
        Assert.Equal(claims.User.Sub.Allow[0], "allowed.>");
        Assert.NotNull(claims.User.Sub.Deny);
        Assert.Equal(claims.User.Sub.Deny.Count, 1);
        Assert.Equal(claims.User.Sub.Deny[0], "denied.>");
        Assert.Equal(claims.User.Resp, new NatsResponsePermission { MaxMsgs = 100, Expires = TimeSpan.FromSeconds(60), });
        Assert.Equal(claims.User.Src, default);
        Assert.Equal(claims.User.Times, default);
        Assert.Equal(claims.User.Locale, default);
        Assert.Equal(claims.User.Subs, NatsJwt.NoLimit);
        Assert.Equal(claims.User.Data, NatsJwt.NoLimit);
        Assert.Equal(claims.User.Payload, NatsJwt.NoLimit);
        Assert.Equal(claims.User.BearerToken, true);
        Assert.NotNull(claims.User.AllowedConnectionTypes);
        Assert.Equal(claims.User.AllowedConnectionTypes.Count, 2);
        Assert.Equal(claims.User.AllowedConnectionTypes[0], "STANDARD");
        Assert.Equal(claims.User.AllowedConnectionTypes[1], "WEBSOCKET");
    }
}
