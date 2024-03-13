using System.Collections.Generic;

namespace NATS.Jwt.Internal
{
    internal class UserClaimJson : JsonSerializable {
        public string IssuerAccount;            // User
        public string[] Tags;                   // User/GenericFields
        public string Type = "user";            // User/GenericFields
        public int Version = 2;                 // User/GenericFields
        public PermissionJson Pub;                  // User/UserPermissionLimits/Permissions
        public PermissionJson Sub;                  // User/UserPermissionLimits/Permissions
        public ResponsePermissionJson Resp;         // User/UserPermissionLimits/Permissions
        public string[] Src;                    // User/UserPermissionLimits/Limits/UserLimits
        public IList<TimeRangeJson> Times;          // User/UserPermissionLimits/Limits/UserLimits
        public string Locale;                   // User/UserPermissionLimits/Limits/UserLimits
        public long Subs = JwtUtils.NoLimit;    // User/UserPermissionLimits/Limits/NatsLimits
        public long Data = JwtUtils.NoLimit;    // User/UserPermissionLimits/Limits/NatsLimits
        public long Payload = JwtUtils.NoLimit; // User/UserPermissionLimits/Limits/NatsLimits
        public bool BearerToken;                // User/UserPermissionLimits
        public string[] AllowedConnectionTypes; // User/UserPermissionLimits
    
        public override JSONNode ToJsonNode() {
            JSONObject o = new JSONObject();
            JsonUtils.AddField(o, "issuer_account", IssuerAccount);
            JsonUtils.AddField(o, "tags", Tags);
            JsonUtils.AddField(o, "type", Type);
            JsonUtils.AddField(o, "version", Version);
            JsonUtils.AddField(o, "pub", Pub);
            JsonUtils.AddField(o, "sub", Sub);
            JsonUtils.AddField(o, "resp", Resp);
            JsonUtils.AddField(o, "src", Src);
            JsonUtils.AddField(o, "times", Times);
            JsonUtils.AddField(o, "times_location", Locale);
            JsonUtils.AddFieldWhenGteMinusOne(o, "subs", Subs);
            JsonUtils.AddFieldWhenGteMinusOne(o, "data", Data);
            JsonUtils.AddFieldWhenGteMinusOne(o, "payload", Payload);
            JsonUtils.AddField(o, "bearer_token", BearerToken);
            JsonUtils.AddField(o, "allowed_connection_types", AllowedConnectionTypes);
            return o;
        }
    }
}