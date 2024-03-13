namespace NATS.Jwt.Internal
{
    internal class TimeRangeJson : JsonSerializable {
        public string Start;
        public string End;

        public TimeRangeJson()
        {
        }

        public TimeRangeJson(string start, string end) {
            Start = start;
            End = end;
        }
    
        public override JSONNode ToJsonNode() {
            JSONObject o = new JSONObject();
            JsonUtils.AddField(o, "start", Start);
            JsonUtils.AddField(o, "end", End);
            return o;
        }
    }
}