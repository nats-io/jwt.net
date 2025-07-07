// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;

namespace NATS.Jwt
{
    /// <summary>
    /// Utility class for encoding and decoding strings.
    /// </summary>
    public static class EncodingUtils
    {
        /// <summary>
        /// Converts a byte array to a base64 URL-encoded string.
        /// </summary>
        /// <param name="bytes">The byte array to encode.</param>
        /// <returns>The base64 URL-encoded string.</returns>
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

        /// <summary>
        /// Decodes a base64 URL-encoded string to a regular string.
        /// </summary>
        /// <param name="encodedString">The base64 URL-encoded string to decode.</param>
        /// <returns>The decoded string.</returns>
        public static byte[] FromBase64UrlEncoded(string encodedString)
        {
            string base64 = encodedString.Replace("_", "/").Replace("-", "+");
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}
