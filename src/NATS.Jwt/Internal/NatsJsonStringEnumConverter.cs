// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NATS.Jwt.Models;

namespace NATS.Jwt.Internal;

/// <inheritdoc />
internal class NatsJsonStringEnumConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    /// <inheritdoc />
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new InvalidOperationException();
        }

        var stringValue = reader.GetString();

        if (typeToConvert == typeof(NatsExportType))
        {
            switch (stringValue)
            {
                case "unknown":
                    return (TEnum)(object)NatsExportType.Unknown;
                case "stream":
                    return (TEnum)(object)NatsExportType.Stream;
                case "service":
                    return (TEnum)(object)NatsExportType.Service;
            }
        }

        throw new InvalidOperationException($"Reading unknown enum type {typeToConvert.Name} or value {stringValue}");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        if (value is NatsExportType consumerConfigDeliverPolicy)
        {
            switch (consumerConfigDeliverPolicy)
            {
                case NatsExportType.Unknown:
                    writer.WriteStringValue("unknown");
                    return;
                case NatsExportType.Stream:
                    writer.WriteStringValue("stream");
                    return;
                case NatsExportType.Service:
                    writer.WriteStringValue("service");
                    return;
            }
        }

        throw new InvalidOperationException($"Writing unknown enum value {value.GetType().Name}.{value}");
    }
}
