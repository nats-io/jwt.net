namespace NATS.Jwt.Internal
{
    internal abstract class JsonSerializable
    {
        public abstract JSONNode ToJsonNode();

        public virtual byte[] Serialize()
        {
            return JsonUtils.Serialize(ToJsonNode());
        }

        public virtual string ToJsonString()
        {
            return ToJsonNode().ToString();
        }
    }
}