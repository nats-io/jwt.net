using System.Text.Json;
using System.Text.Json.Nodes;
using NATS.Jwt.Models;
using NATS.NKeys;
using Xunit;
using Xunit.Abstractions;

namespace NATS.Jwt.Tests;

public class NatsJwtTests(ITestOutputHelper output)
{
    [Fact]
    public void TestEncodeOperatorClaims()
    {
        // Setup
        var okp = KeyPair.CreatePair(PrefixByte.Operator);
        var opk = okp.GetPublicKey();
        var oc = NatsJwt.NewOperatorClaims(opk);
        oc.Name = "O";

        var oskp = KeyPair.CreatePair(PrefixByte.Operator);
        var ospk = oskp.GetPublicKey();

        oc.Operator.SigningKeys = [ospk];

        // Encode the claims
        string jwt = NatsJwt.EncodeOperatorClaims(oc, okp);

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
        var decodedClaims = NatsJwt.DecodeOperatorClaims(jwt);
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
        var ac = NatsJwt.NewAccountClaims(apk);
        ac.Name = "A";

        ac.Account.Imports = [new NatsImport { Name = "Import1", Subject = "import.subject" }];
        ac.Account.Exports = [new NatsExport { Name = "Export1", Subject = "export.subject" }];

        string jwt = NatsJwt.EncodeAccountClaims(ac, akp);

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

        var decodedClaims = NatsJwt.DecodeAccountClaims(jwt);
        Assert.NotNull(decodedClaims);
        Assert.Equal(ac.Name, decodedClaims.Name);
        Assert.Equal(ac.Subject, decodedClaims.Subject);
        Assert.Equal(ac.Account.Imports.Count, decodedClaims.Account.Imports.Count);
        Assert.Equal(ac.Account.Exports.Count, decodedClaims.Account.Exports.Count);
    }

    [Fact]
    public void TestEncodeUserClaims()
    {
        var akp = KeyPair.CreatePair(PrefixByte.Account);
        var apk = akp.GetPublicKey();
        var uc = NatsJwt.NewUserClaims(apk);
        uc.Name = "U";

        uc.User.Pub.Allow = ["allow.>"];
        uc.User.Sub.Allow = ["subscribe.>"];

        string jwt = NatsJwt.EncodeUserClaims(uc, akp);

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
        Assert.Equal(apk, payload.Subject);
        Assert.Equal(apk, payload.Issuer);
        Assert.Contains("allow.>", payload.User.Pub.Allow);
        Assert.Contains("subscribe.>", payload.User.Sub.Allow);
        Assert.Equal("user", payload.User.Type);
        Assert.Equal(2, payload.User.Version);

        Assert.NotEmpty(parts[2]);

        var decodedClaims = NatsJwt.DecodeUserClaims(jwt);
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
        var ac = NatsJwt.NewActivationClaims(apk);
        ac.Name = "Activation";

        ac.Activation.ImportSubject = "import.subject";
        ac.Activation.ImportType = NatsExportType.Stream;

        string jwt = NatsJwt.EncodeActivationClaims(ac, akp);

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
        Assert.Equal(NatsExportType.Stream, payload.Activation.ImportType);
        Assert.Equal("activation", payload.Activation.Type);
        Assert.Equal(2, payload.Activation.Version);

        Assert.NotEmpty(parts[2]);

        var decodedClaims = NatsJwt.DecodeActivationClaims(jwt);
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
        var claims = NatsJwt.NewActivationClaims(subject);

        Assert.NotNull(claims);
        Assert.Equal(subject, claims.Subject);
        Assert.NotNull(claims.Activation);
    }

    [Fact]
    public void TestNewAuthorizationRequestClaims()
    {
        string subject = "auth.request";
        var claims = NatsJwt.NewAuthorizationRequestClaims(subject);

        Assert.NotNull(claims);
        Assert.Equal(subject, claims.Subject);
        Assert.NotNull(claims.AuthorizationRequest);
    }

    [Fact]
    public void TestNewAuthorizationResponseClaims()
    {
        string subject = "auth.response";
        var claims = NatsJwt.NewAuthorizationResponseClaims(subject);

        Assert.NotNull(claims);
        Assert.Equal(subject, claims.Subject);
        Assert.NotNull(claims.AuthorizationResponse);
    }

    [Fact]
    public void TestNewGenericClaims()
    {
        string subject = "generic.subject";
        var claims = NatsJwt.NewGenericClaims(subject);

        Assert.NotNull(claims);
        Assert.Equal(subject, claims.Subject);
    }

    [Fact]
    public void TestNewOperatorClaims()
    {
        string subject = "operator.subject";
        var claims = NatsJwt.NewOperatorClaims(subject);

        Assert.NotNull(claims);
        Assert.Equal(subject, claims.Subject);
        Assert.Equal(subject, claims.Issuer);
        Assert.NotNull(claims.Operator);
    }

    [Fact]
    public void TestNewUserClaims()
    {
        string subject = "user.subject";
        var claims = NatsJwt.NewUserClaims(subject);

        Assert.NotNull(claims);
        Assert.Equal(subject, claims.Subject);
        Assert.NotNull(claims.User);
    }

    [Fact]
    public void TestNewAccountClaims()
    {
        string subject = "account.subject";
        var claims = NatsJwt.NewAccountClaims(subject);

        Assert.NotNull(claims);
        Assert.Equal(subject, claims.Subject);
        Assert.NotNull(claims.Account);
    }

    [Fact]
    public void TestMultipleExports()
    {
        var operatorSigningKey = KeyPair.CreatePair(PrefixByte.Operator);
        var systemAccountKeyPair = KeyPair.CreatePair(PrefixByte.Account);

        // Create System Account
        var systemAccountClaims = NatsJwt.NewAccountClaims(systemAccountKeyPair.GetPublicKey());
        systemAccountClaims.Name = "SYS";
        systemAccountClaims.Account.Exports =
        [
            new()
            {
                Name = "account-monitoring-services",
                Subject = "$SYS.REQ.ACCOUNT.*.*",
                AccountTokenPosition = 4,
                Type = NatsExportType.Service,
                ResponseType = "Stream",
                Description = "Request account specific monitoring services for: SUBSZ, CONNZ, LEAFZ, JSZ and INFO",
                InfoUrl = "https://docs.nats.io/nats-server/configuration/sys_accounts",
            },
            new()
            {
                Name = "account-monitoring-streams",
                Subject = "$SYS.ACCOUNT.*.>",
                AccountTokenPosition = 3,
                Type = NatsExportType.Service,
                Description = "Account specific monitoring stream",
                InfoUrl = "https://docs.nats.io/nats-server/configuration/sys_accounts",
            },
        ];
        systemAccountClaims.Account.Imports =
        [
            new NatsImport
            {
                Name = "account-monitoring",
                Subject = "$SYS.ACCOUNT.*.*",
                Account = systemAccountKeyPair.GetPublicKey(),
                Type = NatsExportType.Service,
                LocalSubject = "account-monitoring",
            },
            new NatsImport
            {
                Name = "account-monitoring2",
                Subject = "$SYS.ACCOUNT.*.>",
                Account = systemAccountKeyPair.GetPublicKey(),
                Type = NatsExportType.Service,
                LocalSubject = "account-monitoring2",
            },
        ];

        var jwt = NatsJwt.EncodeAccountClaims(systemAccountClaims, operatorSigningKey);
        var payload = EncodingUtils.FromBase64UrlEncoded(jwt.Split('.')[1]);
        var json = JsonSerializer.Deserialize<JsonNode>(payload);

        // Verify the exports are sorted by name
        Assert.Equal("account-monitoring-streams", json["nats"]["exports"][0]["name"].GetValue<string>());
        Assert.Equal("account-monitoring-services", json["nats"]["exports"][1]["name"].GetValue<string>());

        string jsonStr = JsonSerializer.Serialize(json, new JsonSerializerOptions { WriteIndented = true });
        output.WriteLine(jsonStr);
    }

    [Fact]
    public void TestDecodeUserClaimWithTamperedJWTThrowsError()
    {
        var jwt = "eyJ0eXAiOiJKV1QiLCJhbGciOiJlZDI1NTE5LW5rZXkifQ.eyJqdGkiOiJPSk9CUkZDQ0NGNEMzU1JWQzRLNEhTVFNYRlBTSVZBSzJPRUxOMlZXNE9GQ0IzQTVMMkNBIiwiaWF0IjoxNzMwMzk3NTU0LCJpc3MiOiJVQklUR0VSQk9JVEZDWkJHNTNUSkk3M1BHTjdBMzdPTVkyWE5YUU82VUZUSlA1TE5VWVFORUpXSSIsIm5hbWUiOiJVWFgiLCJzdWIiOiJVQklUR0VSQk9JVEZDWkJHNTNUSkk3M1BHTjdBMzdPTVkyWE5YUU82VUZUSlA1TE5VWVFORUpXSSIsIm5hdHMiOnsicHViIjp7ImFsbG93IjpbImFsbG93Llx1MDAzRSJdfSwic3ViIjp7ImFsbG93IjpbInN1YnNjcmliZS5cdTAwM0UiXX0sInN1YnMiOi0xLCJkYXRhIjotMSwicGF5bG9hZCI6LTEsInR5cGUiOiJ1c2VyIiwidmVyc2lvbiI6Mn19.SjIBpWWLNCZmgYZwrFHEJSTkm5M9bik0kgQyG-3V9Nn5sTrfO1Llj3hs7z9R7b1rCyGsFm1RkpZAVAnS5ay2BA";
        Assert.Throws<NatsJwtException>(() => NatsJwt.DecodeUserClaims(jwt));
    }
}
