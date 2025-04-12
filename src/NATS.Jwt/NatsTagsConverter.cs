// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using NATS.Jwt.Models;

namespace NATS.Jwt;

/// <summary>
/// A custom JSON converter for the <see cref="NatsTags"/> class.
/// Responsible for serializing and deserializing <see cref="NatsTags"/> objects
/// to and from JSON format.
/// </summary>
internal class NatsTagsConverter : JsonConverter<NatsTags>
{
    /// <inheritdoc/>
    public override NatsTags Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected StartArray token.");
        }

        reader.Read();

        var result = new NatsTags();

        while (reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("Expected String token.");
            }

            string value = reader.GetString()!;
            result.Add(value);

            reader.Read();
        }

        return result;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, NatsTags value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (string? tag in value)
        {
            writer.WriteStringValue(tag);
        }

        writer.WriteEndArray();
    }
}
