using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Internal;

internal class NatsJSJsonNanosecondsConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
        {
            throw new InvalidOperationException("Expected number");
        }

        var value = reader.GetInt64();

        return TimeSpan.FromMilliseconds(value / 1_000_000.0);
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options) =>
        writer.WriteNumberValue((long)(value.TotalMilliseconds * 1_000_000L));
}

internal class NatsJSJsonNullableNanosecondsConverter : JsonConverter<TimeSpan?>
{
    private readonly NatsJSJsonNanosecondsConverter _converter = new();

    public override TimeSpan? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        return _converter.Read(ref reader, typeToConvert, options);
    }

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
