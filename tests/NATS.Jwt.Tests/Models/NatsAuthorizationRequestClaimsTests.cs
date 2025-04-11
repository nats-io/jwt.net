// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Text.Json;
using NATS.Jwt.Models;
using Xunit;

namespace NATS.Jwt.Tests.Models;

public class NatsAuthorizationRequestClaimsTests
{
    [Fact]
    public void SerializeDeserialize_NatsTags_ShouldSucceed()
    {
        var id = new NatsServerId { Tags = new NatsTags { "Tag1", " tAg2", "taG3 ", " tag4 " } };

        string json = JsonSerializer.Serialize(id);
        Assert.Equal("{\"tags\":[\"tag1\",\"tag2\",\"tag3\",\"tag4\"]}", json);

        var deserialized = JsonSerializer.Deserialize<NatsServerId>(json);
        Assert.NotNull(deserialized);
        Assert.Equal(id, deserialized);
    }

    [Fact]
    public void SerializeDeserialize_FullNatsAuthorizationRequestClaims_ShouldSucceed()
    {
        var claims = new NatsAuthorizationRequestClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            Name = "Full Test Authorization Request",
            Audience = "test_audience",
            Expires = DateTimeOffset.FromUnixTimeSeconds(1735689600),
            IssuedAt = DateTimeOffset.FromUnixTimeSeconds(1609459200),
            NotBefore = DateTimeOffset.FromUnixTimeSeconds(1609459200),
            Id = "jti_test",
            AuthorizationRequest = new NatsAuthorizationRequest
            {
                NatsServer = new NatsServerId
                {
                    Name = "Test Server",
                    Host = "test.example.com",
                    Id = "SAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                    Version = "2.2.0",
                    Cluster = "test_cluster",
                    Tags = ["tag1", "tag2"],
                },
                UserNKey = "UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                NatsClientInformation = new NatsClientInformation
                {
                    Host = "client.example.com",
                    Id = 123,
                    User = "test_user",
                    Name = "Test Client",
                    Tags = new NatsTags { "client_tag1", "client_tag2", },
                    NameTag = "client_name_tag",
                    Kind = "client",
                    Type = "test_type",
                    Mqtt = "mqtt_id",
                    Nonce = "test_nonce",
                },
                NatsConnectOptions = new NatsConnectOptions
                {
                    Jwt = "test.jwt.token",
                    NKey = "NKEYAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                    SignedNonce = "signed_nonce",
                    Token = "auth_token",
                    Username = "test_username",
                    Password = "test_password",
                    Name = "Test Connection",
                    Lang = "csharp",
                    Version = "1.0.0",
                    Protocol = 1,
                },
                Tls = new NatsClientTls
                {
                    Version = "TLSv1.3",
                    Cipher = "TLS_AES_256_GCM_SHA384",
                    Certs = new List<string> { "cert1", "cert2", },
                    VerifiedChains = new List<List<string>>
                    {
                        new List<string> { "chain1_cert1", "chain1_cert2", },
                        new List<string> { "chain2_cert1", "chain2_cert2", },
                    },
                },
                RequestNonce = "request_nonce",
                Type = "authorization_request",
                Version = 2,
                Tags = new NatsTags { "auth_tag1", "auth_tag2", },
            },
        };

        string json = JsonSerializer.Serialize(claims);
        var deserialized = JsonSerializer.Deserialize<NatsAuthorizationRequestClaims>(json);

        Assert.Equal(claims.Subject, deserialized.Subject);
        Assert.Equal(claims.Issuer, deserialized.Issuer);
        Assert.Equal(claims.Name, deserialized.Name);
        Assert.Equal(claims.Audience, deserialized.Audience);
        Assert.Equal(claims.Expires, deserialized.Expires);
        Assert.Equal(claims.IssuedAt, deserialized.IssuedAt);
        Assert.Equal(claims.NotBefore, deserialized.NotBefore);
        Assert.Equal(claims.Id, deserialized.Id);

        Assert.NotNull(deserialized.AuthorizationRequest);
        Assert.Equal(claims.AuthorizationRequest.NatsServer.Name, deserialized.AuthorizationRequest.NatsServer.Name);
        Assert.Equal(claims.AuthorizationRequest.NatsServer.Host, deserialized.AuthorizationRequest.NatsServer.Host);
        Assert.Equal(claims.AuthorizationRequest.NatsServer.Id, deserialized.AuthorizationRequest.NatsServer.Id);
        Assert.Equal(claims.AuthorizationRequest.NatsServer.Version, deserialized.AuthorizationRequest.NatsServer.Version);
        Assert.Equal(claims.AuthorizationRequest.NatsServer.Cluster, deserialized.AuthorizationRequest.NatsServer.Cluster);
        Assert.Equal(claims.AuthorizationRequest.NatsServer.Tags, deserialized.AuthorizationRequest.NatsServer.Tags);

        Assert.Equal(claims.AuthorizationRequest.UserNKey, deserialized.AuthorizationRequest.UserNKey);

        Assert.Equal(claims.AuthorizationRequest.NatsClientInformation.Host, deserialized.AuthorizationRequest.NatsClientInformation.Host);
        Assert.Equal(claims.AuthorizationRequest.NatsClientInformation.Id, deserialized.AuthorizationRequest.NatsClientInformation.Id);
        Assert.Equal(claims.AuthorizationRequest.NatsClientInformation.User, deserialized.AuthorizationRequest.NatsClientInformation.User);
        Assert.Equal(claims.AuthorizationRequest.NatsClientInformation.Name, deserialized.AuthorizationRequest.NatsClientInformation.Name);
        Assert.Equal(claims.AuthorizationRequest.NatsClientInformation.Tags, deserialized.AuthorizationRequest.NatsClientInformation.Tags);
        Assert.Equal(claims.AuthorizationRequest.NatsClientInformation.NameTag, deserialized.AuthorizationRequest.NatsClientInformation.NameTag);
        Assert.Equal(claims.AuthorizationRequest.NatsClientInformation.Kind, deserialized.AuthorizationRequest.NatsClientInformation.Kind);
        Assert.Equal(claims.AuthorizationRequest.NatsClientInformation.Type, deserialized.AuthorizationRequest.NatsClientInformation.Type);
        Assert.Equal(claims.AuthorizationRequest.NatsClientInformation.Mqtt, deserialized.AuthorizationRequest.NatsClientInformation.Mqtt);
        Assert.Equal(claims.AuthorizationRequest.NatsClientInformation.Nonce, deserialized.AuthorizationRequest.NatsClientInformation.Nonce);

        Assert.Equal(claims.AuthorizationRequest.NatsConnectOptions.Jwt, deserialized.AuthorizationRequest.NatsConnectOptions.Jwt);
        Assert.Equal(claims.AuthorizationRequest.NatsConnectOptions.NKey, deserialized.AuthorizationRequest.NatsConnectOptions.NKey);
        Assert.Equal(claims.AuthorizationRequest.NatsConnectOptions.SignedNonce, deserialized.AuthorizationRequest.NatsConnectOptions.SignedNonce);
        Assert.Equal(claims.AuthorizationRequest.NatsConnectOptions.Token, deserialized.AuthorizationRequest.NatsConnectOptions.Token);
        Assert.Equal(claims.AuthorizationRequest.NatsConnectOptions.Username, deserialized.AuthorizationRequest.NatsConnectOptions.Username);
        Assert.Equal(claims.AuthorizationRequest.NatsConnectOptions.Password, deserialized.AuthorizationRequest.NatsConnectOptions.Password);
        Assert.Equal(claims.AuthorizationRequest.NatsConnectOptions.Name, deserialized.AuthorizationRequest.NatsConnectOptions.Name);
        Assert.Equal(claims.AuthorizationRequest.NatsConnectOptions.Lang, deserialized.AuthorizationRequest.NatsConnectOptions.Lang);
        Assert.Equal(claims.AuthorizationRequest.NatsConnectOptions.Version, deserialized.AuthorizationRequest.NatsConnectOptions.Version);
        Assert.Equal(claims.AuthorizationRequest.NatsConnectOptions.Protocol, deserialized.AuthorizationRequest.NatsConnectOptions.Protocol);

        Assert.Equal(claims.AuthorizationRequest.Tls.Version, deserialized.AuthorizationRequest.Tls.Version);
        Assert.Equal(claims.AuthorizationRequest.Tls.Cipher, deserialized.AuthorizationRequest.Tls.Cipher);
        Assert.Equal(claims.AuthorizationRequest.Tls.Certs, deserialized.AuthorizationRequest.Tls.Certs);
        Assert.Equal(claims.AuthorizationRequest.Tls.VerifiedChains, deserialized.AuthorizationRequest.Tls.VerifiedChains);

        Assert.Equal(claims.AuthorizationRequest.RequestNonce, deserialized.AuthorizationRequest.RequestNonce);
        Assert.Equal(claims.AuthorizationRequest.Type, deserialized.AuthorizationRequest.Type);
        Assert.Equal(claims.AuthorizationRequest.Version, deserialized.AuthorizationRequest.Version);
        Assert.Equal(claims.AuthorizationRequest.Tags, deserialized.AuthorizationRequest.Tags);
    }

    [Fact]
    public void SerializeDeserialize_MinimalNatsAuthorizationRequestClaims_ShouldSucceed()
    {
        var claims = new NatsAuthorizationRequestClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            AuthorizationRequest = new NatsAuthorizationRequest
            {
                NatsServer = new NatsServerId { Id = "SAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", },
                UserNKey = "UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                NatsClientInformation = new NatsClientInformation(),
                NatsConnectOptions = new NatsConnectOptions { Protocol = 1, },
            },
        };

        string json = JsonSerializer.Serialize(claims);
        var deserialized = JsonSerializer.Deserialize<NatsAuthorizationRequestClaims>(json);

        Assert.Equal(claims.Subject, deserialized.Subject);
        Assert.Equal(claims.Issuer, deserialized.Issuer);
        Assert.NotNull(deserialized.AuthorizationRequest);
        Assert.Equal(claims.AuthorizationRequest.NatsServer.Id, deserialized.AuthorizationRequest.NatsServer.Id);
        Assert.Equal(claims.AuthorizationRequest.UserNKey, deserialized.AuthorizationRequest.UserNKey);
        Assert.NotNull(deserialized.AuthorizationRequest.NatsClientInformation);
        Assert.Equal(claims.AuthorizationRequest.NatsConnectOptions.Protocol, deserialized.AuthorizationRequest.NatsConnectOptions.Protocol);
    }

    [Fact]
    public void Deserialize_ExtraFields_ShouldIgnore()
    {
        string json = """
                      {
                          "sub": "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                          "iss": "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
                          "nats": {
                              "server_id": {
                                  "id": "SAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                              },
                              "user_nkey": "UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                              "client_info": {},
                              "connect_opts": {
                                  "protocol": 1
                              }
                          },
                          "extra_field": "should be ignored"
                      }
                      """;

        var deserialized = JsonSerializer.Deserialize<NatsAuthorizationRequestClaims>(json);

        Assert.Equal("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", deserialized.Subject);
        Assert.Equal("IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII", deserialized.Issuer);
        Assert.NotNull(deserialized.AuthorizationRequest);
        Assert.Equal("SAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", deserialized.AuthorizationRequest.NatsServer.Id);
        Assert.Equal("UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", deserialized.AuthorizationRequest.UserNKey);
        Assert.NotNull(deserialized.AuthorizationRequest.NatsClientInformation);
        Assert.Equal(1, deserialized.AuthorizationRequest.NatsConnectOptions.Protocol);
    }

    [Fact]
    public void Serialize_ShouldOmitDefaultValues()
    {
        var claims = new NatsAuthorizationRequestClaims
        {
            Subject = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            Issuer = "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII",
            AuthorizationRequest = new NatsAuthorizationRequest
            {
                NatsServer = new NatsServerId { Id = "SAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", },
                UserNKey = "UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                NatsClientInformation = new NatsClientInformation(),
                NatsConnectOptions = new NatsConnectOptions { Protocol = 1, },
            },
        };

        string json = JsonSerializer.Serialize(claims);

        Assert.Contains("\"sub\":\"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\"", json);
        Assert.Contains("\"iss\":\"IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII\"", json);
        Assert.Contains("\"nats\":{", json);
        Assert.Contains("\"server_id\":{\"id\":\"SAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\"}", json);
        Assert.Contains("\"user_nkey\":\"UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\"", json);
        Assert.Contains("\"client_info\":{}", json);
        Assert.Contains("\"connect_opts\":{\"protocol\":1}", json);
        Assert.DoesNotContain("\"aud\"", json);
        Assert.DoesNotContain("\"exp\"", json);
        Assert.DoesNotContain("\"iat\"", json);
        Assert.DoesNotContain("\"nbf\"", json);
        Assert.DoesNotContain("\"jti\"", json);
    }
}
