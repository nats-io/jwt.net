using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using NATS.Jwt.Internal;
using NATS.Jwt.Models;
using NATS.NKeys;

namespace NATS.Jwt;

#pragma warning disable SA1303
#pragma warning disable SA1202
#pragma warning disable SA1204

public class NatsJwt
{
    public static readonly JwtHeader NatsJwtHeader = new() { Type = TokenTypeJwt, Algorithm = AlgorithmNkey };

    private const int LibraryVersion = 2;

    public const long NoLimit = -1;

    public const string AnyAccount = "*";

    // OperatorClaim is the type of an operator JWT
    public const string OperatorClaim = "operator";

    // AccountClaim is the type of an Account JWT
    public const string AccountClaim = "account";

    // UserClaim is the type of an user JWT
    public const string UserClaim = "user";

    // ActivationClaim is the type of an activation JWT
    public const string ActivationClaim = "activation";

    // AuthorizationRequestClaim is the type of an auth request claim JWT
    public const string AuthorizationRequestClaim = "authorization_request";

    // AuthorizationResponseClaim is the response for an auth request
    public const string AuthorizationResponseClaim = "authorization_response";

    // GenericClaim is a type that doesn't match Operator/Account/User/ActionClaim
    public const string GenericClaim = "generic";

    // TokenTypeJwt is the JWT token type supported JWT tokens
    // encoded and decoded by this library
    // from RFC7519 5.1 "typ":
    // it is RECOMMENDED that "JWT" always be spelled using uppercase characters for compatibility
    public const string TokenTypeJwt = "JWT";

    // AlgorithmNkey is the algorithm supported by JWT tokens
    // encoded and decoded by this library
    private const string AlgorithmNkeyOld = "ed25519";

    public const string AlgorithmNkey = AlgorithmNkeyOld + "-nkey";

    public string FormatUserConfig(string jwt, string seed)
    {
        // TODO: Decode JWT and validate
        var parts = jwt.Split('.');
        var json = EncodingUtils.FromBase64UrlEncoded(parts[1]);
        var fields = JsonSerializer.Deserialize(json, JsonContext.Default.NatsGenericFieldsClaims);
        var type = fields!.GenericFields.Type;
        if (type != UserClaim)
        {
            throw new NatsJwtException($"{type} can't be serialized as a user config");
        }

        var jwtKind = type.ToUpperInvariant();

        var seedKind = seed.StartsWith("SU", StringComparison.Ordinal) ? "USER"
            : seed.StartsWith("SA", StringComparison.Ordinal) ? "ACCOUNT"
            : seed.StartsWith("SO", StringComparison.Ordinal) ? "OPERATOR"
            : throw new NatsJwtException("Seed is not an operator, account or user seed");

        return $"""
                -----BEGIN NATS {jwtKind} JWT-----
                {jwt}
                ------END NATS {jwtKind} JWT------

                ************************* IMPORTANT *************************
                NKEY Seed printed below can be used to sign and prove identity.
                NKEYs are sensitive and should be treated as secrets.

                -----BEGIN {seedKind} NKEY SEED-----
                {seed}
                ------END {seedKind} NKEY SEED------

                *************************************************************
                """;
    }

    public NatsActivationClaims NewActivationClaims(string subject) => new() { Subject = subject };

    public NatsAuthorizationRequestClaims NewAuthorizationRequestClaims(string subject) => new() { Subject = subject };

    public NatsAuthorizationResponseClaims NewAuthorizationResponseClaims(string subject) => new() { Subject = subject };

    public NatsGenericClaims NewGenericClaims(string subject) => new() { Subject = subject };

    public NatsOperatorClaims NewOperatorClaims(string subject) => new() { Subject = subject, Issuer = subject };

    public NatsUserClaims NewUserClaims(string subject) => new() { Subject = subject };

    public NatsAccountClaims NewAccountClaims(string subject) => new() { Subject = subject };

    /********************************************************************************************/

    public string EncodeActivationClaims(NatsActivationClaims activationClaims, KeyPair keyPair, DateTimeOffset? issuedAt = null)
    {
        SetVersion(activationClaims.Activation, ActivationClaim);
        return DoEncode(NatsJwtHeader, keyPair, activationClaims, JsonContext.Default.NatsActivationClaims, issuedAt);
    }

    public string EncodeAuthorizationRequestClaims(NatsAuthorizationRequestClaims authorizationRequestClaims, KeyPair keyPair, DateTimeOffset? issuedAt = null)
    {
        SetVersion(authorizationRequestClaims.AuthorizationRequest, AuthorizationRequestClaim);
        return DoEncode(NatsJwtHeader, keyPair, authorizationRequestClaims, JsonContext.Default.NatsAuthorizationRequestClaims, issuedAt);
    }

    public string EncodeAuthorizationResponseClaims(NatsAuthorizationResponseClaims authorizationResponseClaims, KeyPair keyPair, DateTimeOffset? issuedAt = null)
    {
        SetVersion(authorizationResponseClaims.AuthorizationResponse, AuthorizationRequestClaim);
        return DoEncode(NatsJwtHeader, keyPair, authorizationResponseClaims, JsonContext.Default.NatsAuthorizationResponseClaims, issuedAt);
    }

    public string EncodeGenericClaims(NatsGenericClaims genericClaims, KeyPair keyPair, DateTimeOffset? issuedAt = null)
    {
        return DoEncode(NatsJwtHeader, keyPair, genericClaims, JsonContext.Default.NatsGenericClaims, issuedAt);
    }

    public string EncodeOperatorClaims(NatsOperatorClaims operatorClaims, KeyPair keyPair, DateTimeOffset? issuedAt = null)
    {
        SetVersion(operatorClaims.Operator, OperatorClaim);
        return DoEncode(NatsJwtHeader, keyPair, operatorClaims, JsonContext.Default.NatsOperatorClaims, issuedAt);
    }

    public string EncodeUserClaims(NatsUserClaims userClaims, KeyPair keyPair, DateTimeOffset? issuedAt = null)
    {
        SetVersion(userClaims.User, UserClaim);
        return DoEncode(NatsJwtHeader, keyPair, userClaims, JsonContext.Default.NatsUserClaims, issuedAt);
    }

    public string EncodeAccountClaims(NatsAccountClaims accountClaims, KeyPair keyPair, DateTimeOffset? issuedAt = null)
    {
        SetVersion(accountClaims.Account, AccountClaim);
        accountClaims.Account.Imports?.Sort();
        accountClaims.Account.Exports?.Sort();
        return DoEncode(NatsJwtHeader, keyPair, accountClaims, JsonContext.Default.NatsAccountClaims, issuedAt);
    }

    /********************************************************************************************/

    private void SetVersion<T>(T claims, string type)
        where T : NatsGenericFields
    {
        claims.Type = type;
        claims.Version = LibraryVersion;
    }

    private string DoEncode<T>(JwtHeader jwtHeader, KeyPair keyPair, T claim, JsonTypeInfo<T> typeInfo, DateTimeOffset? now)
        where T : JwtClaimsData
    {
        using var writer = new NatsBufferWriter<byte>();

        var issuedAt = now ?? DateTimeOffset.UtcNow;

        var h = Serialize(jwtHeader, JsonContext.Default.JwtHeader);
        var issuerBytes = keyPair.GetPublicKey();

        // TODO: Validate prefixes
        var c = claim;

        c.Issuer = issuerBytes;
        c.IssuedAt = issuedAt.ToUnixTimeSeconds();

        // TODO: ID generation same as Go implementation
        // c.Id = Hash(c, typeInfo);
        c.Id = Hash(c, JsonContext.Default.JwtClaimsData);

        var payload = Serialize(c, typeInfo);

        // TODO: Check algorithm, only allow ed25519
        var toSign = $"{h}.{payload}";
        var sig = Encoding.ASCII.GetBytes(toSign);
        var signature = new byte[64];
        keyPair.Sign(sig, signature);
        var eSig = EncodingUtils.ToBase64UrlEncoded(signature);

        return $"{toSign}.{eSig}";
    }

    private string Hash<T>(T c, JsonTypeInfo<T> typeInfo)
    {
        using var writer = new NatsBufferWriter<byte>();
        var jsonWriter = new Utf8JsonWriter(writer);
        JsonSerializer.Serialize(jsonWriter, c, typeInfo);
        var bytes = writer.WrittenMemory.ToArray();

        // TODO: ID generation same as Go implementation
        // It's just an ID so we can use SHA-256
        // var hasher = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        // hasher.AppendData(bytes);
        // var hashResult = hasher.GetHashAndReset();
        var hashResult = Sha512256.ComputeHash(bytes);

        Span<char> hashResultChars = stackalloc char[Base32.GetEncodedLength(hashResult)];
        Base32.ToBase32(hashResult, hashResultChars);
        return hashResultChars.ToString();
    }

    private static string Serialize<T>(T data, JsonTypeInfo<T> typeInfo)
    {
        using var writer = new NatsBufferWriter<byte>();
        var jsonWriter = new Utf8JsonWriter(writer);
        JsonSerializer.Serialize(jsonWriter, data, typeInfo);
#if DEBUG
        var bytes = writer.WrittenMemory.ToArray();
        var json = Encoding.UTF8.GetString(bytes);
#endif
        return EncodingUtils.ToBase64UrlEncoded(writer.WrittenMemory.ToArray());
    }
}

public class NatsJwtException(string message) : Exception(message);
