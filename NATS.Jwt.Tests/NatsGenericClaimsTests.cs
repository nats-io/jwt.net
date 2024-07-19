using System;
using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using JsonDiffPatchDotNet;
using NATS.Jwt.Internal;
using NATS.Jwt.Models;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace NATS.Jwt.Tests;

public class NatsGenericClaimsTests
{
    private readonly ITestOutputHelper _output;

    public NatsGenericClaimsTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Generic_claims_can_serialize_data()
    {
        var claims = new NatsGenericClaims
        {
            Subject = "1234567890",
            Data = new()
            {
                ["user"] = new JsonObject
                {
                    ["name"] = "John Doe",
                    ["email"] = "john@example.com",
                    ["roles"] = new JsonArray { "admin", "user" },
                },
            },
        };
        var writer = new SimpleBufferWriter(1024);
        var jsonWriter = new Utf8JsonWriter(writer);
        JsonSerializer.Serialize(jsonWriter, claims, JsonContext.Default.NatsGenericClaims);
        var json = Encoding.ASCII.GetString(writer.WrittenMemory.ToArray());

        _output.WriteLine($"json: {json}");

        var jsonReader = new Utf8JsonReader(writer.WrittenMemory.Span);
        var claims2 = JsonSerializer.Deserialize(ref jsonReader, JsonContext.Default.NatsGenericClaims);
        _output.WriteLine($"claims2: {claims2}");

        var json2 = JsonSerializer.Serialize(claims2);
        _output.WriteLine($"json2: {json2}");

        var jdp = new JsonDiffPatch();
        var left = JToken.Parse(json);
        var right = JToken.Parse(json2);
        var patch = jdp.Diff(left, right);
        _output.WriteLine($"patch: {patch}");
        Assert.Null(patch);
    }
}

public class SimpleBufferWriter : IBufferWriter<byte>
{
    private byte[] _buffer;
    private int _index;

    public SimpleBufferWriter(int initialCapacity)
    {
        _buffer = new byte[initialCapacity];
        _index = 0;
    }

    public ReadOnlyMemory<byte> WrittenMemory => _buffer.AsMemory(0, _index);

    public void Advance(int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        _index += count;
    }

    public Memory<byte> GetMemory(int sizeHint = 0)
    {
        EnsureCapacity(sizeHint);
        return _buffer.AsMemory(_index);
    }

    public Span<byte> GetSpan(int sizeHint = 0)
    {
        EnsureCapacity(sizeHint);
        return _buffer.AsSpan(_index);
    }

    private void EnsureCapacity(int sizeHint)
    {
        if (sizeHint < 0)
            sizeHint = 0;

        var availableSpace = _buffer.Length - _index;

        if (sizeHint <= availableSpace)
            return;
        var growBy = Math.Max(sizeHint, _buffer.Length);
        Array.Resize(ref _buffer, checked(_buffer.Length + growBy));
    }
}
