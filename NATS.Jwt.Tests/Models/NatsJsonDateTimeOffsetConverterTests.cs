// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.IO;
using System.Text.Json;
using NATS.Jwt.Internal;
using Xunit;

namespace NATS.Jwt.Tests.Models;

public class NatsJsonDateTimeOffsetConverterTests
{
    [Fact]
    public void Read_UnsupportedType_ShouldThrowException()
    {
        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            // Arrange
            var converter = new TestConverter();
            var reader = new Utf8JsonReader(JsonSerializer.SerializeToUtf8Bytes(123));
            reader.Read(); // Move to the first token
            converter.Read(ref reader, typeof(DateTime), new JsonSerializerOptions());
        });
        Assert.Contains("Reading unknown date type", ex.Message);
    }

    [Fact]
    public void Write_ValidValue_ShouldWriteCorrectly()
    {
        // Arrange
        var converter = new TestConverter();
        MemoryStream memoryStream = new();
        var writer = new Utf8JsonWriter(memoryStream);
        var value = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero);

        // Act
        converter.Write(writer, value, new JsonSerializerOptions());
        writer.Flush();

        // Assert
        var json = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
        Assert.Equal("1609459200", json);
    }

    private class TestConverter : NatsJsonDateTimeOffsetConverter
    {
        public new void Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            base.Read(ref reader, typeToConvert, options);
        }

        public new void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
        {
            base.Write(writer, value, options);
        }
    }
}
