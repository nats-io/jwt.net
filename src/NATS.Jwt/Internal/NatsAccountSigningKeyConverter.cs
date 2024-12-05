// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using NATS.Jwt.Models;

namespace NATS.Jwt.Internal
{
    /// <summary>
    /// .
    /// </summary>
    internal class NatsAccountSigningKeyConverter : JsonConverter<List<NatsAccountSigningKey>>
    {
        /// <summary>
        /// Converts a JSON representation of SigningKeys to their correct type.
        /// </summary>
        /// <param name="reader">The Utf8JsonReader used to read the JSON data.</param>
        /// <param name="typeToConvert">The type of the object to be converted.</param>
        /// <param name="options">The JsonSerializerOptions to be used during serialization.</param>
        /// <returns>A list of NatsAccountSigningKey.</returns>
        /// <exception cref="JsonException">yeah, this isn't done yet.</exception>
        public override List<NatsAccountSigningKey>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return default;
            }
            else if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Expected Null or Array");
            }

            List<NatsAccountSigningKey> results = [];

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return results;
                }

                if (reader.TokenType == JsonTokenType.String)
                {
                    string? simpleSigningKey = reader.GetString();
                    if (simpleSigningKey != null)
                    {
                        results.Add(simpleSigningKey);
                    }
                }
                else if (reader.TokenType == JsonTokenType.StartObject)
                {
                    NatsAccountScopedSigningKey? scopedSigningKey = JsonSerializer.Deserialize(ref reader, JsonContext.Default.NatsAccountScopedSigningKey);
                    if (scopedSigningKey != null)
                    {
                        results.Add(scopedSigningKey);
                    }
                }
                else
                {
                    throw new JsonException();
                }
            }

            throw new JsonException();
        }

        /// <summary>
        /// Writes the List of SigningKeys to its JSON representation.
        /// </summary>
        /// <param name="writer">The Utf8JsonWriter used to write the JSON data.</param>
        /// <param name="value">The List of NatsAccountSigningKeys to be written.</param>
        /// <param name="options">The JsonSerializerOptions to be used during serialization.</param>
        public override void Write(Utf8JsonWriter writer, List<NatsAccountSigningKey> value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStartArray();

                foreach (NatsAccountSigningKey sk in value)
                {
                    if (sk.GetType() == typeof(NatsAccountSigningKey))
                    {
                        writer.WriteStringValue((string)sk);
                    }
                    else if (sk.GetType() == typeof(NatsAccountScopedSigningKey))
                    {
                        JsonSerializer.Serialize(writer, (NatsAccountScopedSigningKey)sk, JsonContext.Default.NatsAccountScopedSigningKey);
                    }
                }

                writer.WriteEndArray();
            }
        }
    }
}
