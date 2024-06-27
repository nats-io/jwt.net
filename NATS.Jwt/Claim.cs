using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace NATS.Jwt
{
    public class Claim
    {
        [JsonPropertyName("aud")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Aud { get; set; }

        [JsonPropertyName("jti")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Jti { get; set; }

        [JsonPropertyName("iat")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public long Iat { get; set; }

        [JsonPropertyName("iss")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Iss { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }

        [JsonPropertyName("sub")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Sub { get; set; }

        [JsonPropertyName("exp")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public long Exp { get; set; }

        [JsonPropertyName("nats")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public JsonNode Nats { get; set; }
    }
}
