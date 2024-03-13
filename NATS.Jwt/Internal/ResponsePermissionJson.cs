namespace NATS.Jwt.Internal
{
    internal class ResponsePermissionJson : JsonSerializable {
        public int MaxMsgs;
        public DurationJson Expires;
    
        public override JSONNode ToJsonNode() {
            JSONObject o = new JSONObject();
            JsonUtils.AddField(o, "max", MaxMsgs);
            JsonUtils.AsDuration(o, "ttl", Expires);
            return o;
        }
    }
}