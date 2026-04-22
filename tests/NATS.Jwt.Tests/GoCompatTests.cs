// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using NATS.Jwt.Models;
using NATS.NKeys;
using Synadia.Orbit.Testing.GoHarness;
using Xunit;
using Xunit.Abstractions;

namespace NATS.Jwt.Tests;

public class GoCompatTests(ITestOutputHelper output)
{
    private const string GoEncodeAuthRequest = "go_encode_auth_request";
    private const string GoEncodeAuthResponse = "go_encode_auth_response";
    private const string GoEncodeGeneric = "go_encode_generic";
    private const string GoEncodeOperator = "go_encode_operator";
    private const string GoEncodeAccount = "go_encode_account";
    private const string GoEncodeUser = "go_encode_user";
    private const string GoDecodeAuthRequest = "go_decode_auth_request";
    private const string GoDecodeAuthResponse = "go_decode_auth_response";
    private const string GoDecodeGeneric = "go_decode_generic";
    private const string GoDecodeOperator = "go_decode_operator";
    private const string GoDecodeAccount = "go_decode_account";
    private const string GoDecodeUser = "go_decode_user";
    private const string GoDecodeRaw = "go_decode_raw";
    private const string Done = "done";

    private static readonly string[] GoModules =
    [
        "github.com/nats-io/jwt/v2@latest",
        "github.com/nats-io/nkeys@latest",
    ];

    // lang=go
    private const string GoCode = $$"""
        package main

        import (
            "bufio"
            "encoding/json"
            "fmt"
            "os"

            "github.com/nats-io/jwt/v2"
            "github.com/nats-io/nkeys"
        )

        type Msg struct {
            Type       string `json:"type"`
            JWT        string `json:"jwt,omitempty"`
            Name       string `json:"name,omitempty"`
            Subject    string `json:"subject,omitempty"`
            OK         bool   `json:"ok,omitempty"`
            Error      string `json:"error,omitempty"`
            ServerPub  string `json:"server_pub,omitempty"`
            AccountPub string `json:"account_pub,omitempty"`
            UserPub    string `json:"user_pub,omitempty"`
            CustomKey  string `json:"custom_key,omitempty"`
            CustomNum  int    `json:"custom_number,omitempty"`
        }

        func writeLine(v interface{}) {
            data, _ := json.Marshal(v)
            fmt.Println(string(data))
        }

        func main() {
            scanner := bufio.NewScanner(os.Stdin)
            for scanner.Scan() {
                line := scanner.Text()
                if line == "" {
                    continue
                }

                var msg Msg
                if err := json.Unmarshal([]byte(line), &msg); err != nil {
                    writeLine(Msg{Type: "error", Error: fmt.Sprintf("parse error: %v", err)})
                    continue
                }

                switch msg.Type {
                case "{{GoEncodeAuthRequest}}":
                    skp, _ := nkeys.CreateServer()
                    sPub, _ := skp.PublicKey()
                    ukp, _ := nkeys.CreateUser()
                    uPub, _ := ukp.PublicKey()

                    arc := jwt.NewAuthorizationRequestClaims(sPub)
                    arc.Name = "GoAuthRequest"
                    arc.UserNkey = uPub
                    arc.Server = jwt.ServerID{Name: "go-server", Host: "localhost", ID: "NABC123"}
                    arc.ClientInformation = jwt.ClientInformation{Host: "127.0.0.1", User: "gouser"}
                    arc.ConnectOptions = jwt.ConnectOptions{Username: "gouser", Protocol: 1}
                    token, _ := arc.Encode(skp)
                    writeLine(Msg{Type: "auth_request", JWT: token, ServerPub: sPub, UserPub: uPub})

                case "{{GoEncodeAuthResponse}}":
                    akp, _ := nkeys.CreateAccount()
                    aPub, _ := akp.PublicKey()
                    ukp, _ := nkeys.CreateUser()
                    uPub, _ := ukp.PublicKey()

                    arc := jwt.NewAuthorizationResponseClaims(uPub)
                    arc.Name = "GoAuthResponse"
                    arc.Jwt = "dummy.jwt.token"
                    arc.IssuerAccount = aPub
                    token, _ := arc.Encode(akp)
                    writeLine(Msg{Type: "auth_response", JWT: token, AccountPub: aPub, UserPub: uPub})

                case "{{GoEncodeGeneric}}":
                    akp, _ := nkeys.CreateAccount()
                    aPub, _ := akp.PublicKey()

                    gc := jwt.NewGenericClaims(aPub)
                    gc.Name = "GoGeneric"
                    gc.Data["custom_key"] = "custom_value"
                    gc.Data["custom_number"] = float64(42)
                    token, _ := gc.Encode(akp)
                    writeLine(Msg{Type: "generic", JWT: token, AccountPub: aPub})

                case "{{GoDecodeAuthRequest}}":
                    claims, err := jwt.DecodeAuthorizationRequestClaims(msg.JWT)
                    if err != nil {
                        writeLine(Msg{Type: "result", OK: false, Error: err.Error()})
                    } else {
                        writeLine(Msg{Type: "result", OK: true, Name: claims.Name, Subject: claims.Subject})
                    }

                case "{{GoDecodeAuthResponse}}":
                    claims, err := jwt.DecodeAuthorizationResponseClaims(msg.JWT)
                    if err != nil {
                        writeLine(Msg{Type: "result", OK: false, Error: err.Error()})
                    } else {
                        writeLine(Msg{Type: "result", OK: true, Name: claims.Name, Subject: claims.Subject})
                    }

                case "{{GoDecodeGeneric}}":
                    claims, err := jwt.DecodeGeneric(msg.JWT)
                    if err != nil {
                        writeLine(Msg{Type: "result", OK: false, Error: err.Error()})
                    } else {
                        name := claims.Name
                        subject := claims.Subject
                        customKey, _ := claims.Data["custom_key"].(string)
                        customNum := 0
                        if v, ok := claims.Data["custom_number"]; ok {
                            if f, ok := v.(float64); ok {
                                customNum = int(f)
                            }
                        }
                        writeLine(Msg{Type: "result", OK: true, Name: name, Subject: subject, CustomKey: customKey, CustomNum: customNum})
                    }

                case "{{GoEncodeOperator}}":
                    okp, _ := nkeys.CreateOperator()
                    oPub, _ := okp.PublicKey()
                    oc := jwt.NewOperatorClaims(oPub)
                    oc.Name = "GoOperator"
                    oc.Operator.AccountServerURL = "https://acct.example.com"
                    token, _ := oc.Encode(okp)
                    writeLine(Msg{Type: "operator", JWT: token, AccountPub: oPub})

                case "{{GoEncodeAccount}}":
                    okp, _ := nkeys.CreateOperator()
                    akp, _ := nkeys.CreateAccount()
                    aPub, _ := akp.PublicKey()
                    ac := jwt.NewAccountClaims(aPub)
                    ac.Name = "GoAccount"
                    token, _ := ac.Encode(okp)
                    writeLine(Msg{Type: "account", JWT: token, AccountPub: aPub})

                case "{{GoEncodeUser}}":
                    akp, _ := nkeys.CreateAccount()
                    aPub, _ := akp.PublicKey()
                    ukp, _ := nkeys.CreateUser()
                    uPub, _ := ukp.PublicKey()
                    uc := jwt.NewUserClaims(uPub)
                    uc.Name = "GoUser"
                    uc.IssuerAccount = aPub
                    token, _ := uc.Encode(akp)
                    writeLine(Msg{Type: "user", JWT: token, AccountPub: aPub, UserPub: uPub})

                case "{{GoDecodeOperator}}":
                    oc, err := jwt.DecodeOperatorClaims(msg.JWT)
                    if err != nil {
                        writeLine(Msg{Type: "result", OK: false, Error: err.Error()})
                    } else {
                        writeLine(Msg{Type: "result", OK: true, Name: oc.Name, Subject: oc.Subject})
                    }

                case "{{GoDecodeAccount}}":
                    ac, err := jwt.DecodeAccountClaims(msg.JWT)
                    if err != nil {
                        writeLine(Msg{Type: "result", OK: false, Error: err.Error()})
                    } else {
                        writeLine(Msg{Type: "result", OK: true, Name: ac.Name, Subject: ac.Subject})
                    }

                case "{{GoDecodeUser}}":
                    uc, err := jwt.DecodeUserClaims(msg.JWT)
                    if err != nil {
                        writeLine(Msg{Type: "result", OK: false, Error: err.Error()})
                    } else {
                        writeLine(Msg{Type: "result", OK: true, Name: uc.Name, Subject: uc.Subject})
                    }

                case "{{GoDecodeRaw}}":
                    _, err := jwt.Decode(msg.JWT)
                    if err != nil {
                        writeLine(Msg{Type: "result", OK: false, Error: err.Error()})
                    } else {
                        writeLine(Msg{Type: "result", OK: true})
                    }

                case "{{Done}}":
                    return

                default:
                    writeLine(Msg{Type: "error", Error: "unknown type: " + msg.Type})
                }
            }
        }
        """;

    private static async Task<JsonNode> Send(GoProcess go, object msg)
    {
        await go.WriteLineAsync(JsonSerializer.Serialize(msg));
        var line = await go.ReadLineAsync();
        return JsonSerializer.Deserialize<JsonNode>(line);
    }

    private static async Task SendDone(GoProcess go) =>
        await go.WriteLineAsync(JsonSerializer.Serialize(new { type = Done }));

    [Fact]
    public async Task Decode_Go_encoded_AuthorizationRequestClaims()
    {
        await using var go = await GoProcess.RunCodeAsync(GoCode, output.WriteLine, GoModules);

        var msg = await Send(go, new { type = GoEncodeAuthRequest });

        var jwt = msg["jwt"].GetValue<string>();
        var userPub = msg["user_pub"].GetValue<string>();

        var claims = NatsJwt.DecodeClaims<NatsAuthorizationRequestClaims>(jwt);

        Assert.Equal("GoAuthRequest", claims.Name);
        Assert.Equal(userPub, claims.AuthorizationRequest.UserNKey);
        Assert.Equal("go-server", claims.AuthorizationRequest.NatsServer.Name);
        Assert.Equal("localhost", claims.AuthorizationRequest.NatsServer.Host);
        Assert.Equal("NABC123", claims.AuthorizationRequest.NatsServer.Id);
        Assert.Equal("127.0.0.1", claims.AuthorizationRequest.NatsClientInformation.Host);
        Assert.Equal("gouser", claims.AuthorizationRequest.NatsClientInformation.User);
        Assert.Equal("gouser", claims.AuthorizationRequest.NatsConnectOptions.Username);
        Assert.Equal(1, claims.AuthorizationRequest.NatsConnectOptions.Protocol);

        await SendDone(go);
    }

    [Fact]
    public async Task Decode_Go_encoded_AuthorizationResponseClaims()
    {
        await using var go = await GoProcess.RunCodeAsync(GoCode, output.WriteLine, GoModules);

        var msg = await Send(go, new { type = GoEncodeAuthResponse });

        var jwt = msg["jwt"].GetValue<string>();
        var accountPub = msg["account_pub"].GetValue<string>();
        var userPub = msg["user_pub"].GetValue<string>();

        var claims = NatsJwt.DecodeClaims<NatsAuthorizationResponseClaims>(jwt);

        Assert.Equal("GoAuthResponse", claims.Name);
        Assert.Equal(userPub, claims.Subject);
        Assert.Equal("dummy.jwt.token", claims.AuthorizationResponse.Jwt);
        Assert.Equal(accountPub, claims.AuthorizationResponse.IssuerAccount);

        await SendDone(go);
    }

    [Fact]
    public async Task Decode_Go_encoded_GenericClaims()
    {
        await using var go = await GoProcess.RunCodeAsync(GoCode, output.WriteLine, GoModules);

        var msg = await Send(go, new { type = GoEncodeGeneric });

        var jwt = msg["jwt"].GetValue<string>();
        var accountPub = msg["account_pub"].GetValue<string>();

        var claims = NatsJwt.DecodeClaims<NatsGenericClaims>(jwt);

        Assert.Equal("GoGeneric", claims.Name);
        Assert.Equal(accountPub, claims.Subject);
        Assert.Equal("custom_value", claims.Data["custom_key"].GetValue<string>());
        Assert.Equal(42, claims.Data["custom_number"].GetValue<int>());

        await SendDone(go);
    }

    [Fact]
    public async Task Go_decodes_DotNet_encoded_AuthorizationRequestClaims()
    {
        await using var go = await GoProcess.RunCodeAsync(GoCode, output.WriteLine, GoModules);

        var skp = KeyPair.CreatePair(PrefixByte.Server);
        var spk = skp.GetPublicKey();
        var ukp = KeyPair.CreatePair(PrefixByte.User);
        var upk = ukp.GetPublicKey();

        var arc = NatsJwt.NewAuthorizationRequestClaims(spk);
        arc.Name = "DotNetAuthRequest";
        arc.AuthorizationRequest.UserNKey = upk;
        arc.AuthorizationRequest.NatsServer = new NatsServerId { Name = "dotnet-server", Host = "localhost", Id = "NXYZ" };
        arc.AuthorizationRequest.NatsClientInformation = new NatsClientInformation { Host = "127.0.0.1", User = "dotnetuser" };
        arc.AuthorizationRequest.NatsConnectOptions = new NatsConnectOptions { Username = "dotnetuser", Protocol = 1 };

        var jwt = NatsJwt.EncodeAuthorizationRequestClaims(arc, skp);

        var result = await Send(go, new { type = GoDecodeAuthRequest, jwt });

        Assert.True(result["ok"].GetValue<bool>(), result["error"]?.GetValue<string>() ?? "unknown error");
        Assert.Equal("DotNetAuthRequest", result["name"].GetValue<string>());
        Assert.Equal(spk, result["subject"].GetValue<string>());

        await SendDone(go);
    }

    [Fact]
    public async Task Go_decodes_DotNet_encoded_AuthorizationResponseClaims()
    {
        await using var go = await GoProcess.RunCodeAsync(GoCode, output.WriteLine, GoModules);

        var akp = KeyPair.CreatePair(PrefixByte.Account);
        var apk = akp.GetPublicKey();
        var ukp = KeyPair.CreatePair(PrefixByte.User);
        var upk = ukp.GetPublicKey();

        var arc = NatsJwt.NewAuthorizationResponseClaims(upk);
        arc.Name = "DotNetAuthResponse";
        arc.AuthorizationResponse.Jwt = "dotnet.jwt.token";
        arc.AuthorizationResponse.IssuerAccount = apk;

        var jwt = NatsJwt.EncodeAuthorizationResponseClaims(arc, akp);

        var result = await Send(go, new { type = GoDecodeAuthResponse, jwt });

        Assert.True(result["ok"].GetValue<bool>(), result["error"]?.GetValue<string>() ?? "unknown error");
        Assert.Equal("DotNetAuthResponse", result["name"].GetValue<string>());
        Assert.Equal(upk, result["subject"].GetValue<string>());

        await SendDone(go);
    }

    [Fact]
    public async Task Go_decodes_DotNet_encoded_GenericClaims()
    {
        await using var go = await GoProcess.RunCodeAsync(GoCode, output.WriteLine, GoModules);

        var akp = KeyPair.CreatePair(PrefixByte.Account);
        var apk = akp.GetPublicKey();

        var gc = NatsJwt.NewGenericClaims(apk);
        gc.Name = "DotNetGeneric";
        gc.Data["custom_key"] = "dotnet_value";
        gc.Data["custom_number"] = 99;

        var jwt = NatsJwt.EncodeGenericClaims(gc, akp);

        var result = await Send(go, new { type = GoDecodeGeneric, jwt });

        Assert.True(result["ok"].GetValue<bool>(), result["error"]?.GetValue<string>() ?? "unknown error");
        Assert.Equal("DotNetGeneric", result["name"].GetValue<string>());
        Assert.Equal(apk, result["subject"].GetValue<string>());
        Assert.Equal("dotnet_value", result["custom_key"].GetValue<string>());
        Assert.Equal(99, result["custom_number"].GetValue<int>());

        await SendDone(go);
    }

    [Fact]
    public async Task Decode_Go_encoded_OperatorClaims()
    {
        await using var go = await GoProcess.RunCodeAsync(GoCode, output.WriteLine, GoModules);

        var msg = await Send(go, new { type = GoEncodeOperator });

        var jwt = msg["jwt"].GetValue<string>();
        var operatorPub = msg["account_pub"].GetValue<string>();

        var claims = NatsJwt.DecodeOperatorClaims(jwt);

        Assert.Equal("GoOperator", claims.Name);
        Assert.Equal(operatorPub, claims.Subject);
        Assert.Equal("https://acct.example.com", claims.Operator.AccountServerUrl);

        await SendDone(go);
    }

    [Fact]
    public async Task Decode_Go_encoded_AccountClaims()
    {
        await using var go = await GoProcess.RunCodeAsync(GoCode, output.WriteLine, GoModules);

        var msg = await Send(go, new { type = GoEncodeAccount });

        var jwt = msg["jwt"].GetValue<string>();
        var accountPub = msg["account_pub"].GetValue<string>();

        var claims = NatsJwt.DecodeAccountClaims(jwt);

        Assert.Equal("GoAccount", claims.Name);
        Assert.Equal(accountPub, claims.Subject);

        await SendDone(go);
    }

    [Fact]
    public async Task Decode_Go_encoded_UserClaims()
    {
        await using var go = await GoProcess.RunCodeAsync(GoCode, output.WriteLine, GoModules);

        var msg = await Send(go, new { type = GoEncodeUser });

        var jwt = msg["jwt"].GetValue<string>();
        var userPub = msg["user_pub"].GetValue<string>();
        var accountPub = msg["account_pub"].GetValue<string>();

        var claims = NatsJwt.DecodeUserClaims(jwt);

        Assert.Equal("GoUser", claims.Name);
        Assert.Equal(userPub, claims.Subject);
        Assert.Equal(accountPub, claims.User.IssuerAccount);

        await SendDone(go);
    }

    [Fact]
    public async Task Go_decodes_DotNet_encoded_OperatorClaims()
    {
        await using var go = await GoProcess.RunCodeAsync(GoCode, output.WriteLine, GoModules);

        var okp = KeyPair.CreatePair(PrefixByte.Operator);
        var opk = okp.GetPublicKey();

        var oc = NatsJwt.NewOperatorClaims(opk);
        oc.Name = "DotNetOperator";

        var jwt = NatsJwt.EncodeOperatorClaims(oc, okp);

        var result = await Send(go, new { type = GoDecodeOperator, jwt });

        Assert.True(result["ok"].GetValue<bool>(), result["error"]?.GetValue<string>() ?? "unknown error");
        Assert.Equal("DotNetOperator", result["name"].GetValue<string>());
        Assert.Equal(opk, result["subject"].GetValue<string>());

        await SendDone(go);
    }

    [Fact]
    public async Task Go_decodes_DotNet_encoded_AccountClaims()
    {
        await using var go = await GoProcess.RunCodeAsync(GoCode, output.WriteLine, GoModules);

        var okp = KeyPair.CreatePair(PrefixByte.Operator);
        var akp = KeyPair.CreatePair(PrefixByte.Account);
        var apk = akp.GetPublicKey();

        var ac = NatsJwt.NewAccountClaims(apk);
        ac.Name = "DotNetAccount";

        var jwt = NatsJwt.EncodeAccountClaims(ac, okp);

        var result = await Send(go, new { type = GoDecodeAccount, jwt });

        Assert.True(result["ok"].GetValue<bool>(), result["error"]?.GetValue<string>() ?? "unknown error");
        Assert.Equal("DotNetAccount", result["name"].GetValue<string>());
        Assert.Equal(apk, result["subject"].GetValue<string>());

        await SendDone(go);
    }

    [Fact]
    public async Task Go_decodes_DotNet_encoded_UserClaims()
    {
        await using var go = await GoProcess.RunCodeAsync(GoCode, output.WriteLine, GoModules);

        var akp = KeyPair.CreatePair(PrefixByte.Account);
        var apk = akp.GetPublicKey();
        var ukp = KeyPair.CreatePair(PrefixByte.User);
        var upk = ukp.GetPublicKey();

        var uc = NatsJwt.NewUserClaims(upk);
        uc.Name = "DotNetUser";
        uc.User.IssuerAccount = apk;

        var jwt = NatsJwt.EncodeUserClaims(uc, akp);

        var result = await Send(go, new { type = GoDecodeUser, jwt });

        Assert.True(result["ok"].GetValue<bool>(), result["error"]?.GetValue<string>() ?? "unknown error");
        Assert.Equal("DotNetUser", result["name"].GetValue<string>());
        Assert.Equal(upk, result["subject"].GetValue<string>());

        await SendDone(go);
    }

    [Fact]
    public async Task Go_rejects_account_jwt_signed_by_user_key()
    {
        await using var go = await GoProcess.RunCodeAsync(GoCode, output.WriteLine, GoModules);

        // Hand-craft an account JWT signed by a user key (bypassing
        // the .NET encode-side prefix check). Both .NET and Go should
        // reject this on decode.
        var ukp = KeyPair.CreatePair(PrefixByte.User);
        var upk = ukp.GetPublicKey();
        var targetSubject = KeyPair.CreatePair(PrefixByte.Account).GetPublicKey();

        var payloadJson =
            "{\"jti\":\"forged\"," +
            "\"iat\":1700000000," +
            "\"iss\":\"" + upk + "\"," +
            "\"sub\":\"" + targetSubject + "\"," +
            "\"nats\":{" +
            "\"limits\":{\"subs\":-1,\"data\":-1,\"payload\":-1}," +
            "\"default_permissions\":{}," +
            "\"type\":\"account\"," +
            "\"version\":2}}";
        var headerJson = "{\"typ\":\"JWT\",\"alg\":\"ed25519-nkey\"}";

        var h = EncodingUtils.ToBase64UrlEncoded(System.Text.Encoding.UTF8.GetBytes(headerJson));
        var p = EncodingUtils.ToBase64UrlEncoded(System.Text.Encoding.UTF8.GetBytes(payloadJson));

        var toSign = System.Text.Encoding.ASCII.GetBytes(h + "." + p);
        var signature = new byte[64];
        ukp.Sign(toSign, signature);
        var sig = EncodingUtils.ToBase64UrlEncoded(signature);

        var forgedJwt = h + "." + p + "." + sig;

        // .NET rejects it
        Assert.Throws<NatsJwtException>(() => NatsJwt.DecodeAccountClaims(forgedJwt));

        // Go also rejects it
        var result = await Send(go, new { type = GoDecodeRaw, jwt = forgedJwt });
        Assert.NotNull(result["error"]);
        output.WriteLine($"Go rejection: {result["error"]?.GetValue<string>()}");

        await SendDone(go);
    }
}
