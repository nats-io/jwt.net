using System;
using System.Text.Json.Serialization;
using NATS.Jwt.Internal;

namespace NATS.Jwt.Models
{
    public record NatsResponsePermission
    {
        [JsonPropertyName("max")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int MaxMsgs { get; set; }

        [JsonPropertyName("ttl")]
        [JsonConverter(typeof(NatsJsJsonNanosecondsConverter))]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TimeSpan Expires { get; set; }
    }
}
