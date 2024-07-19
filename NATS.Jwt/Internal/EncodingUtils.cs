using System;
using System.Text;

#pragma warning disable

namespace NATS.Jwt.Internal
{
    internal static class EncodingUtils
    {
        public static string ToBase64UrlEncoded(byte[] bytes)
        {
            string s = Convert.ToBase64String(bytes);
            int at = s.IndexOf('=');
            if (at != -1)
            {
                s = s.Substring(0, at);
            }
            return s.Replace("/", "_").Replace("+", "-");
        }

        public static string FromBase64UrlEncoded(string s)
        {
            try
            {
                return Encoding.ASCII.GetString(Convert.FromBase64String(s.Replace("_", "/").Replace("-", "+")));
            }
            catch (System.FormatException)
            {
                // maybe wasn't padded correctly?
                try
                {
                    return Encoding.ASCII.GetString(Convert.FromBase64String(s + "="));
                }
                catch (System.FormatException)
                {
                    // maybe wasn't padded correctly?
                    return Encoding.ASCII.GetString(Convert.FromBase64String(s + "=="));
                }
            }
        }
    }
}
