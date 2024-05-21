using System;
using System.Security.Cryptography;
using System.Text;
using NATS.Jwt.Internal;
using NATS.NKeys;

namespace NATS.Jwt
{
    public static class JwtUtils
    {
        private static readonly string EncodedClaimHeader =
            EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes("{\"typ\":\"JWT\", \"alg\":\"ed25519-nkey\"}"));

        internal static readonly long NoLimit = -1;

        /// <summary>
        /// Format string with `%s` placeholder for the JWT token followed
        /// by the user NKey seed. This can be directly used as such:
        /// <pre>
        /// NKey userKey = NKey.createUser(new SecureRandom());
        /// NKey signingKey = loadFromSecretStore();
        /// string jwt = IssueUserJWT(signingKey, accountId, userKey.EncodedPublicKey);
        /// string.format(JwtUtils.NatsUserJwtFormat, jwt, userKey.EncodedSeed);
        /// </pre>
        /// </summary>
        public static readonly string NatsUserJwtFormat =
            "-----BEGIN NATS USER JWT-----\n" +
            "{0}\n" +
            "------END NATS USER JWT------\n" +
            "\n" +
            "************************* IMPORTANT *************************\n" +
            "NKEY Seed printed below can be used to sign and prove identity.\n" +
            "NKEYs are sensitive and should be treated as secrets.\n" +
            "\n" +
            "-----BEGIN USER NKEY SEED-----\n" +
            "{1}\n" +
            "------END USER NKEY SEED------\n" +
            "\n" +
            "*************************************************************\n";

        private static long UnixTimeSeconds()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        /// <summary>
        /// Issue a user JWT from a scoped signing key. See <a href="https://docs.nats.io/nats-tools/nsc/signing_keys">Signing Keys</a>
        /// </summary>
        /// <param name="signingKey">a mandatory account nkey pair to sign the generated jwt.</param>
        /// <param name="accountId">a mandatory public account nkey. Will throw error when not set or not account nkey.</param>
        /// <param name="publicUserKey">a mandatory public user nkey. Will throw error when not set or not user nkey.</param>
        /// <returns>a JWT</returns>
        public static string IssueUserJwt(KeyPair signingKey, string accountId, string publicUserKey)
        {
            return IssueUserJwt(signingKey, publicUserKey, null, null, UnixTimeSeconds(), null, new UserClaim { IssuerAccount = accountId });
        }

        /// <summary>
        /// Issue a user JWT from a scoped signing key. See <a href="https://docs.nats.io/nats-tools/nsc/signing_keys">Signing Keys</a>
        /// </summary>
        /// <param name="signingKey">a mandatory account nkey pair to sign the generated jwt.</param>
        /// <param name="accountId">a mandatory public account nkey. Will throw error when not set or not account nkey.</param>
        /// <param name="publicUserKey">a mandatory public user nkey. Will throw error when not set or not user nkey.</param>
        /// <param name="name">optional human-readable name. When absent, default to publicUserKey.</param>
        /// <returns>a JWT</returns>
        public static string IssueUserJwt(KeyPair signingKey, string accountId, string publicUserKey, string name)
        {
            return IssueUserJwt(signingKey, publicUserKey, name, null, UnixTimeSeconds(), null, new UserClaim { IssuerAccount = accountId });
        }

        /// <summary>
        /// Issue a user JWT from a scoped signing key. See <a href="https://docs.nats.io/nats-tools/nsc/signing_keys">Signing Keys</a>
        /// </summary>
        /// <param name="signingKey">a mandatory account nkey pair to sign the generated jwt.</param>
        /// <param name="accountId">a mandatory public account nkey. Will throw error when not set or not account nkey.</param>
        /// <param name="publicUserKey">a mandatory public user nkey. Will throw error when not set or not user nkey.</param>
        /// <param name="name">optional human-readable name. When absent, default to publicUserKey.</param>
        /// <param name="expiration">optional but recommended duration, when the generated jwt needs to expire. If not set, JWT will not expire.</param>
        /// <param name="tags">optional list of tags to be included in the JWT.</param>
        /// <returns>a JWT</returns>
        public static string IssueUserJwt(KeyPair signingKey, string accountId, string publicUserKey, string name, TimeSpan? expiration, params string[] tags)
        {
            return IssueUserJwt(signingKey, publicUserKey, name, expiration, UnixTimeSeconds(), null, new UserClaim { IssuerAccount = accountId, Tags = tags });
        }

        /// <summary>
        /// Issue a user JWT from a scoped signing key. See <a href="https://docs.nats.io/nats-tools/nsc/signing_keys">Signing Keys</a>
        /// </summary>
        /// <param name="signingKey">a mandatory account nkey pair to sign the generated jwt.</param>
        /// <param name="accountId">a mandatory public account nkey. Will throw error when not set or not account nkey.</param>
        /// <param name="publicUserKey">a mandatory public user nkey. Will throw error when not set or not user nkey.</param>
        /// <param name="name">optional human-readable name. When absent, default to publicUserKey.</param>
        /// <param name="expiration">optional but recommended duration, when the generated jwt needs to expire. If not set, JWT will not expire.</param>
        /// <param name="tags">optional list of tags to be included in the JWT.</param>
        /// <param name="issuedAt">the current epoch seconds.</param>
        /// <returns>a JWT</returns>
        public static string IssueUserJwt(KeyPair signingKey, string accountId, string publicUserKey, string name, TimeSpan? expiration, string[] tags, long issuedAt)
        {
            return IssueUserJwt(signingKey, publicUserKey, name, expiration, issuedAt, null, new UserClaim {IssuerAccount = accountId, Tags = tags });
        }

        /// <summary>
        /// Issue a user JWT from a scoped signing key. See <a href="https://docs.nats.io/nats-tools/nsc/signing_keys">Signing Keys</a>
        /// </summary>
        /// <param name="signingKey">a mandatory account nkey pair to sign the generated jwt.</param>
        /// <param name="accountId">a mandatory public account nkey. Will throw error when not set or not account nkey.</param>
        /// <param name="publicUserKey">a mandatory public user nkey. Will throw error when not set or not user nkey.</param>
        /// <param name="name">optional human-readable name. When absent, default to publicUserKey.</param>
        /// <param name="expiration">optional but recommended duration, when the generated jwt needs to expire. If not set, JWT will not expire.</param>
        /// <param name="tags">optional list of tags to be included in the JWT.</param>
        /// <param name="issuedAt">the current epoch seconds.</param>
        /// <param name="audience">optional audience</param>
        /// <returns>a JWT</returns>
        public static string IssueUserJwt(KeyPair signingKey, string accountId, string publicUserKey, string name, TimeSpan? expiration, string[] tags, long issuedAt, string audience)
        {
            return IssueUserJwt(signingKey, publicUserKey, name, expiration, issuedAt, audience, new UserClaim{IssuerAccount = accountId, Tags = tags});
        }

        /// <summary>
        /// Issue a user JWT from a scoped signing key. See <a href="https://docs.nats.io/nats-tools/nsc/signing_keys">Signing Keys</a>
        /// </summary>
        /// <param name="signingKey">a mandatory account nkey pair to sign the generated jwt.</param>
        /// <param name="publicUserKey">a mandatory public user nkey. Will throw error when not set or not user nkey.</param>
        /// <param name="name">optional human-readable name. When absent, default to publicUserKey.</param>
        /// <param name="duration">optional but recommended duration, when the generated jwt needs to expire. If not set, JWT will not expire.</param>
        /// <param name="issuedAt">the current epoch seconds.</param>
        /// <param name="audience">optional audience</param>
        /// <param name="userClaim">the user claim</param>
        /// <returns>a JWT</returns>
        public static string IssueUserJwt(
            KeyPair signingKey,
            string publicUserKey,
            string name,
            TimeSpan? duration,
            long issuedAt,
            string audience,
            UserClaim userClaim)
        {
            var durationJson = duration?.ToDurationJson();
            var userClaimJson = userClaim.ToUserClaimJson();
            
            // Validate the signingKey:
            if (!signingKey.GetPublicKey().StartsWith("A"))
            {
                throw new ArgumentException(
                    "IssueUserJWT requires an account key for the signingKey parameter, but got " + signingKey.GetPublicKey()[0]);
            }
            
            // Validate the accountId:
            KeyPair accountKey = KeyPair.FromPublicKey(userClaimJson.IssuerAccount.ToCharArray());
            if (!accountKey.GetPublicKey().StartsWith("A"))
            {
                throw new ArgumentException(
                    "IssueUserJWT requires an account key for the accountId parameter, but got " + accountKey.GetPublicKey()[0]);
            }
            
            // Validate the publicUserKey:
            KeyPair userKey = KeyPair.FromPublicKey(publicUserKey.ToCharArray());
            // if (userKey.Type != NKeys.NKeys.PrefixByte.User)
            if (!userKey.GetPublicKey().StartsWith("U"))
            {
                throw new ArgumentException("IssueUserJWT requires a user key for the publicUserKey parameter, but got " + userKey.GetPublicKey()[0]);
            }

            string accSigningKeyPub = signingKey.GetPublicKey();

            string claimName = string.IsNullOrWhiteSpace(name) ? publicUserKey : name;

            return IssueJwt(signingKey, publicUserKey, claimName, durationJson, issuedAt, accSigningKeyPub, audience, userClaimJson);
        }

        /// <summary>
        /// Issue a JWT
        /// </summary>
        /// <param name="signingKey">account nkey pair to sign the generated jwt.</param>
        /// <param name="publicUserKey">a mandatory public user nkey.</param>
        /// <param name="name">optional human-readable name.</param>
        /// <param name="expirationJson">optional but recommended duration, when the generated jwt needs to expire. If not set, JWT will not expire.</param>
        /// <param name="issuedAt">the current epoch seconds.</param>
        /// <param name="accSigningKeyPub">the account signing key</param>
        /// <param name="audience">optional audience</param>
        /// <param name="nats">the generic nats claim</param>
        /// <returns>a JWT</returns>
        private static string IssueJwt(KeyPair signingKey, string publicUserKey, string name, DurationJson expirationJson,
            long issuedAt, string accSigningKeyPub, string audience, JsonSerializable nats)
        {
            var claim = new Claim
            {
                Aud = audience,
                Iat = issuedAt,
                Iss = accSigningKeyPub,
                Name = name,
                Sub = publicUserKey,
                Exp = expirationJson,
                Nats = nats
            };

            // Issue At time is stored in unix seconds
            string claimJson = claim.ToJsonString();

            // Compute jti, a base32 encoded sha256 hash
            IncrementalHash hasher = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
            hasher.AppendData(Encoding.ASCII.GetBytes(claimJson));

            claim.Jti = Base32Encode(hasher.GetHashAndReset());

            // all three components (header/body/signature) are base64url encoded
            string encBody = EncodingUtils.ToBase64UrlEncoded(claim.Serialize());

            // compute the signature off of header + body (. included on purpose)
            byte[] sig = Encoding.ASCII.GetBytes(EncodedClaimHeader + "." + encBody);
            var signature = new byte[64];
            signingKey.Sign(sig, signature);
            string encSig = EncodingUtils.ToBase64UrlEncoded(signature);

            // append signature to header and body and return it
            return EncodedClaimHeader + "." + encBody + "." + encSig;
        }

        private static string Base32Encode(byte[] bytes)
        {
            var encodedLength = Base32.GetEncodedLength(bytes);
            var chars = new char[encodedLength];
            Base32.ToBase32(bytes, chars);
            return new string(chars);
        }

        /// <summary>
        /// Get the claim body from a JWT
        /// </summary>
        /// <param name="jwt">the encoded jwt</param>
        /// <returns>the claim body json</returns>
        public static string GetClaimBody(string jwt)
        {
            return EncodingUtils.FromBase64UrlEncoded(jwt.Split('.')[1]);
        }
    }
}