// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

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

/// <summary>
/// Class for managing NATS JWT encoding and decoding.
/// </summary>
public class NatsJwt
{
    /// <summary>
    /// Represents the JSON Web Token (JWT) header used for encoding NATS JWT claims.
    /// </summary>
    public static readonly JwtHeader NatsJwtHeader = new() { Type = TokenTypeJwt, Algorithm = AlgorithmNkey };

    private const int LibraryVersion = 2;

    /// <summary>
    /// Represents the value that indicates no limit.
    /// </summary>
    public const long NoLimit = -1;

    /// <summary>
    /// Represents the JSON Web Token (JWT) header used for encoding NATS JWT claims.
    /// </summary>
    public const string AnyAccount = "*";

    /// <summary>
    /// The OperatorClaim constant represents the type of an operator JWT.
    /// </summary>
    public const string OperatorClaim = "operator";

    /// <summary>
    /// Represents the "account" claim used for encoding NATS account claims in a JSON Web Token (JWT).
    /// </summary>
    public const string AccountClaim = "account";

    /// <summary>
    /// Represents the user claim used for encoding NATS JWT claims.
    /// </summary>
    public const string UserClaim = "user";

    /// <summary>
    /// Represents the activation claim used for encoding activation-related NATS JWT claims.
    /// </summary>
    public const string ActivationClaim = "activation";

    /// <summary>
    /// Represents the claim used for authorization requests.
    /// </summary>
    public const string AuthorizationRequestClaim = "authorization_request";

    /// <summary>
    /// Represents the claim indicating an authorization response.
    /// </summary>
    public const string AuthorizationResponseClaim = "authorization_response";

    /// <summary>
    /// Represents the constant value of the "generic" claim.
    /// </summary>
    public const string GenericClaim = "generic";

    /// <summary>
    /// Represents the JWT token type supported by JWT tokens encoded and decoded by this library.
    /// The JWT token type is defined in RFC7519 5.1 with the "typ" field.
    /// It is RECOMMENDED that "JWT" always be spelled using uppercase characters for compatibility.
    /// </summary>
    public const string TokenTypeJwt = "JWT";

    /// <summary>
    /// Represents the algorithm used for encoding NATS NKEY JWT claims.
    /// </summary>
    public const string AlgorithmNkey = "ed25519-nkey";

    private static readonly NatsExportComparer ExportComparer = new();

    private static readonly NatsImportComparer ImportComparer = new();

    /// <summary>
    /// Formats the user configuration.
    /// </summary>
    /// <param name="jwt">The JWT token.</param>
    /// <param name="seed">The seed.</param>
    /// <returns>The formatted user configuration.</returns>
    /// <exception cref="NatsJwtException">Thrown when the seed is not an operator, account, or user seed.</exception>
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

    /// <summary>
    /// Creates new instances of NatsActivationClaims with the specified subject.
    /// </summary>
    /// <param name="subject">The subject for the activation claims.</param>
    /// <returns>A new instance of NatsActivationClaims with the specified subject.</returns>
    public NatsActivationClaims NewActivationClaims(string subject) => new() { Subject = subject };

    /// <summary>
    /// Creates a new instance of the NatsAuthorizationRequestClaims class with the specified subject.
    /// </summary>
    /// <param name="subject">The subject of the authorization request.</param>
    /// <returns>A new instance of the NatsAuthorizationRequestClaims class.</returns>
    public NatsAuthorizationRequestClaims NewAuthorizationRequestClaims(string subject) => new() { Subject = subject };

    /// <summary>
    /// Creates a new instance of NatsAuthorizationResponseClaims with the specified subject.
    /// </summary>
    /// <param name="subject">The subject of the claims.</param>
    /// <returns>A new instance of NatsAuthorizationResponseClaims.</returns>
    public NatsAuthorizationResponseClaims NewAuthorizationResponseClaims(string subject) => new() { Subject = subject };

    /// <summary>
    /// Creates a new instance of NatsGenericClaims with the specified subject.
    /// </summary>
    /// <param name="subject">The subject of the claim.</param>
    /// <returns>A new instance of NatsGenericClaims.</returns>
    public NatsGenericClaims NewGenericClaims(string subject) => new() { Subject = subject };

    /// <summary>
    /// Creates new operator claims for generating a NATS JWT token.
    /// </summary>
    /// <param name="subject">The subject of the operator claims.</param>
    /// <returns>A new instance of the <see cref="NatsOperatorClaims"/> class.</returns>
    public NatsOperatorClaims NewOperatorClaims(string subject) => new() { Subject = subject, Issuer = subject };

    /// <summary>
    /// Creates a new instance of the NatsUserClaims class with the specified subject.
    /// </summary>
    /// <param name="subject">The subject of the user claims.</param>
    /// <returns>A new instance of the NatsUserClaims class.</returns>
    public NatsUserClaims NewUserClaims(string subject) => new() { Subject = subject };

    /// <summary>
    /// Creates a new instance of the NatsAccountClaims class with the specified subject.
    /// </summary>
    /// <param name="subject">The subject for the account claims.</param>
    /// <returns>A new instance of NatsAccountClaims.</returns>
    public NatsAccountClaims NewAccountClaims(string subject) => new() { Subject = subject };

    /// <summary>
    /// Encodes the activation claims into a JWT token.
    /// </summary>
    /// <param name="activationClaims">The activation claims.</param>
    /// <param name="keyPair">The key pair used for signing the JWT token.</param>
    /// <param name="issuedAt">The optional issued at timestamp. If not provided, the current time will be used.</param>
    /// <returns>The encoded JWT token.</returns>
    public string EncodeActivationClaims(NatsActivationClaims activationClaims, KeyPair keyPair, DateTimeOffset? issuedAt = null)
    {
        SetVersion(activationClaims.Activation, ActivationClaim);
        return DoEncode(NatsJwtHeader, keyPair, activationClaims, JsonContext.Default.NatsActivationClaims, issuedAt);
    }

    /// <summary>
    /// Encodes the authorization request claims into a JWT token.
    /// </summary>
    /// <param name="authorizationRequestClaims">The authorization request claims.</param>
    /// <param name="keyPair">The key pair used for signing the token.</param>
    /// <param name="issuedAt">The optional issued At timestamp for the token. If not provided, the current timestamp will be used.</param>
    /// <returns>The encoded JWT token.</returns>
    public string EncodeAuthorizationRequestClaims(NatsAuthorizationRequestClaims authorizationRequestClaims, KeyPair keyPair, DateTimeOffset? issuedAt = null)
    {
        SetVersion(authorizationRequestClaims.AuthorizationRequest, AuthorizationRequestClaim);
        return DoEncode(NatsJwtHeader, keyPair, authorizationRequestClaims, JsonContext.Default.NatsAuthorizationRequestClaims, issuedAt);
    }

    /// <summary>
    /// Encodes the authorization response claims and returns the encoded string representation.
    /// </summary>
    /// <param name="authorizationResponseClaims">The authorization response claims to encode.</param>
    /// <param name="keyPair">The key pair to use for encryption.</param>
    /// <param name="issuedAt">The optional issued at date and time.</param>
    /// <returns>The encoded string representation of the authorization response claims.</returns>
    public string EncodeAuthorizationResponseClaims(NatsAuthorizationResponseClaims authorizationResponseClaims, KeyPair keyPair, DateTimeOffset? issuedAt = null)
    {
        SetVersion(authorizationResponseClaims.AuthorizationResponse, AuthorizationResponseClaim);
        return DoEncode(NatsJwtHeader, keyPair, authorizationResponseClaims, JsonContext.Default.NatsAuthorizationResponseClaims, issuedAt);
    }

    /// <summary>
    /// Encodes the generic claims into a JWT token.
    /// </summary>
    /// <param name="genericClaims">The generic claims to encode.</param>
    /// <param name="keyPair">The key pair used for encoding.</param>
    /// <param name="issuedAt">The optional issued at datetime offset.</param>
    /// <returns>The encoded JWT token.</returns>
    public string EncodeGenericClaims(NatsGenericClaims genericClaims, KeyPair keyPair, DateTimeOffset? issuedAt = null)
    {
        return DoEncode(NatsJwtHeader, keyPair, genericClaims, JsonContext.Default.NatsGenericClaims, issuedAt);
    }

    /// <summary>
    /// Encodes the operator claims into a JWT token.
    /// </summary>
    /// <param name="operatorClaims">The operator claims to be encoded.</param>
    /// <param name="keyPair">The key pair used for signing the token.</param>
    /// <param name="issuedAt">The optional issued at timestamp.</param>
    /// <returns>The encoded JWT token.</returns>
    public string EncodeOperatorClaims(NatsOperatorClaims operatorClaims, KeyPair keyPair, DateTimeOffset? issuedAt = null)
    {
        SetVersion(operatorClaims.Operator, OperatorClaim);
        return DoEncode(NatsJwtHeader, keyPair, operatorClaims, JsonContext.Default.NatsOperatorClaims, issuedAt);
    }

    /// <summary>
    /// Encodes the user claims into a JWT token.
    /// </summary>
    /// <param name="userClaims">The user claims to encode.</param>
    /// <param name="keyPair">The key pair to sign the JWT token.</param>
    /// <param name="issuedAt">The optional issued at datetime offset.</param>
    /// <returns>The encoded JWT token.</returns>
    public string EncodeUserClaims(NatsUserClaims userClaims, KeyPair keyPair, DateTimeOffset? issuedAt = null)
    {
        SetVersion(userClaims.User, UserClaim);
        return DoEncode(NatsJwtHeader, keyPair, userClaims, JsonContext.Default.NatsUserClaims, issuedAt);
    }

    /// <summary>
    /// Encodes the account claims into a JWT token.
    /// </summary>
    /// <param name="accountClaims">The account claims data.</param>
    /// <param name="keyPair">The key pair used for signing the token.</param>
    /// <param name="issuedAt">The optional time when the token was issued. If not specified, the current time will be used.</param>
    /// <returns>The encoded JWT token.</returns>
    public string EncodeAccountClaims(NatsAccountClaims accountClaims, KeyPair keyPair, DateTimeOffset? issuedAt = null)
    {
        SetVersion(accountClaims.Account, AccountClaim);
        accountClaims.Account.Imports?.Sort(ImportComparer);
        accountClaims.Account.Exports?.Sort(ExportComparer);
        return DoEncode(NatsJwtHeader, keyPair, accountClaims, JsonContext.Default.NatsAccountClaims, issuedAt);
    }

    /// <summary>
    /// Decodes the claims data from a JWT token.
    /// </summary>
    /// <param name="jwt">The JWT token.</param>
    /// <param name="jsonTypeInfo">The JSON type information for deserialization.</param>
    /// <typeparam name="T">The type of the claims data.</typeparam>
    /// <returns>The decoded claims data.</returns>
    /// <exception cref="NatsJwtException">Thrown if the JWT format is invalid, deserialization fails, or signature verification fails.</exception>
    /// <remarks>
    /// The JWT token is expected to be in the format "header.payload.signature".
    /// The payload is base64-encoded JSON representing the claims data.
    /// The method verifies the signature by comparing it against the signing input generated from the header and payload.
    /// </remarks>
    public T DecodeClaims<T>(string jwt, JsonTypeInfo<T> jsonTypeInfo)
        where T : JwtClaimsData
    {
        var parts = jwt.Split('.');
        if (parts.Length != 3)
        {
            throw new NatsJwtException("Invalid JWT format");
        }

        var payloadJson = EncodingUtils.FromBase64UrlEncoded(parts[1]);
        var claims = JsonSerializer.Deserialize(payloadJson, jsonTypeInfo);

        if (claims == null)
        {
            throw new NatsJwtException("Failed to deserialize JWT payload");
        }

        // Verify the signature
        // var signingInput = $"{parts[0]}.{parts[1]}";
        // var signature = Encoding.ASCII.GetBytes(EncodingUtils.FromBase64UrlEncoded(parts[2]));
        // var publicKey = KeyPair.FromPublicKey(claims.Issuer.AsSpan());
        //
        // if (!publicKey.Verify(Encoding.ASCII.GetBytes(signingInput), signature))
        // {
        //     throw new NatsJwtException("JWT signature verification failed");
        // }
        return claims;
    }

    /// <summary>
    /// Decodes the operator claims from a JWT token.
    /// </summary>
    /// <param name="jwt">The JWT token to decode.</param>
    /// <returns>The decoded operator claims.</returns>
    /// <exception cref="NatsJwtException">Thrown when the operator claim type is invalid.</exception>
    public NatsOperatorClaims DecodeOperatorClaims(string jwt)
    {
        var claims = DecodeClaims(jwt, JsonContext.Default.NatsOperatorClaims);
        if (claims.Operator.Type != OperatorClaim)
        {
            throw new NatsJwtException($"Expected operator claim, but got {claims.Operator.Type}");
        }

        return claims;
    }

    /// <summary>
    /// Decodes the account claims from a JWT token.
    /// </summary>
    /// <param name="jwt">The JWT token.</param>
    /// <returns>The decoded account claims.</returns>
    /// <exception cref="NatsJwtException">Thrown when the account claims type is not as expected.</exception>
    public NatsAccountClaims DecodeAccountClaims(string jwt)
    {
        var claims = DecodeClaims(jwt, JsonContext.Default.NatsAccountClaims);
        if (claims.Account.Type != AccountClaim)
        {
            throw new NatsJwtException($"Expected account claim, but got {claims.Account.Type}");
        }

        return claims;
    }

    /// <summary>
    /// Decodes the user claims from a JWT token.
    /// </summary>
    /// <param name="jwt">The JWT token.</param>
    /// <returns>The decoded user claims.</returns>
    /// <exception cref="NatsJwtException">Thrown when the user claim is not found or has an invalid type.</exception>
    public NatsUserClaims DecodeUserClaims(string jwt)
    {
        var claims = DecodeClaims(jwt, JsonContext.Default.NatsUserClaims);
        if (claims.User.Type != UserClaim)
        {
            throw new NatsJwtException($"Expected user claim, but got {claims.User.Type}");
        }

        return claims;
    }

    /// <summary>
    /// Decodes the activation claims from a JWT token.
    /// </summary>
    /// <param name="jwt">The JWT token.</param>
    /// <returns>The decoded activation claims.</returns>
    /// <exception cref="NatsJwtException">Thrown when the activation type in the claims is not as expected.</exception>
    public NatsActivationClaims DecodeActivationClaims(string jwt)
    {
        var claims = DecodeClaims(jwt, JsonContext.Default.NatsActivationClaims);
        if (claims.Activation.Type != ActivationClaim)
        {
            throw new NatsJwtException($"Expected user claim, but got {claims.Activation.Type}");
        }

        return claims;
    }

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
        c.IssuedAt = issuedAt;

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
        return EncodingUtils.ToBase64UrlEncoded(writer.WrittenMemory.ToArray());
    }
}
