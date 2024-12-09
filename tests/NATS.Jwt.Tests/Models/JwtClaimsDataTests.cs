// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NATS.Jwt.Internal;
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
            IssuedAt = DateTimeOffset.FromUnixTimeSeconds(1609459200), // 2021-01-01
            Issuer = "test_issuer",
            Name = "Test JWT",
            Subject = "test_subject",
            Expires = DateTimeOffset.FromUnixTimeSeconds(1735689600), // 2025-01-01
            NotBefore = DateTimeOffset.FromUnixTimeSeconds(1609459200), // 2021-01-01
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
        Assert.Null(deserialized.IssuedAt);
        Assert.Null(deserialized.Name);
        Assert.Null(deserialized.Expires);
        Assert.Null(deserialized.NotBefore);
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
        Assert.Null(deserialized.IssuedAt);
        Assert.Null(deserialized.Name);
        Assert.Null(deserialized.Expires);
        Assert.Null(deserialized.NotBefore);
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

    [Fact]
    public void Deserialize_WithNullValues_ShouldSetPropertiesToNull()
    {
        // Arrange
        var json = "{}";

        // Act
        var claims = JsonSerializer.Deserialize<JwtClaimsData>(json);

        // Assert
        Assert.Null(claims.Audience);
        Assert.Null(claims.Id);
        Assert.Null(claims.IssuedAt);
        Assert.Null(claims.Issuer);
        Assert.Null(claims.Name);
        Assert.Null(claims.Subject);
        Assert.Null(claims.Expires);
        Assert.Null(claims.NotBefore);
    }

    [Fact]
    public void Serialize_WithNullValues_ShouldOmitNullProperties()
    {
        // Arrange
        var claims = new JwtClaimsData();

        // Act
        var json = JsonSerializer.Serialize(claims);

        // Assert
        Assert.Equal("{}", json);
    }

    [Fact]
    public void Deserialize_WithUnixTimestamps_ShouldConvertCorrectly()
    {
        // Arrange
        var json = @"{
                ""iat"": 1609459200,
                ""exp"": 1609545600,
                ""nbf"": 1609372800
            }";

        // Act
        var claims = JsonSerializer.Deserialize<JwtClaimsData>(json);

        // Assert
        Assert.Equal(new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero), claims.IssuedAt);
        Assert.Equal(new DateTimeOffset(2021, 1, 2, 0, 0, 0, TimeSpan.Zero), claims.Expires);
        Assert.Equal(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), claims.NotBefore);
    }

    [Fact]
    public void NatsJsonDateTimeOffsetConverter_Read_ValidNumber_ShouldConvertCorrectly()
    {
        // Arrange
        var json = @"{""Timestamp"": 1609459200}";
        var options = new JsonSerializerOptions
        {
            Converters = { new NatsJsonDateTimeOffsetConverter() }
        };

        // Act
        var result = JsonSerializer.Deserialize<TestClass>(json, options);

        // Assert
        Assert.Equal(new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero), result.Timestamp);
    }

    [Fact]
    public void NatsJsonDateTimeOffsetConverter_Read_NullValue_ShouldReturnNull()
    {
        // Arrange
        var json = @"{""Timestamp"": null}";
        var options = new JsonSerializerOptions
        {
            Converters = { new NatsJsonDateTimeOffsetConverter() }
        };

        // Act
        var result = JsonSerializer.Deserialize<TestClass>(json, options);

        // Assert
        Assert.Null(result.Timestamp);
    }

    [Fact]
    public void NatsJsonDateTimeOffsetConverter_Read_InvalidTokenType_ShouldThrowException()
    {
        // Arrange
        var json = @"{""Timestamp"": ""not a number""}";
        var options = new JsonSerializerOptions
        {
            Converters = { new NatsJsonDateTimeOffsetConverter() },
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => JsonSerializer.Deserialize<TestClass>(json, options));
    }

    [Fact]
    public void NatsJsonDateTimeOffsetConverter_Write_ValidValue_ShouldWriteNumber()
    {
        // Arrange
        var testObject = new TestClass
        {
            Timestamp = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero)
        };
        var options = new JsonSerializerOptions
        {
            Converters = { new NatsJsonDateTimeOffsetConverter() }
        };

        // Act
        var json = JsonSerializer.Serialize(testObject, options);

        // Assert
        Assert.Equal(@"{""Timestamp"":1609459200}", json);
    }

    [Fact]
    public void NatsJsonDateTimeOffsetConverter_Write_NullValue_ShouldWriteNull()
    {
        // Arrange
        var testObject = new TestClass
        {
            Timestamp = null
        };
        var options = new JsonSerializerOptions
        {
            Converters = { new NatsJsonDateTimeOffsetConverter() }
        };

        // Act
        var json = JsonSerializer.Serialize(testObject, options);

        // Assert
        Assert.Equal(@"{""Timestamp"":null}", json);
    }

    private class TestClass
    {
        [JsonConverter(typeof(NatsJsonDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }
    }
}
