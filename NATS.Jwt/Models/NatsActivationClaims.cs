using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record NatsActivationClaims : JwtClaimsData
{
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public NatsActivation Activation { get; set; } = new();
}

public record NatsActivation : NatsGenericFields
{
    [JsonPropertyName("subject")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string ImportSubject { get; set; }

    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public int ImportType { get; set; }

    /// <summary>
    /// IssuerAccount stores the public key for the account the issuer represents.
    /// When set, the claim was issued by a signing key.
    /// </summary>
    [JsonPropertyName("issuer_account")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string IssuerAccount { get; set; }
}
