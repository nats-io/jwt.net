using System.Text.Json;
using NATS.Jwt.Models;
using NATS.NKeys;
using Xunit;

namespace NATS.Jwt.Tests;

public class NatsJwtTests
{
    private readonly NatsJwt _natsJwt = new();

    [Fact]
    public void TestEncodeOperatorClaims()
    {
        // Setup
        var okp = KeyPair.CreatePair(PrefixByte.Operator);
        var opk = okp.GetPublicKey();
        var oc = _natsJwt.NewOperatorClaims(opk);
        oc.Name = "O";

        var oskp = KeyPair.CreatePair(PrefixByte.Operator);
        var ospk = oskp.GetPublicKey();

        oc.Operator.SigningKeys = [ospk];

        // Encode the claims
        string jwt = _natsJwt.EncodeOperatorClaims(oc, okp);

        // Verify the JWT
        Assert.NotNull(jwt);
        Assert.NotEmpty(jwt);

        // Decode and verify parts
        var parts = jwt.Split('.');
        Assert.Equal(3, parts.Length);

        // Verify header
        var headerJson = EncodingUtils.FromBase64UrlEncoded(parts[0]);
        var header = JsonSerializer.Deserialize<JwtHeader>(headerJson);
        Assert.Equal("JWT", header.Type);
        Assert.Equal("ed25519-nkey", header.Algorithm);

        // Verify payload
        var payloadJson = EncodingUtils.FromBase64UrlEncoded(parts[1]);
        var payload = JsonSerializer.Deserialize<NatsOperatorClaims>(payloadJson);
        Assert.Equal("O", payload.Name);
        Assert.Equal(opk, payload.Subject);
        Assert.Equal(opk, payload.Issuer);
        Assert.Contains(ospk, payload.Operator.SigningKeys);
        Assert.Equal("operator", payload.Operator.Type);
        Assert.Equal(2, payload.Operator.Version);

        // Verify signature (basic check)
        Assert.NotEmpty(parts[2]);

        // Verify the JWT can be decoded and validated
        var decodedClaims = _natsJwt.DecodeOperatorClaims(jwt);
        Assert.NotNull(decodedClaims);
        Assert.Equal(oc.Name, decodedClaims.Name);
        Assert.Equal(oc.Subject, decodedClaims.Subject);
        Assert.Equal(oc.Operator.SigningKeys, decodedClaims.Operator.SigningKeys);
    }

    [Fact]
    public void TestEncodeAccountClaims()
    {
        var akp = KeyPair.CreatePair(PrefixByte.Account);
        var apk = akp.GetPublicKey();
        var ac = _natsJwt.NewAccountClaims(apk);
        ac.Name = "A";

        ac.Account.Imports = [new NatsImport { Name = "Import1", Subject = "import.subject" }];
        ac.Account.Exports = [new NatsExport { Name = "Export1", Subject = "export.subject" }];

        string jwt = _natsJwt.EncodeAccountClaims(ac, akp);

        Assert.NotNull(jwt);
        Assert.NotEmpty(jwt);

        var parts = jwt.Split('.');
        Assert.Equal(3, parts.Length);

        var headerJson = EncodingUtils.FromBase64UrlEncoded(parts[0]);
        var header = JsonSerializer.Deserialize<JwtHeader>(headerJson);
        Assert.Equal("JWT", header.Type);
        Assert.Equal("ed25519-nkey", header.Algorithm);

        var payloadJson = EncodingUtils.FromBase64UrlEncoded(parts[1]);
        var payload = JsonSerializer.Deserialize<NatsAccountClaims>(payloadJson);
        Assert.Equal("A", payload.Name);
        Assert.Equal(apk, payload.Subject);
        Assert.Equal(apk, payload.Issuer);
        Assert.Single(payload.Account.Imports);
        Assert.Single(payload.Account.Exports);
        Assert.Equal("account", payload.Account.Type);
        Assert.Equal(2, payload.Account.Version);

        Assert.NotEmpty(parts[2]);

        var decodedClaims = _natsJwt.DecodeAccountClaims(jwt);
        Assert.NotNull(decodedClaims);
        Assert.Equal(ac.Name, decodedClaims.Name);
        Assert.Equal(ac.Subject, decodedClaims.Subject);
        Assert.Equal(ac.Account.Imports.Count, decodedClaims.Account.Imports.Count);
        Assert.Equal(ac.Account.Exports.Count, decodedClaims.Account.Exports.Count);
    }

    [Fact]
    public void TestEncodeUserClaims()
    {
        var ukp = KeyPair.CreatePair(PrefixByte.User);
        var upk = ukp.GetPublicKey();
        var uc = _natsJwt.NewUserClaims(upk);
        uc.Name = "U";

        uc.User.Pub.Allow = ["allow.>"];
        uc.User.Sub.Allow = ["subscribe.>"];

        string jwt = _natsJwt.EncodeUserClaims(uc, ukp);

        Assert.NotNull(jwt);
        Assert.NotEmpty(jwt);

        var parts = jwt.Split('.');
        Assert.Equal(3, parts.Length);

        var headerJson = EncodingUtils.FromBase64UrlEncoded(parts[0]);
        var header = JsonSerializer.Deserialize<JwtHeader>(headerJson);
        Assert.Equal("JWT", header.Type);
        Assert.Equal("ed25519-nkey", header.Algorithm);

        var payloadJson = EncodingUtils.FromBase64UrlEncoded(parts[1]);
        var payload = JsonSerializer.Deserialize<NatsUserClaims>(payloadJson);
        Assert.Equal("U", payload.Name);
        Assert.Equal(upk, payload.Subject);
        Assert.Equal(upk, payload.Issuer);
        Assert.Contains("allow.>", payload.User.Pub.Allow);
        Assert.Contains("subscribe.>", payload.User.Sub.Allow);
        Assert.Equal("user", payload.User.Type);
        Assert.Equal(2, payload.User.Version);

        Assert.NotEmpty(parts[2]);

        var decodedClaims = _natsJwt.DecodeUserClaims(jwt);
        Assert.NotNull(decodedClaims);
        Assert.Equal(uc.Name, decodedClaims.Name);
        Assert.Equal(uc.Subject, decodedClaims.Subject);
        Assert.Equal(uc.User.Pub.Allow, decodedClaims.User.Pub.Allow);
        Assert.Equal(uc.User.Sub.Allow, decodedClaims.User.Sub.Allow);
    }

    [Fact]
    public void TestEncodeActivationClaims()
    {
        var akp = KeyPair.CreatePair(PrefixByte.Account);
        var apk = akp.GetPublicKey();
        var ac = _natsJwt.NewActivationClaims(apk);
        ac.Name = "Activation";

        ac.Activation.ImportSubject = "import.subject";
        ac.Activation.ImportType = 1;

        string jwt = _natsJwt.EncodeActivationClaims(ac, akp);

        Assert.NotNull(jwt);
        Assert.NotEmpty(jwt);

        var parts = jwt.Split('.');
        Assert.Equal(3, parts.Length);

        var headerJson = EncodingUtils.FromBase64UrlEncoded(parts[0]);
        var header = JsonSerializer.Deserialize<JwtHeader>(headerJson);
        Assert.Equal("JWT", header.Type);
        Assert.Equal("ed25519-nkey", header.Algorithm);

        var payloadJson = EncodingUtils.FromBase64UrlEncoded(parts[1]);
        var payload = JsonSerializer.Deserialize<NatsActivationClaims>(payloadJson);
        Assert.Equal("Activation", payload.Name);
        Assert.Equal(apk, payload.Subject);
        Assert.Equal(apk, payload.Issuer);
        Assert.Equal("import.subject", payload.Activation.ImportSubject);
        Assert.Equal(1, payload.Activation.ImportType);
        Assert.Equal("activation", payload.Activation.Type);
        Assert.Equal(2, payload.Activation.Version);

        Assert.NotEmpty(parts[2]);

        var decodedClaims = _natsJwt.DecodeActivationClaims(jwt);
        Assert.NotNull(decodedClaims);
        Assert.Equal(ac.Name, decodedClaims.Name);
        Assert.Equal(ac.Subject, decodedClaims.Subject);
        Assert.Equal(ac.Activation.ImportSubject, decodedClaims.Activation.ImportSubject);
        Assert.Equal(ac.Activation.ImportType, decodedClaims.Activation.ImportType);
    }

    [Fact]
    public void TestNewActivationClaims()
    {
        string subject = "test.subject";
        var claims = _natsJwt.NewActivationClaims(subject);

        Assert.NotNull(claims);
        Assert.Equal(subject, claims.Subject);
        Assert.NotNull(claims.Activation);
    }

    [Fact]
    public void TestNewAuthorizationRequestClaims()
    {
        string subject = "auth.request";
        var claims = _natsJwt.NewAuthorizationRequestClaims(subject);

        Assert.NotNull(claims);
        Assert.Equal(subject, claims.Subject);
        Assert.NotNull(claims.AuthorizationRequest);
    }

    [Fact]
    public void TestNewAuthorizationResponseClaims()
    {
        string subject = "auth.response";
        var claims = _natsJwt.NewAuthorizationResponseClaims(subject);

        Assert.NotNull(claims);
        Assert.Equal(subject, claims.Subject);
        Assert.NotNull(claims.AuthorizationResponse);
    }

    [Fact]
    public void TestNewGenericClaims()
    {
        string subject = "generic.subject";
        var claims = _natsJwt.NewGenericClaims(subject);

        Assert.NotNull(claims);
        Assert.Equal(subject, claims.Subject);
    }

    [Fact]
    public void TestNewOperatorClaims()
    {
        string subject = "operator.subject";
        var claims = _natsJwt.NewOperatorClaims(subject);

        Assert.NotNull(claims);
        Assert.Equal(subject, claims.Subject);
        Assert.Equal(subject, claims.Issuer);
        Assert.NotNull(claims.Operator);
    }

    [Fact]
    public void TestNewUserClaims()
    {
        string subject = "user.subject";
        var claims = _natsJwt.NewUserClaims(subject);

        Assert.NotNull(claims);
        Assert.Equal(subject, claims.Subject);
        Assert.NotNull(claims.User);
    }

    [Fact]
    public void TestNewAccountClaims()
    {
        string subject = "account.subject";
        var claims = _natsJwt.NewAccountClaims(subject);

        Assert.NotNull(claims);
        Assert.Equal(subject, claims.Subject);
        Assert.NotNull(claims.Account);
    }
}
