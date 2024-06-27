using System.Text.Json.Serialization;

namespace NATS.Jwt
{
    public class Permission
    {
        [JsonPropertyName("allow")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string[] Allow { get; set; }

        [JsonPropertyName("deny")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string[] Deny { get; set; }
    }
}
