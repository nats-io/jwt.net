// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using NATS.Jwt.Models;
using NATS.NKeys;

namespace NATS.Jwt;

/// <summary>
/// Provides extension methods for validating JSON Web Token (JWT) models.
/// </summary>
public static class ModelValidationExtensions
{
    private static readonly Dictionary<Type, PrefixByte[]> ExpectedClaimsPrefixes = new()
    {
        { typeof(NatsAccountClaims), [PrefixByte.Account, PrefixByte.Operator] },
        { typeof(NatsActivationClaims), [PrefixByte.Account, PrefixByte.Operator] },
        { typeof(NatsAuthorizationRequestClaims), [PrefixByte.Server] },
        { typeof(NatsAuthorizationResponseClaims), [PrefixByte.Account] },
        { typeof(NatsGenericClaims), [] },
        { typeof(NatsOperatorClaims), [PrefixByte.Operator] },
        { typeof(NatsUserClaims), [PrefixByte.Account] },
    };

    /// <summary>
    /// Determines the expected prefix bytes for the given JWT claims data.
    /// </summary>
    /// <param name="claims">The JWT claims data whose expected prefixes need to be determined.</param>
    /// <returns>An array of PrefixByte values indicating the expected prefixes for the specified claims.</returns>
    /// <exception cref="NatsJwtException">Thrown if the expected prefixes cannot be determined for the given claims type.</exception>
    public static PrefixByte[] ExpectedPrefixes(this JwtClaimsData claims)
    {
        if (!ExpectedClaimsPrefixes.TryGetValue(claims.GetType(), out var prefixes))
        {
            throw new NatsJwtException($"Can't find prefixes for {claims.GetType().Name}");
        }

        return prefixes;
    }

    /// <summary>
    /// Validates the specified JWT header to ensure its type and algorithm are supported.
    /// </summary>
    /// <param name="header">The JWT header to validate.</param>
    /// <exception cref="NatsJwtException">Invalid JWT header.</exception>
    public static void Validate(this JwtHeader header)
    {
        if (!string.Equals("JWT", header.Type, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new NatsJwtException($"Invalid JWT header: not supported type {header.Type}");
        }

        if (!string.Equals("ed25519", header.Algorithm, StringComparison.InvariantCultureIgnoreCase)
            && !string.Equals(NatsJwt.AlgorithmNkey, header.Algorithm, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new NatsJwtException($"Invalid JWT header: unexpected {header.Algorithm} algorithm");
        }
    }
}
