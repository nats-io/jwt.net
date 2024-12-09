// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Internal;

/// <summary>
/// Converts a TimeSpan value to and from its JSON representation in nanoseconds.
/// </summary>
internal class NatsJsJsonNanosecondsConverter : JsonConverter<TimeSpan>
{
    /// <summary>
    /// Converts a JSON representation in nanoseconds to a TimeSpan value.
    /// </summary>
    /// <param name="reader">The Utf8JsonReader used to read the JSON data.</param>
    /// <param name="typeToConvert">The type of the object to be converted.</param>
    /// <param name="options">The JsonSerializerOptions to be used during serialization.</param>
    /// <returns>The TimeSpan value represented by the JSON data.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the JSON data is not in the expected format.</exception>
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
        {
            throw new InvalidOperationException("Expected number");
        }

        var value = reader.GetInt64();

        return TimeSpan.FromMilliseconds(value / 1_000_000.0);
    }

    /// <summary>
    /// Writes a TimeSpan value as its JSON representation in nanoseconds.
    /// </summary>
    /// <param name="writer">The Utf8JsonWriter used to write the JSON data.</param>
    /// <param name="value">The TimeSpan value to be written.</param>
    /// <param name="options">The JsonSerializerOptions to be used during serialization.</param>
    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options) =>
        writer.WriteNumberValue((long)(value.TotalMilliseconds * 1_000_000L));
}
