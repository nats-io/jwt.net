using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record NatsAuthorizationResponseClaims : JwtClaimsData
{
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public NatsAuthorizationResponse AuthorizationResponse { get; set; } = new();
}

public record NatsAuthorizationResponse : NatsGenericFields
{
    [JsonPropertyName("jwt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string Jwt { get; set; }

    [JsonPropertyName("error")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string Error { get; set; }

    /// <summary>
    /// IssuerAccount stores the public key for the account the issuer represents.
    /// When set, the claim was issued by a signing key.
    /// </summary>
    [JsonPropertyName("issuer_account")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string IssuerAccount { get; set; }
}
