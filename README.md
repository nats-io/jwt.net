# NATS JWT .NET

[![codecov](https://codecov.io/github/nats-io/jwt.net/graph/badge.svg?token=zXUTHG6L3Q)](https://codecov.io/github/nats-io/jwt.net)

**IMPORTANT**: This is a pre-release version of the library. The API is subject to change.

This is a .NET implementation of the JWT library for the NATS ecosystem.

## TODO

- [ ] Add public API documentation
- [ ] Add more tests
- [ ] Enable code coverage
- [ ] Add more examples
- [ ] Add more documentation
- [ ] Remove No-warnings from build

## Installation

You can install the package via NuGet:

```bash
dotnet add package NATS.Jwt --prerelease
```

## Usage

```csharp
var jwt = new NatsJwt();

// create an operator key pair (private key)
var okp = KeyPair.CreatePair(PrefixByte.Operator);
var opk = okp.GetPublicKey();

// create an operator claim using the public key for the identifier
var oc = jwt.NewOperatorClaims(opk);
oc.Name = "Example Operator";

// add an operator signing key to sign accounts
var oskp = KeyPair.CreatePair(PrefixByte.Operator);
var ospk = oskp.GetPublicKey();

// add the signing key to the operator - this makes any account
// issued by the signing key to be valid for the operator
oc.Operator.SigningKeys = [ospk];

// self-sign the operator JWT - the operator trusts itself
var operatorJwt = jwt.Encode(oc, okp);

// create an account keypair
var akp = KeyPair.CreatePair(PrefixByte.Account);
var apk = akp.GetPublicKey();

// create the claim for the account using the public key of the account
var ac = jwt.NewAccountClaims(apk);
ac.Name = "Example Account";

var askp = KeyPair.CreatePair(PrefixByte.Account);
var aspk = askp.GetPublicKey();

// add the signing key (public) to the account
ac.Account.SigningKeys = [aspk];
var accountJwt = jwt.Encode(ac, oskp);

// now back to the account, the account can issue users
// need not be known to the operator - the users are trusted
// because they will be signed by the account. The server will
// look up the account get a list of keys the account has and
// verify that the user was issued by one of those keys
var ukp = KeyPair.CreatePair(PrefixByte.User);
var upk = ukp.GetPublicKey();
var uc = jwt.NewUserClaims(upk);

// since the jwt will be issued by a signing key, the issuer account
// must be set to the public ID of the account
uc.User.IssuerAccount = apk;
var userJwt = jwt.Encode(uc, askp);

// the seed is a version of the keypair that is stored as text
var userSeed = ukp.GetSeed();

var conf = $$"""
             operator: {{operatorJwt}}

             resolver: MEMORY
             resolver_preload: {
                     {{apk}}: {{accountJwt}}
             }
             """;

// generate a creds formatted file that can be used by a NATS client
const string credsPath = $"example_user.creds";
File.WriteAllText(credsPath, jwt.FormatUserConfig(userJwt, userSeed));

// now we are going to put it together into something that can be run
// we create a file to store the server configuration, the creds
// file and a small program that uses the creds file
const string confPath = $"example_server.conf";
File.WriteAllText(confPath, conf);

// run the server:
// > nats-server -c example_server.conf

// Connect as user
var authOpts = new NatsAuthOpts { CredsFile = credsPath };
var opts = new NatsOpts { Url = server.Url, AuthOpts = authOpts };
await using var nats = new NatsConnection(opts);
await nats.PingAsync();
```

## About

A [JWT](https://jwt.io/) implementation that uses [nkeys](https://github.com/nats-io/nkeys.net) to digitally sign
JWT tokens for the [NATS](https://nats.io/) ecosystem.

See also https://github.com/nats-io/jwt
