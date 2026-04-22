# NATS .NET JWT Generator

[![License Apache 2.0](https://img.shields.io/badge/License-Apache2-blue.svg)](https://www.apache.org/licenses/LICENSE-2.0)
[![NuGet](https://img.shields.io/nuget/v/NATS.Jwt.svg?cacheSeconds=3600)](https://www.nuget.org/packages/NATS.Jwt)
[![Build](https://github.com/nats-io/jwt.net/actions/workflows/test.yml/badge.svg?branch=main)](https://github.com/nats-io/jwt.net/actions/workflows/test.yml?query=branch%3Amain)
[![codecov](https://codecov.io/github/nats-io/jwt.net/graph/badge.svg?token=zXUTHG6L3Q)](https://codecov.io/github/nats-io/jwt.net)

A .NET library for creating and encoding [NATS](https://nats.io/) JWT tokens
signed with [NKeys](https://github.com/nats-io/nkeys.net). Build operator,
account, and user claims for NATS decentralized authentication and
authorization.

This library is focused on **JWT generation**, not validation. Full token
validation is performed by the [NATS server](https://github.com/nats-io/nats-server)
and tools like [nsc](https://github.com/nats-io/nsc), which use the
[NATS JWT Go library](https://github.com/nats-io/jwt). Do not rely on this
library for server-side token verification.

## Installation

You can install the package via NuGet:

```bash
dotnet add package NATS.Jwt
```

## Usage

```csharp
// create an operator key pair (private key)
var okp = KeyPair.CreatePair(PrefixByte.Operator);
var opk = okp.GetPublicKey();

// create an operator claim using the public key for the identifier
var oc = NatsJwt.NewOperatorClaims(opk);
oc.Name = "Example Operator";

// add an operator signing key to sign accounts
var oskp = KeyPair.CreatePair(PrefixByte.Operator);
var ospk = oskp.GetPublicKey();

// add the signing key to the operator - this makes any account
// issued by the signing key to be valid for the operator
oc.Operator.SigningKeys = [ospk];

// self-sign the operator JWT - the operator trusts itself
var operatorJwt = NatsJwt.Encode(oc, okp);

// create an account keypair
var akp = KeyPair.CreatePair(PrefixByte.Account);
var apk = akp.GetPublicKey();

// create the claim for the account using the public key of the account
var ac = NatsJwt.NewAccountClaims(apk);
ac.Name = "Example Account";

var askp = KeyPair.CreatePair(PrefixByte.Account);
var aspk = askp.GetPublicKey();

// add the signing key (public) to the account
ac.Account.SigningKeys = [aspk];
var accountJwt = NatsJwt.Encode(ac, oskp);

// now back to the account, the account can issue users
// need not be known to the operator - the users are trusted
// because they will be signed by the account. The server will
// look up the account get a list of keys the account has and
// verify that the user was issued by one of those keys
var ukp = KeyPair.CreatePair(PrefixByte.User);
var upk = ukp.GetPublicKey();
var uc = NatsJwt.NewUserClaims(upk);

// since the jwt will be issued by a signing key, the issuer account
// must be set to the public ID of the account
uc.User.IssuerAccount = apk;
var userJwt = NatsJwt.Encode(uc, askp);

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
File.WriteAllText(credsPath, NatsJwt.FormatUserConfig(userJwt, userSeed));

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

A [JWT](https://jwt.io/) generator that uses [NKeys](https://github.com/nats-io/nkeys.net)
to digitally sign tokens for the [NATS](https://nats.io/) decentralized
authentication and authorization model. See also the reference
[Go implementation](https://github.com/nats-io/jwt).
