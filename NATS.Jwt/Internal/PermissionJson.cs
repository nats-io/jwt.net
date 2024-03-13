namespace NATS.Jwt.Internal
{
    internal class PermissionJson : JsonSerializable {
        public string[] Allow;
        public string[] Deny;
    
        public override JSONNode ToJsonNode() {
            JSONObject o = new JSONObject();
            JsonUtils.AddField(o, "allow", Allow);
            JsonUtils.AddField(o, "deny", Deny);
            return o;
        }
    }
}