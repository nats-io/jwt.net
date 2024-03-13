using System;
using NATS.Jwt.Internal;

namespace NATS.Jwt
{
    public class ResponsePermission
    {
        internal ResponsePermissionJson ToResponsePermissionJson()
        {
            return new ResponsePermissionJson
            {
                Expires = Expires.ToDurationJson(),
                MaxMsgs = MaxMsgs,
            };
        }

        public int MaxMsgs { get; set; }

        public TimeSpan Expires { get; set; }
    }
}