using System;
using System.Text.Json.Serialization;
using NATS.Jwt.Internal;

namespace NATS.Jwt
{
    public class ResponsePermission
    {
        [JsonPropertyName("max")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int MaxMsgs { get; set; }

        [JsonPropertyName("ttl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(NatsJSJsonNanosecondsConverter))]
        public TimeSpan Expires { get; set; }
    }
}
