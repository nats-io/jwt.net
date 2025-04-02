﻿using System;
using System.Text;
using NATS.Jwt.Models;
using NATS.NKeys;
using Xunit;
using Xunit.Abstractions;

namespace NATS.Jwt.Tests;

public class ValidationTests(ITestOutputHelper output)
{
    [Fact]
    public void Invalid_jwt_when_its_too_short()
    {
        var jwt = new NatsJwt();
        var exception = Assert.Throws<NatsJwtException>(() => jwt.DecodeClaims<NatsAuthorizationRequestClaims>("123"));
        Assert.Equal("Invalid JWT format", exception.Message);
    }
    
    [Fact]
    public void Invalid_jwt_when_there_are_too_few_dots()
    {
        var jwt = new NatsJwt();
        var exception = Assert.Throws<NatsJwtException>(() => jwt.DecodeClaims<NatsAuthorizationRequestClaims>("12.34"));
        Assert.Equal("Invalid JWT format", exception.Message);
    }

    [Fact]
    public void Invalid_jwt_no_issuer()
    {
        var part1 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes("""{"typ":"JWT","alg":"ed25519-nkey"}"""));
        var part2 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes("""{"nats":{"version":2,"type":"authorization_request"}}"""));
        var part3 = EncodingUtils.ToBase64UrlEncoded([1, 2, 3]);
        var token = $"{part1}.{part2}.{part3}";
        var jwt = new NatsJwt();
        var exception = Assert.Throws<NatsJwtException>(() => jwt.DecodeClaims<NatsAuthorizationRequestClaims>(token));
        Assert.Equal("Invalid JWT: can't find issuer", exception.Message);
    }

    [Fact]
    public void Invalid_jwt_can_not_parse_header()
    {
        var part1 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes("null"));
        var part2 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes("{}"));
        var part3 = EncodingUtils.ToBase64UrlEncoded([1, 2, 3]);
        var token = $"{part1}.{part2}.{part3}";
        var jwt = new NatsJwt();
        var exception = Assert.Throws<NatsJwtException>(() => jwt.DecodeClaims<NatsAuthorizationRequestClaims>(token));
        Assert.Equal("Can't parse JWT header", exception.Message);
    }

    [Fact]
    public void Invalid_jwt_new_version()
    {
        var part1 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes("""{"typ":"JWT","alg":"ed25519-nkey"}"""));
        var part2 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes("""{"nats":{"version":3,"type":"authorization_request"}}"""));
        var part3 = EncodingUtils.ToBase64UrlEncoded([1, 2, 3]);
        var token = $"{part1}.{part2}.{part3}";
        var jwt = new NatsJwt();
        var exception = Assert.Throws<NatsJwtException>(() => jwt.DecodeClaims<NatsAuthorizationRequestClaims>(token));
        Assert.Equal("JWT was generated by a newer version", exception.Message);
    }

    [Fact]
    public void Invalid_jwt_unsupported_claim_type()
    {
        var part1 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes("""{"typ":"JWT","alg":"ed25519-nkey"}"""));
        var part2 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes("""{"nats":{"version":2,"type":"non-existent"}}"""));
        var part3 = EncodingUtils.ToBase64UrlEncoded([1, 2, 3]);
        var token = $"{part1}.{part2}.{part3}";
        var jwt = new NatsJwt();
        var exception = Assert.Throws<NatsJwtException>(() => jwt.DecodeClaims<NatsAuthorizationRequestClaims>(token));
        Assert.Equal("Unsupported claim type non-existent", exception.Message);
    }

    [Fact]
    public void Invalid_jwt_mismatch_claim_type()
    {
        var part1 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes("""{"typ":"JWT","alg":"ed25519-nkey"}"""));
        var part2 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes("""{"nats":{"version":2,"type":"user"}}"""));
        var part3 = EncodingUtils.ToBase64UrlEncoded([1, 2, 3]);
        var token = $"{part1}.{part2}.{part3}";
        var jwt = new NatsJwt();
        var exception = Assert.Throws<NatsJwtException>(() => jwt.DecodeClaims<NatsAuthorizationRequestClaims>(token));
        output.WriteLine($"Error: '{exception.Message}'");
        Assert.Equal("Claim type mismatch: requested NATS.Jwt.Models.NatsAuthorizationRequestClaims but found NATS.Jwt.Models.NatsUserClaims (for user) in JWT", exception.Message);
    }

    [Fact]
    public void Verify_version_1()
    {
        var kp = KeyPair.CreatePair(PrefixByte.Server);
        var part1 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes("""{"typ":"JWT","alg":"ed25519-nkey"}"""));
        var part2 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes($$$"""{"iss":"{{{kp.GetPublicKey()}}}","nats":{"version":1,"type":"authorization_request"}}"""));
        var sig = new byte[64];
        kp.Sign(Encoding.ASCII.GetBytes(part2), sig);
        var part3 = EncodingUtils.ToBase64UrlEncoded(sig);
        var token = $"{part1}.{part2}.{part3}";
        var jwt = new NatsJwt();
        var claims = jwt.DecodeClaims<NatsAuthorizationRequestClaims>(token);
        output.WriteLine($"claims:{claims}");
        Assert.Equal(kp.GetPublicKey(), claims.Issuer);
        Assert.Equal(1, claims.AuthorizationRequest.Version);
    }

    [Fact]
    public void Verify_version_1_from_type()
    {
        var kp = KeyPair.CreatePair(PrefixByte.Server);
        var part1 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes("""{"typ":"JWT","alg":"ed25519-nkey"}"""));
        var part2 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes($$$"""{"type":"authorization_request","iss":"{{{kp.GetPublicKey()}}}","nats":{}}"""));
        var sig = new byte[64];
        kp.Sign(Encoding.ASCII.GetBytes(part2), sig);
        var part3 = EncodingUtils.ToBase64UrlEncoded(sig);
        var token = $"{part1}.{part2}.{part3}";
        var jwt = new NatsJwt();
        var claims = jwt.DecodeClaims<NatsAuthorizationRequestClaims>(token);
        output.WriteLine($"claims:{claims}");
        Assert.Equal(kp.GetPublicKey(), claims.Issuer);
    }

    [Theory]
    [InlineData("""{"type":"","iss":"@@PublicKey@@","X":{}}""", "Failed to get nats element")]
    [InlineData("""{"type":"","iss":"@@PublicKey@@","nats":{"X":2}}""", "Failed to get nats.version element")]
    [InlineData("""{"type":"","iss":"@@PublicKey@@","nats":{"version":2.2}}""", "Failed to get nats.version as integer")]
    [InlineData("""{"type":"","iss":"@@PublicKey@@","nats":{"version":2, "X":"X"}}""", "Failed to get nats.type element")]
    [InlineData("""{"type":"","iss":"@@PublicKey@@","nats":{"version":2, "type":""}}""", "Failed to get nats.type element as non-empty string")]
    [InlineData("""{"type":"","iss":"@@PublicKey@@","nats":{"version":2, "type":" "}}""", "Failed to get nats.type element as non-empty string")]
    public void Verify_version_and_type_check(string json, string error)
    {
        var kp = KeyPair.CreatePair(PrefixByte.Server);
        var part1 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes("""{"typ":"JWT","alg":"ed25519-nkey"}"""));
        var part2 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes(json.Replace("@@PublicKey@@", kp.GetPublicKey())));
        var sig = new byte[64];
        kp.Sign(Encoding.ASCII.GetBytes(part2), sig);
        var part3 = EncodingUtils.ToBase64UrlEncoded(sig);
        var token = $"{part1}.{part2}.{part3}";
        var jwt = new NatsJwt();
        var exception = Assert.Throws<NatsJwtException>(() => jwt.DecodeClaims<NatsAuthorizationRequestClaims>(token));
        Assert.Equal(error, exception.Message);
    }

    [Fact]
    public void Verify_decode_encode_subject_is_not_set()
    {
        var kp = KeyPair.CreatePair(PrefixByte.Server);
        var part1 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes("""{"typ":"JWT","alg":"ed25519-nkey"}"""));
        var part2 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes($$$"""{"iss":"{{{kp.GetPublicKey()}}}","nats":{"version":2,"type":"authorization_request"}}"""));
        var sig = new byte[64];
        kp.Sign(Encoding.ASCII.GetBytes($"{part1}.{part2}"), sig);
        var part3 = EncodingUtils.ToBase64UrlEncoded(sig);
        var token = $"{part1}.{part2}.{part3}";
        var jwt = new NatsJwt();
        var claims = jwt.DecodeClaims<NatsAuthorizationRequestClaims>(token);
        Assert.Equal(kp.GetPublicKey(), claims.Issuer);
        Assert.Equal(2, claims.AuthorizationRequest.Version);

        var exception = Assert.Throws<NatsJwtException>(() => jwt.EncodeAuthorizationRequestClaims(claims, kp, DateTimeOffset.Parse("1970-1-1")));
        output.WriteLine($"Error: '{exception.Message}'");
        Assert.Equal("Subject is not set", exception.Message);
    }

    [Fact]
    public void Verify_decode_encode_invalid_signing_key()
    {
        var kp = KeyPair.CreatePair(PrefixByte.Account);
        var part1 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes("""{"typ":"JWT","alg":"ed25519-nkey"}"""));
        var part2 = EncodingUtils.ToBase64UrlEncoded(Encoding.ASCII.GetBytes($$$"""{"sub":"X","iss":"{{{kp.GetPublicKey()}}}","nats":{"version":2,"type":"authorization_request"}}"""));
        var sig = new byte[64];
        kp.Sign(Encoding.ASCII.GetBytes($"{part1}.{part2}"), sig);
        var part3 = EncodingUtils.ToBase64UrlEncoded(sig);
        var token = $"{part1}.{part2}.{part3}";
        var jwt = new NatsJwt();
        var claims = jwt.DecodeClaims<NatsAuthorizationRequestClaims>(token);
        Assert.Equal(kp.GetPublicKey(), claims.Issuer);
        Assert.Equal(2, claims.AuthorizationRequest.Version);

        var exception = Assert.Throws<NatsJwtException>(() => jwt.EncodeAuthorizationRequestClaims(claims, kp, DateTimeOffset.Parse("1970-1-1")));
        output.WriteLine($"Error: '{exception.Message}'");
        Assert.Equal("Invalid signing key of 'Account': expected one of 'Server'", exception.Message);
    }

    [Theory]
    [InlineData("X", "", "Invalid JWT header: not supported type X")]
    [InlineData("JWT", "X", "Invalid JWT header: unexpected X algorithm")]
    public void Header_validation(string type, string algo, string error)
    {
        var header = new JwtHeader { Type = type, Algorithm = algo };
        var exception = Assert.Throws<NatsJwtException>(() => header.Validate());
        Assert.Equal(error, exception.Message);
    }

    [Fact]
    public void Prefix_validation()
    {
        /*func (a *AccountClaims) ExpectedPrefixes() []nkeys.PrefixByte {
	        return []nkeys.PrefixByte{nkeys.PrefixByteAccount, nkeys.PrefixByteOperator}
        }*/
        Assert.Equal([PrefixByte.Account, PrefixByte.Operator], new NatsAccountClaims().ExpectedPrefixes());

        /*func (a *ActivationClaims) ExpectedPrefixes() []nkeys.PrefixByte {
	        return []nkeys.PrefixByte{nkeys.PrefixByteAccount, nkeys.PrefixByteOperator}
        }*/
        Assert.Equal([PrefixByte.Account, PrefixByte.Operator], new NatsActivationClaims().ExpectedPrefixes());

        /*func (gc *GenericClaims) ExpectedPrefixes() []nkeys.PrefixByte {
	        return nil
        }*/
        Assert.Equal([], new NatsGenericClaims().ExpectedPrefixes());

        /*func (oc *OperatorClaims) ExpectedPrefixes() []nkeys.PrefixByte {
	        return []nkeys.PrefixByte{nkeys.PrefixByteOperator}
        }*/
        Assert.Equal([PrefixByte.Operator], new NatsOperatorClaims().ExpectedPrefixes());

        /*func (u *UserClaims) ExpectedPrefixes() []nkeys.PrefixByte {
	        return []nkeys.PrefixByte{nkeys.PrefixByteAccount}
        }*/
        Assert.Equal([PrefixByte.Account], new NatsUserClaims().ExpectedPrefixes());

        /*func (ac *AuthorizationRequestClaims) ExpectedPrefixes() []nkeys.PrefixByte {
	        return []nkeys.PrefixByte{nkeys.PrefixByteServer}
        }*/
        Assert.Equal([PrefixByte.Server], new NatsAuthorizationRequestClaims().ExpectedPrefixes());

        /*func (ar *AuthorizationResponseClaims) ExpectedPrefixes() []nkeys.PrefixByte {
	        return []nkeys.PrefixByte{nkeys.PrefixByteAccount}
        }*/
        Assert.Equal([PrefixByte.Account], new NatsAuthorizationResponseClaims().ExpectedPrefixes());
    }

    [Fact]
    public void Prefix_validation_non_existent()
    {
        var claims = new JwtClaimsData();
        var exception = Assert.Throws<NatsJwtException>(() => claims.ExpectedPrefixes());
        Assert.Equal("Can't find prefixes for JwtClaimsData", exception.Message);
    }
}