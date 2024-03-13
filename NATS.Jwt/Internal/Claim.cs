namespace NATS.Jwt.Internal
{
    internal class Claim : JsonSerializable {
        public string Aud;
        public string Jti;
        public long Iat;
        public string Iss;
        public string Name;
        public string Sub;
        public DurationJson Exp;
        public JsonSerializable Nats;
    
        public override JSONNode ToJsonNode() {
            JSONObject o = new JSONObject();

            JsonUtils.AddField(o, "aud", Aud);
            JsonUtils.AddFieldEvenEmpty(o, "jti", Jti);
            JsonUtils.AddField(o, "iat", Iat);
            JsonUtils.AddField(o, "iss", Iss);
            JsonUtils.AddField(o, "name", Name);
            JsonUtils.AddField(o, "sub", Sub);
            
            if (Exp != null && !Exp.IsZero() && !Exp.IsNegative()) {
                long seconds = Exp.Millis / 1000;
                JsonUtils.AddField(o, "exp", Iat + seconds);  // relative to the iat
            }
            
            JsonUtils.AddField(o, "nats", Nats);
            return o;
        }
    }
}