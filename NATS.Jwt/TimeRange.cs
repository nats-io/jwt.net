using System.Text.Json.Serialization;

namespace NATS.Jwt
{
    public class TimeRange
    {
        [JsonPropertyName("start")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Start { get; set; }

        [JsonPropertyName("end")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string End { get; set; }
    }
}
