// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NATS.Jwt.Models;

namespace NATS.Jwt.Internal;

/// <inheritdoc />
internal class NatsJsonDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
{
    /// <inheritdoc />
    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
        {
            throw new InvalidOperationException("Expected number");
        }

        var numberValue = reader.GetInt64();

        if (typeToConvert == typeof(DateTimeOffset?))
        {
            return DateTimeOffset.FromUnixTimeSeconds(numberValue);
        }

        throw new InvalidOperationException($"Reading unknown date type {typeToConvert.Name} or value {numberValue}");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        if (value is { } dateTimeOffset)
        {
            writer.WriteNumberValue(dateTimeOffset.ToUnixTimeSeconds());
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
