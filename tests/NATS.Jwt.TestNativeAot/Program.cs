// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using NATS.Jwt;
using NATS.Jwt.TestNativeAot;
using NATS.NKeys;

var okp = KeyPair.FromSeed("SOAMMC2AYOVIEEW6MRVS3ZC73G3KH5NW23GBB67E44STYPRPBTUC7DUKJU");
var opk = okp.GetPublicKey();

var oc = NatsJwt.NewOperatorClaims(opk);
oc.Name = "O";

var oskp = KeyPair.FromSeed("SOAIJKUDGENIQC7H3R3E55B6HAWSC223RJMZ6NJFTWRF5SVMUQ2CCQYJTI");
var ospk = oskp.GetPublicKey();
oc.Operator.SigningKeys = [ospk];

var operatorJwt = NatsJwt.EncodeOperatorClaims(oc, okp, DateTimeOffset.FromUnixTimeSeconds(1720720359));
const string operatorJwt1 = "eyJ0eXAiOiJKV1QiLCJhbGciOiJlZDI1NTE5LW5rZXkifQ.eyJqdGkiOiI3Q0haWEJDQ0FCTFZENk80VFJGRkcyWjJFUUtHT1A1MzZESFRaUkU0VVZTQ0lDTjZHS0VBIiwiaWF0IjoxNzIwNzIwMzU5LCJpc3MiOiJPQVVHVEg0VEQyNTNZS0RGTVBCVlFBNkxTNkQ0WlJST1RLRksyQkVHRkdPN0EyNVlPUVNUVFpHTiIsIm5hbWUiOiJPIiwic3ViIjoiT0FVR1RINFREMjUzWUtERk1QQlZRQTZMUzZENFpSUk9US0ZLMkJFR0ZHTzdBMjVZT1FTVFRaR04iLCJuYXRzIjp7InNpZ25pbmdfa2V5cyI6WyJPQ0dBQVFIWEJXN1RaN1BMUVZUNUhUU1JMVFVSNldXS1lXTkdESFpKSUZKVlVGM0dWQktZNjI2WiJdLCJ0eXBlIjoib3BlcmF0b3IiLCJ2ZXJzaW9uIjoyfX0.Kv8xA8FmO0XKC79pgEty-bmYCTKpKU6gJPby3OfMMbsUHY4qobdvrpsbrmroCNNZHjSCmwY0Y8Fs-AxO-gSUBQ";
Assert.Equal(operatorJwt1, operatorJwt, "operatorJwt");

var akp = KeyPair.FromSeed("SAAIEBXNONQAZMHHGG4PAFAY5NNDOOFD5PKXG3JHCNWT2HGPVOPNBGLVXY");
var apk = akp.GetPublicKey();
var ac = NatsJwt.NewAccountClaims(apk);
ac.Name = "A";

var askp = KeyPair.FromSeed("SAAOEJM7WOGBA6E67NAHP7TNGR2ABLKIGKA4EY264LIINLRJNRZJE2HOSU");
var aspk = askp.GetPublicKey();
ac.Account.SigningKeys = [aspk];
var accountJwt = NatsJwt.EncodeAccountClaims(ac, oskp, DateTimeOffset.FromUnixTimeSeconds(1720720359));

const string accountJwt1 = "eyJ0eXAiOiJKV1QiLCJhbGciOiJlZDI1NTE5LW5rZXkifQ.eyJqdGkiOiI3STIzVDJGUEpLUU9LRkdTNVVJUjdOUVc0MlVNVEVYRkhISDdOSURYQkFZRVNDNFZFU1JRIiwiaWF0IjoxNzIwNzIwMzU5LCJpc3MiOiJPQ0dBQVFIWEJXN1RaN1BMUVZUNUhUU1JMVFVSNldXS1lXTkdESFpKSUZKVlVGM0dWQktZNjI2WiIsIm5hbWUiOiJBIiwic3ViIjoiQUFGSDNPVTRUUDIzQlZLTEFXUkpFQTNTM1RZSU9BQlI1NUJHQkRZT0g3NFVVTDZKNlBTSkhHSUIiLCJuYXRzIjp7ImxpbWl0cyI6eyJzdWJzIjotMSwiZGF0YSI6LTEsInBheWxvYWQiOi0xLCJpbXBvcnRzIjotMSwiZXhwb3J0cyI6LTEsIndpbGRjYXJkcyI6dHJ1ZSwiY29ubiI6LTEsImxlYWYiOi0xfSwic2lnbmluZ19rZXlzIjpbIkFBMjdGMjRJQVVES1M1T0RRUUU2S0xVQk5TVllIVU5OS0dJWTRWMkpXTDJEU1dKNFFQWlo3TE43Il0sImRlZmF1bHRfcGVybWlzc2lvbnMiOnsicHViIjp7fSwic3ViIjp7fX0sImF1dGhvcml6YXRpb24iOnt9LCJ0eXBlIjoiYWNjb3VudCIsInZlcnNpb24iOjJ9fQ.0dEQvGqCwZhjhCEfnSkhMbGfMtx-G9PxXIyaRjMvqPaTZahHRypH38tbLegJcmVPJ0GvmtqRFD95M5F_bXFsDQ";
Assert.Equal(accountJwt1, accountJwt, "accountJwt");

const string apk1 = "AAFH3OU4TP23BVKLAWRJEA3S3TYIOABR55BGBDYOH74UUL6J6PSJHGIB";
Assert.Equal(apk1, apk, "apk");

var ukp = KeyPair.FromSeed("SUAOT7W25IDZJYUCOMTNXZORZZCQM6HNYMCPYRAIP7JXAMWJT72IURZHBY");
var upk = ukp.GetPublicKey();
var uc = NatsJwt.NewUserClaims(upk);
uc.User.IssuerAccount = apk;
var userJwt = NatsJwt.EncodeUserClaims(uc, askp, DateTimeOffset.FromUnixTimeSeconds(1720720359));

const string userJwt1 = "eyJ0eXAiOiJKV1QiLCJhbGciOiJlZDI1NTE5LW5rZXkifQ.eyJqdGkiOiJRWFBKRFBaNUFVWlBQWk5MWDJBVUI3M1lLU1VZQ0RRNEhVTkpNQ0dTREhaVDNNU1hERlNRIiwiaWF0IjoxNzIwNzIwMzU5LCJpc3MiOiJBQTI3RjI0SUFVREtTNU9EUVFFNktMVUJOU1ZZSFVOTktHSVk0VjJKV0wyRFNXSjRRUFpaN0xONyIsInN1YiI6IlVDTUpLUFJZR1kzUUNaQVhBVUJWWFhJSVIySElPVFJHS1FDTUFERkZHVjNORFZWUkxHNUpLSUxKIiwibmF0cyI6eyJwdWIiOnt9LCJzdWIiOnt9LCJzdWJzIjotMSwiZGF0YSI6LTEsInBheWxvYWQiOi0xLCJpc3N1ZXJfYWNjb3VudCI6IkFBRkgzT1U0VFAyM0JWS0xBV1JKRUEzUzNUWUlPQUJSNTVCR0JEWU9INzRVVUw2SjZQU0pIR0lCIiwidHlwZSI6InVzZXIiLCJ2ZXJzaW9uIjoyfX0.EOj8fO8TshAjXYUxvyg1msy4sg7T250_FK_Jd4qmsOXCu2LrI3-dKeXfj2W3LWegiSvJL09uC2FQ8LEWSHD5BA";
Assert.Equal(userJwt1, userJwt, "userJwt");

const string userSeed1 = "SUAOT7W25IDZJYUCOMTNXZORZZCQM6HNYMCPYRAIP7JXAMWJT72IURZHBY";
Assert.Equal(userSeed1, ukp.GetSeed(), "userSeed");

Console.WriteLine("PASS");
