// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Text.Json;
using NATS.Jwt.Models;
using Xunit;

namespace NATS.Jwt.Tests.Models;

public class NatsAccountClaimsTests
{
    [Fact]
    public void SerializeDeserialize_FullNatsAccountClaims_ShouldSucceed()
    {
        var claims = new NatsAccountClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            Name = "Full Test Account",
            Audience = "test_audience",
            Expires = 1735689600, // 2025-01-01
            IssuedAt = 1609459200, // 2021-01-01
            NotBefore = 1609459200, // 2021-01-01
            Id = "jti_test",
            Account = new NatsAccount
            {
                Type = "account",
                Version = 2,
                Description = "Test Account Description",
                InfoUrl = "https://example.com/info",
                Limits = new NatsOperatorLimits
                {
                    Subs = 1000,
                    Data = 10_000_000,
                    Payload = 1024,
                    Imports = 100,
                    Exports = 100,
                    WildcardExports = true,
                    DisallowBearer = false,
                    Conn = 1000,
                    LeafNodeConn = 100,
                    Streams = 10,
                    Consumer = 100,
                    MemoryStorage = 1_000_000,
                    DiskStorage = 10_000_000,
                    MaxAckPending = 1000,
                    MemoryMaxStreamBytes = 1_000_000,
                    DiskMaxStreamBytes = 10_000_000,
                    MaxBytesRequired = true,
                },
                SigningKeys =
                    new List<string>
                    {
                        "SKAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                        "SKBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB",
                    },
                Imports = new List<NatsImport>
                {
                    new NatsImport
                    {
                        Name = "TestImport",
                        Subject = "import.>",
                        Account = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
                        LocalSubject = "local.import.>",
                        Type = 1,
                        Share = true,
                        AllowTrace = true,
                    },
                },
                Exports = new List<NatsExport>
                {
                    new NatsExport
                    {
                        Name = "TestExport",
                        Subject = "export.>",
                        Type = 1,
                        TokenReq = true,
                        Revocations =
                            new Dictionary<string, long>
                            {
                                { "RKAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", 1609459200 },
                            },
                        ResponseType = "Singleton",
                        ResponseThreshold = TimeSpan.FromSeconds(1),
                        AccountTokenPosition = 1,
                        Advertise = true,
                        AllowTrace = true,
                    },
                },
                Mappings =
                    new Dictionary<string, NatsWeightedMapping>
                    {
                        {
                            "test",
                            new NatsWeightedMapping { Subject = "test.>", Weight = 100, Cluster = "test_cluster" }
                        },
                    },
                DefaultPermissions =
                    new NatsPermissions
                    {
                        Pub = new NatsPermission
                        {
                            Allow = new List<string> { "allowed.>", }, Deny = new List<string> { "denied.>", },
                        },
                        Sub =
                            new NatsPermission
                            {
                                Allow = new List<string> { "allowed.>", },
                                Deny = new List<string> { "denied.>", },
                            },
                        Resp = new NatsResponsePermission { MaxMsgs = 100, Expires = TimeSpan.FromSeconds(60), },
                    },
                Revocations =
                    new Dictionary<string, long> { { "RKAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", 1609459200 } },
                Tags = new List<string> { "tag1", "tag2", },
                Authorization = new NatsExternalAuthorization
                {
                    AuthUsers = new List<string> { "user1", "user2", },
                    AllowedAccounts = new List<string> { "acc1", "acc2", },
                    XKey = "XKAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                },
                Trace = new NatsMsgTrace { Destination = "trace.subject", Sampling = "100", },
            },
        };

        string json = JsonSerializer.Serialize(claims);
        var deserialized = JsonSerializer.Deserialize<NatsAccountClaims>(json);

        Assert.Equal(claims.Subject, deserialized.Subject);
        Assert.Equal(claims.Issuer, deserialized.Issuer);
        Assert.Equal(claims.Name, deserialized.Name);
        Assert.Equal(claims.Audience, deserialized.Audience);
        Assert.Equal(claims.Expires, deserialized.Expires);
        Assert.Equal(claims.IssuedAt, deserialized.IssuedAt);
        Assert.Equal(claims.NotBefore, deserialized.NotBefore);
        Assert.Equal(claims.Id, deserialized.Id);

        Assert.NotNull(deserialized.Account);
        Assert.Equal(claims.Account.Type, deserialized.Account.Type);
        Assert.Equal(claims.Account.Version, deserialized.Account.Version);
        Assert.Equal(claims.Account.Description, deserialized.Account.Description);
        Assert.Equal(claims.Account.InfoUrl, deserialized.Account.InfoUrl);

        Assert.Equal(claims.Account.Limits.Subs, deserialized.Account.Limits.Subs);
        Assert.Equal(claims.Account.Limits.Data, deserialized.Account.Limits.Data);
        Assert.Equal(claims.Account.Limits.Payload, deserialized.Account.Limits.Payload);
        Assert.Equal(claims.Account.Limits.Imports, deserialized.Account.Limits.Imports);
        Assert.Equal(claims.Account.Limits.Exports, deserialized.Account.Limits.Exports);
        Assert.Equal(claims.Account.Limits.WildcardExports, deserialized.Account.Limits.WildcardExports);
        Assert.Equal(claims.Account.Limits.DisallowBearer, deserialized.Account.Limits.DisallowBearer);
        Assert.Equal(claims.Account.Limits.Conn, deserialized.Account.Limits.Conn);
        Assert.Equal(claims.Account.Limits.LeafNodeConn, deserialized.Account.Limits.LeafNodeConn);
        Assert.Equal(claims.Account.Limits.Streams, deserialized.Account.Limits.Streams);
        Assert.Equal(claims.Account.Limits.Consumer, deserialized.Account.Limits.Consumer);
        Assert.Equal(claims.Account.Limits.MemoryStorage, deserialized.Account.Limits.MemoryStorage);
        Assert.Equal(claims.Account.Limits.DiskStorage, deserialized.Account.Limits.DiskStorage);
        Assert.Equal(claims.Account.Limits.MaxAckPending, deserialized.Account.Limits.MaxAckPending);
        Assert.Equal(claims.Account.Limits.MemoryMaxStreamBytes, deserialized.Account.Limits.MemoryMaxStreamBytes);
        Assert.Equal(claims.Account.Limits.DiskMaxStreamBytes, deserialized.Account.Limits.DiskMaxStreamBytes);
        Assert.Equal(claims.Account.Limits.MaxBytesRequired, deserialized.Account.Limits.MaxBytesRequired);

        Assert.Equal(claims.Account.SigningKeys, deserialized.Account.SigningKeys);

        Assert.Single(deserialized.Account.Imports);
        Assert.Equal(claims.Account.Imports[0].Name, deserialized.Account.Imports[0].Name);
        Assert.Equal(claims.Account.Imports[0].Subject, deserialized.Account.Imports[0].Subject);
        Assert.Equal(claims.Account.Imports[0].Account, deserialized.Account.Imports[0].Account);
        Assert.Equal(claims.Account.Imports[0].LocalSubject, deserialized.Account.Imports[0].LocalSubject);
        Assert.Equal(claims.Account.Imports[0].Type, deserialized.Account.Imports[0].Type);
        Assert.Equal(claims.Account.Imports[0].Share, deserialized.Account.Imports[0].Share);
        Assert.Equal(claims.Account.Imports[0].AllowTrace, deserialized.Account.Imports[0].AllowTrace);

        Assert.Single(deserialized.Account.Exports);
        Assert.Equal(claims.Account.Exports[0].Name, deserialized.Account.Exports[0].Name);
        Assert.Equal(claims.Account.Exports[0].Subject, deserialized.Account.Exports[0].Subject);
        Assert.Equal(claims.Account.Exports[0].Type, deserialized.Account.Exports[0].Type);
        Assert.Equal(claims.Account.Exports[0].TokenReq, deserialized.Account.Exports[0].TokenReq);
        Assert.Equal(claims.Account.Exports[0].Revocations, deserialized.Account.Exports[0].Revocations);
        Assert.Equal(claims.Account.Exports[0].ResponseType, deserialized.Account.Exports[0].ResponseType);
        Assert.Equal(claims.Account.Exports[0].ResponseThreshold, deserialized.Account.Exports[0].ResponseThreshold);
        Assert.Equal(claims.Account.Exports[0].AccountTokenPosition, deserialized.Account.Exports[0].AccountTokenPosition);
        Assert.Equal(claims.Account.Exports[0].Advertise, deserialized.Account.Exports[0].Advertise);
        Assert.Equal(claims.Account.Exports[0].AllowTrace, deserialized.Account.Exports[0].AllowTrace);

        Assert.Single(deserialized.Account.Mappings);
        Assert.Equal(claims.Account.Mappings["test"].Subject, deserialized.Account.Mappings["test"].Subject);
        Assert.Equal(claims.Account.Mappings["test"].Weight, deserialized.Account.Mappings["test"].Weight);
        Assert.Equal(claims.Account.Mappings["test"].Cluster, deserialized.Account.Mappings["test"].Cluster);

        Assert.Equal(claims.Account.DefaultPermissions.Pub.Allow, deserialized.Account.DefaultPermissions.Pub.Allow);
        Assert.Equal(claims.Account.DefaultPermissions.Pub.Deny, deserialized.Account.DefaultPermissions.Pub.Deny);
        Assert.Equal(claims.Account.DefaultPermissions.Sub.Allow, deserialized.Account.DefaultPermissions.Sub.Allow);
        Assert.Equal(claims.Account.DefaultPermissions.Sub.Deny, deserialized.Account.DefaultPermissions.Sub.Deny);
        Assert.Equal(claims.Account.DefaultPermissions.Resp.MaxMsgs, deserialized.Account.DefaultPermissions.Resp.MaxMsgs);
        Assert.Equal(claims.Account.DefaultPermissions.Resp.Expires, deserialized.Account.DefaultPermissions.Resp.Expires);

        Assert.Equal(claims.Account.Revocations, deserialized.Account.Revocations);
        Assert.Equal(claims.Account.Tags, deserialized.Account.Tags);

        Assert.Equal(claims.Account.Authorization.AuthUsers, deserialized.Account.Authorization.AuthUsers);
        Assert.Equal(claims.Account.Authorization.AllowedAccounts, deserialized.Account.Authorization.AllowedAccounts);
        Assert.Equal(claims.Account.Authorization.XKey, deserialized.Account.Authorization.XKey);

        Assert.Equal(claims.Account.Trace.Destination, deserialized.Account.Trace.Destination);
        Assert.Equal(claims.Account.Trace.Sampling, deserialized.Account.Trace.Sampling);
    }

    [Fact]
    public void SerializeDeserialize_MinimalNatsAccountClaims_ShouldSucceed()
    {
        var claims = new NatsAccountClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            Account = new NatsAccount(),
        };

        string json = JsonSerializer.Serialize(claims);
        var deserialized = JsonSerializer.Deserialize<NatsAccountClaims>(json);

        Assert.Equal(claims.Subject, deserialized.Subject);
        Assert.Equal(claims.Issuer, deserialized.Issuer);
        Assert.NotNull(deserialized.Account);
    }

    [Fact]
    public void Deserialize_ExtraFields_ShouldIgnore()
    {
        string json = """
                      {
                          "sub": "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                          "iss": "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
                          "nats": {},
                          "extra_field": "should be ignored"
                      }
                      """;

        var deserialized = JsonSerializer.Deserialize<NatsAccountClaims>(json);

        Assert.Equal("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", deserialized.Subject);
        Assert.Equal("IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII", deserialized.Issuer);
        Assert.NotNull(deserialized.Account);
    }

    [Fact]
    public void Serialize_ShouldOmitDefaultValues()
    {
        var claims = new NatsAccountClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            Account = new NatsAccount(),
        };

        string json = JsonSerializer.Serialize(claims);

        Assert.Contains("\"sub\":\"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\"", json);
        Assert.Contains("\"iss\":\"IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII\"", json);
        Assert.DoesNotContain("\"aud\"", json);
        Assert.DoesNotContain("\"exp\"", json);
        Assert.DoesNotContain("\"iat\"", json);
        Assert.DoesNotContain("\"nbf\"", json);
        Assert.DoesNotContain("\"jti\"", json);
    }
}
