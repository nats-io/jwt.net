using System;
using System.Text.Json.Serialization;

namespace NATS.Jwt
{
    public class ResponsePermission
    {
        [JsonPropertyName("max")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int MaxMsgs { get; set; }

        [JsonPropertyName("ttl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TimeSpan Expires { get; set; }
    }
}
