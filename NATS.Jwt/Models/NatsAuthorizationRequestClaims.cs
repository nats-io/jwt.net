using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record NatsAuthorizationRequestClaims : JwtClaimsData
{
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public NatsAuthorizationRequest AuthorizationRequest { get; set; } = new();
}

public record NatsAuthorizationRequest : NatsGenericFields
{
    [JsonPropertyName("server_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public NatsServerId NatsServer { get; set; } = new();

    [JsonPropertyName("user_nkey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string UserNKey { get; set; }

    [JsonPropertyName("client_info")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public NatsClientInformation NatsClientInformation { get; set; } = new();

    [JsonPropertyName("connect_opts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public NatsConnectOptions NatsConnectOptions { get; set; } = new();

    [JsonPropertyName("client_tls")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsClientTls Tls { get; set; }

    [JsonPropertyName("request_nonce")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string RequestNonce { get; set; }
}

public record NatsServerId
{
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Name { get; set; }

    [JsonPropertyName("host")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Host { get; set; }

    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Id { get; set; }

    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Version { get; set; }

    [JsonPropertyName("cluster")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Cluster { get; set; }

    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string> Tags { get; set; }

    [JsonPropertyName("xkey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string XKey { get; set; }
}

public record NatsClientInformation
{
    [JsonPropertyName("host")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Host { get; set; }

    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Id { get; set; }

    [JsonPropertyName("user")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string User { get; set; }

    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Name { get; set; }

    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string> Tags { get; set; }

    [JsonPropertyName("name_tag")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string NameTag { get; set; }

    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Kind { get; set; }

    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Type { get; set; }

    [JsonPropertyName("mqtt_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Mqtt { get; set; }

    [JsonPropertyName("nonce")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Nonce { get; set; }
}

public record NatsConnectOptions
{
    [JsonPropertyName("jwt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Jwt { get; set; }

    [JsonPropertyName("nkey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string NKey { get; set; }

    [JsonPropertyName("sig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string SignedNonce { get; set; }

    [JsonPropertyName("auth_token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Token { get; set; }

    [JsonPropertyName("user")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Username { get; set; }

    [JsonPropertyName("pass")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Password { get; set; }

    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Name { get; set; }

    [JsonPropertyName("lang")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Lang { get; set; }

    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Version { get; set; }

    [JsonPropertyName("protocol")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public int Protocol { get; set; }
}

public record NatsClientTls
{
    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Version { get; set; }

    [JsonPropertyName("cipher")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Cipher { get; set; }

    [JsonPropertyName("certs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string> Certs { get; set; }

    [JsonPropertyName("verified_chains")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<List<string>> VerifiedChains { get; set; }
}
