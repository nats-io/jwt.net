using System.Text.Json;
using System.Text.Json.Serialization;

namespace NATS.Jwt
{
    public class UserClaim
    {
        [JsonPropertyName("issuer_account")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string IssuerAccount { get; set; }

        [JsonPropertyName("tags")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string[] Tags { get; set; }

        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Type { get; set; } = "user";

        [JsonPropertyName("version")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Version { get; set; } = 2;

        [JsonPropertyName("pub")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Permission Pub { get; set; }

        [JsonPropertyName("sub")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Permission Sub { get; set; }

        [JsonPropertyName("resp")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ResponsePermission Resp { get; set; }

        [JsonPropertyName("src")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string[] Src { get; set; }

        [JsonPropertyName("times")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TimeRange[] Times { get; set; }

        [JsonPropertyName("times_location")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Locale { get; set; }

        [JsonPropertyName("subs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public long Subs { get; set; } = -1;

        [JsonPropertyName("data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public long Data { get; set; } = -1;

        [JsonPropertyName("payload")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public long Payload { get; set; } = -1;

        [JsonPropertyName("bearer_token")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool BearerToken { get; set; }

        [JsonPropertyName("allowed_connection_types")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string[] AllowedConnectionTypes { get; set; }
    }
}
