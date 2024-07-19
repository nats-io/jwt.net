using System.Text.Json.Serialization;

namespace NATS.Jwt.Models
{
    public record JwtClaimsData
    {
        [JsonPropertyName("aud")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Audience { get; set; }

        [JsonPropertyName("jti")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Id { get; set; }

        [JsonPropertyName("iat")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public long IssuedAt { get; set; }

        [JsonPropertyName("iss")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Issuer { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }

        [JsonPropertyName("sub")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Subject { get; set; }

        [JsonPropertyName("exp")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public long Expires { get; set; }

        [JsonPropertyName("nbf")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public long NotBefore { get; set; }
    }
}
