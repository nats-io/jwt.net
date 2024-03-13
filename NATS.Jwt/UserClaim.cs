using System.Linq;
using NATS.Jwt.Internal;

namespace NATS.Jwt
{
    public class UserClaim
    {
        internal UserClaimJson ToUserClaimJson()
        {
            return new UserClaimJson
            {
                Data = Data,
                Payload = Payload,
                Subs = Subs,
                Locale = Locale,
                Pub = Pub?.ToPermissionJson(),
                Resp = Resp?.ToResponsePermissionJson(),
                Src = Src,
                BearerToken = BearerToken,
                IssuerAccount = IssuerAccount,
                AllowedConnectionTypes = AllowedConnectionTypes,
                Sub = Sub?.ToPermissionJson(),
                Tags = Tags,
                Times = Times?.Select(t => t.ToTimeRangeJson()).ToArray(),
                Type = Type,
                Version = Version,
            };
        }

        public int Version { get; set; } = 2;

        public string Type { get; set; } = "user";

        public TimeRange[] Times { get; set; }

        public string[] Tags { get; set; }

        public Permission Sub { get; set; }

        public string[] AllowedConnectionTypes { get; set; }

        public string IssuerAccount { get; set; }

        public bool BearerToken { get; set; }

        public string[] Src { get; set; }

        public ResponsePermission Resp { get; set; }

        public Permission Pub { get; set; }

        public string Locale { get; set; }

        public long Subs { get; set; } = -1;

        public long Payload { get; set; } = -1;

        public long Data { get; set; } = -1;

        public string ToJsonString()
        {
            return ToUserClaimJson().ToJsonString();
        }
    }
}