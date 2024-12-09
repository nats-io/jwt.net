// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Internal;

/// <summary>
/// Converts a nullable TimeSpan value to and from its JSON representation in nanoseconds.
/// </summary>
internal class NatsJsJsonNullableNanosecondsConverter : JsonConverter<TimeSpan?>
{
    private readonly NatsJsJsonNanosecondsConverter _converter = new();

    /// <summary>
    /// Converts a nullable TimeSpan value to and from its JSON representation in nanoseconds.
    /// </summary>
    /// <param name="reader">The Utf8JsonReader used to read the JSON data.</param>
    /// <param name="typeToConvert">The type of the object to be converted.</param>
    /// <param name="options">The JsonSerializerOptions to be used during serialization.</param>
    /// <returns>The nullable TimeSpan value represented by the JSON data or null if the JSON value is null.</returns>
    public override TimeSpan? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        return _converter.Read(ref reader, typeToConvert, options);
    }

    /// <summary>
    /// Writes a nullable TimeSpan value to and from its JSON representation in nanoseconds.
    /// </summary>
    /// <param name="writer">The Utf8JsonWriter used to write the JSON data.</param>
    /// <param name="value">The nullable TimeSpan value to be written.</param>
    /// <param name="options">The JsonSerializerOptions to be used during serialization.</param>
    public override void Write(Utf8JsonWriter writer, TimeSpan? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            _converter.Write(writer, value.Value, options);
        }
    }
}
