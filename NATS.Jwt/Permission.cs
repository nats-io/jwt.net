using NATS.Jwt.Internal;

namespace NATS.Jwt
{
    public class Permission
    {
        internal PermissionJson ToPermissionJson()
        {
            return new PermissionJson { Allow = Allow, Deny = Deny };
        }

        public string[] Deny { get; set; }

        public string[] Allow { get; set; }
    }
}