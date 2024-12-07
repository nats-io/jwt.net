using System;
using System.IO;
using System.Threading.Tasks;
using NATS.Client.Core;
using NATS.NKeys;
using Xunit;
using Xunit.Abstractions;

namespace NATS.Jwt.Tests;

public class ConnectTests
{
    private readonly ITestOutputHelper _output;

    public ConnectTests(ITestOutputHelper output) => _output = output;

    [Fact]
    public async Task Connect_with_pre_generated_JWTs()
    {
        const string operatorJwt = "eyJ0eXAiOiJKV1QiLCJhbGciOiJlZDI1NTE5LW5rZXkifQ.eyJqdGkiOiJBRFJSSU1WTVE1Q0NQSkNYVVZaV1g1R1dONDJCREFVRDNUUUlaT0xNWE1SRFdUQkxWS1FRIiwiaWF0IjoxNzE5NTE5NzI3LCJpc3MiOiJPRFNYNldCTU5SRURCNEk2UzZOQkc3RFVNQ1hJVklFNlFBNDRRNEE0R0c1WlNEQU9DRENFV1A0USIsIm5hbWUiOiJPIiwic3ViIjoiT0RTWDZXQk1OUkVEQjRJNlM2TkJHN0RVTUNYSVZJRTZRQTQ0UTRBNEdHNVpTREFPQ0RDRVdQNFEiLCJuYXRzIjp7InNpZ25pbmdfa2V5cyI6WyJPQlVDRzNSRjQ0VkI0V1FZSEVNVEdKRzNOTUszNlgyNEg0RTVFRkpaSllXWVgzVlY1TDRKSUpTQiJdLCJ0eXBlIjoib3BlcmF0b3IiLCJ2ZXJzaW9uIjoyfX0.9xcbut1K6Lln8231cD0E7Wd9Sell-xEZr8XWbY6Ej4rzFXrAzkz1TUHDrTYm8G2xyEwxf3tykbRuGE-y1DLYDA";
        const string apk = "ABSMSCU7T4MJXN44NXZCAI52BY4QIL7SIKMSFL3LGRWMZLNCIGSTI7K7";
        const string accountJwt = "eyJ0eXAiOiJKV1QiLCJhbGciOiJlZDI1NTE5LW5rZXkifQ.eyJqdGkiOiJGVFVRMzJXWERDR0VaNldJVUdFR0NEM1dSRlJYR1MyQkpENkhQQ09PSTI2WTJMNlhIVlZRIiwiaWF0IjoxNzE5NTE5NzI3LCJpc3MiOiJPQlVDRzNSRjQ0VkI0V1FZSEVNVEdKRzNOTUszNlgyNEg0RTVFRkpaSllXWVgzVlY1TDRKSUpTQiIsIm5hbWUiOiJBIiwic3ViIjoiQUJTTVNDVTdUNE1KWE40NE5YWkNBSTUyQlk0UUlMN1NJS01TRkwzTEdSV01aTE5DSUdTVEk3SzciLCJuYXRzIjp7ImxpbWl0cyI6eyJzdWJzIjotMSwiZGF0YSI6LTEsInBheWxvYWQiOi0xLCJpbXBvcnRzIjotMSwiZXhwb3J0cyI6LTEsIndpbGRjYXJkcyI6dHJ1ZSwiY29ubiI6LTEsImxlYWYiOi0xfSwic2lnbmluZ19rZXlzIjpbIkFCUVZLS1czUTVMUklWSlVORFBMWVZTUkxLRklJR0VLUUdZWkNQWVJEVkFZVFNDVU5DWVJUVDdYIl0sImRlZmF1bHRfcGVybWlzc2lvbnMiOnsicHViIjp7fSwic3ViIjp7fX0sImF1dGhvcml6YXRpb24iOnt9LCJ0eXBlIjoiYWNjb3VudCIsInZlcnNpb24iOjJ9fQ.vpBwb1JelL-ZpDY5QZX520upq2xs6Kq-4UKOhEA-llbSzXu9MgQpH8chmiZHIqOfwiRslNBxfFvmxjJBlikHAQ";
        const string userJwt = "eyJ0eXAiOiJKV1QiLCJhbGciOiJlZDI1NTE5LW5rZXkifQ.eyJqdGkiOiJLTzNMWlhHM0FEWkRER0tKU1Q0UUJZMkFPUlFPQzdGS1lMQ0lPTTdGT0hKSkFPMkVSV1RRIiwiaWF0IjoxNzE5NTE5NzI3LCJpc3MiOiJBQlFWS0tXM1E1TFJJVkpVTkRQTFlWU1JMS0ZJSUdFS1FHWVpDUFlSRFZBWVRTQ1VOQ1lSVFQ3WCIsInN1YiI6IlVBNktDNktCSVBHTklDMk4yRlZKTlFPV0dITjNXMkJITUtaSUtCSlVYRU41SU5JSVpKRlcyNVlGIiwibmF0cyI6eyJwdWIiOnt9LCJzdWIiOnt9LCJzdWJzIjotMSwiZGF0YSI6LTEsInBheWxvYWQiOi0xLCJpc3N1ZXJfYWNjb3VudCI6IkFCU01TQ1U3VDRNSlhONDROWFpDQUk1MkJZNFFJTDdTSUtNU0ZMM0xHUldNWkxOQ0lHU1RJN0s3IiwidHlwZSI6InVzZXIiLCJ2ZXJzaW9uIjoyfX0.eMkzuH_0qI2MHm97OmWL-v40BSq_fF5YJXNkZ71oFG6M6NVnFreXrLvb79nUOOj2Kln5O7LoufL_DWoiCtU9Cg";
        const string userSeed = "SUAOPJ35Z64IDGTSF5CPRXODVLKV4PXYOQ7SNUZMMNSNJVDPV3EF3PNZ2Y";

        const string conf = $$"""
                              operator: {{operatorJwt}}

                              resolver: MEMORY
                              resolver_preload: {
                                      {{apk}}: {{accountJwt}}
                              }
                              """;

        const string confPath = $"server_{nameof(Connect_with_pre_generated_JWTs)}.conf";

        File.WriteAllText(confPath, conf);
        await using var server = await NatsServerProcess.StartAsync(config: confPath);

        // Connect as user
        {
            var authOpts = new NatsAuthOpts { Jwt = userJwt, Seed = userSeed };
            var opts = new NatsOpts { Url = server.Url, AuthOpts = authOpts };
            await using var nats = new NatsConnection(opts);
            await nats.PingAsync();
        }

        // Wrong user credentials
        var exception = await Assert.ThrowsAsync<NatsException>(async () =>
        {
            var authOpts = new NatsAuthOpts { Jwt = userJwt };
            var opts = new NatsOpts { Url = server.Url, AuthOpts = authOpts };
            await using var nats = new NatsConnection(opts);
            await nats.PingAsync();
        });
        _output.WriteLine($"{exception.GetType().Name}: {exception.Message}");
        _output.WriteLine($"{exception.InnerException?.GetType().Name}: {exception.InnerException?.Message}");
        Assert.Contains("Authorization Violation", exception.InnerException?.Message);
    }

    [Fact]
    public async Task Generate_JWTs()
    {
        var jwt = new NatsJwt();

        var okp = KeyPair.FromSeed("SOAMMC2AYOVIEEW6MRVS3ZC73G3KH5NW23GBB67E44STYPRPBTUC7DUKJU".ToCharArray());
        var opk = okp.GetPublicKey();

        var oc = jwt.NewOperatorClaims(opk);
        oc.Name = "O";

        var oskp = KeyPair.FromSeed("SOAIJKUDGENIQC7H3R3E55B6HAWSC223RJMZ6NJFTWRF5SVMUQ2CCQYJTI".ToCharArray());
        var ospk = oskp.GetPublicKey();
        oc.Operator.SigningKeys = [ospk];

        var operatorJwt = jwt.EncodeOperatorClaims(oc, okp, DateTimeOffset.FromUnixTimeSeconds(1720720359));
        const string operatorJwt1 = "eyJ0eXAiOiJKV1QiLCJhbGciOiJlZDI1NTE5LW5rZXkifQ.eyJqdGkiOiI3Q0haWEJDQ0FCTFZENk80VFJGRkcyWjJFUUtHT1A1MzZESFRaUkU0VVZTQ0lDTjZHS0VBIiwiaWF0IjoxNzIwNzIwMzU5LCJpc3MiOiJPQVVHVEg0VEQyNTNZS0RGTVBCVlFBNkxTNkQ0WlJST1RLRksyQkVHRkdPN0EyNVlPUVNUVFpHTiIsIm5hbWUiOiJPIiwic3ViIjoiT0FVR1RINFREMjUzWUtERk1QQlZRQTZMUzZENFpSUk9US0ZLMkJFR0ZHTzdBMjVZT1FTVFRaR04iLCJuYXRzIjp7InNpZ25pbmdfa2V5cyI6WyJPQ0dBQVFIWEJXN1RaN1BMUVZUNUhUU1JMVFVSNldXS1lXTkdESFpKSUZKVlVGM0dWQktZNjI2WiJdLCJ0eXBlIjoib3BlcmF0b3IiLCJ2ZXJzaW9uIjoyfX0.Kv8xA8FmO0XKC79pgEty-bmYCTKpKU6gJPby3OfMMbsUHY4qobdvrpsbrmroCNNZHjSCmwY0Y8Fs-AxO-gSUBQ";
        Assert.Equal(operatorJwt1, operatorJwt);

        var akp = KeyPair.FromSeed("SAAIEBXNONQAZMHHGG4PAFAY5NNDOOFD5PKXG3JHCNWT2HGPVOPNBGLVXY".ToCharArray());
        var apk = akp.GetPublicKey();
        var ac = jwt.NewAccountClaims(apk);
        ac.Name = "A";

        var askp = KeyPair.FromSeed("SAAOEJM7WOGBA6E67NAHP7TNGR2ABLKIGKA4EY264LIINLRJNRZJE2HOSU".ToCharArray());
        var aspk = askp.GetPublicKey();
        ac.Account.SigningKeys = [aspk];
        var accountJwt = jwt.EncodeAccountClaims(ac, oskp, DateTimeOffset.FromUnixTimeSeconds(1720720359));

        const string accountJwt1 = "eyJ0eXAiOiJKV1QiLCJhbGciOiJlZDI1NTE5LW5rZXkifQ.eyJqdGkiOiI3STIzVDJGUEpLUU9LRkdTNVVJUjdOUVc0MlVNVEVYRkhISDdOSURYQkFZRVNDNFZFU1JRIiwiaWF0IjoxNzIwNzIwMzU5LCJpc3MiOiJPQ0dBQVFIWEJXN1RaN1BMUVZUNUhUU1JMVFVSNldXS1lXTkdESFpKSUZKVlVGM0dWQktZNjI2WiIsIm5hbWUiOiJBIiwic3ViIjoiQUFGSDNPVTRUUDIzQlZLTEFXUkpFQTNTM1RZSU9BQlI1NUJHQkRZT0g3NFVVTDZKNlBTSkhHSUIiLCJuYXRzIjp7ImxpbWl0cyI6eyJzdWJzIjotMSwiZGF0YSI6LTEsInBheWxvYWQiOi0xLCJpbXBvcnRzIjotMSwiZXhwb3J0cyI6LTEsIndpbGRjYXJkcyI6dHJ1ZSwiY29ubiI6LTEsImxlYWYiOi0xfSwic2lnbmluZ19rZXlzIjpbIkFBMjdGMjRJQVVES1M1T0RRUUU2S0xVQk5TVllIVU5OS0dJWTRWMkpXTDJEU1dKNFFQWlo3TE43Il0sImRlZmF1bHRfcGVybWlzc2lvbnMiOnsicHViIjp7fSwic3ViIjp7fX0sImF1dGhvcml6YXRpb24iOnt9LCJ0eXBlIjoiYWNjb3VudCIsInZlcnNpb24iOjJ9fQ.0dEQvGqCwZhjhCEfnSkhMbGfMtx-G9PxXIyaRjMvqPaTZahHRypH38tbLegJcmVPJ0GvmtqRFD95M5F_bXFsDQ";
        Assert.Equal(accountJwt1, accountJwt);

        const string apk1 = "AAFH3OU4TP23BVKLAWRJEA3S3TYIOABR55BGBDYOH74UUL6J6PSJHGIB";
        Assert.Equal(apk1, apk);

        var ukp = KeyPair.FromSeed("SUAOT7W25IDZJYUCOMTNXZORZZCQM6HNYMCPYRAIP7JXAMWJT72IURZHBY".ToCharArray());
        var upk = ukp.GetPublicKey();
        var uc = jwt.NewUserClaims(upk);
        uc.User.IssuerAccount = apk;
        var userJwt = jwt.EncodeUserClaims(uc, askp, DateTimeOffset.FromUnixTimeSeconds(1720720359));

        const string userJwt1 = "eyJ0eXAiOiJKV1QiLCJhbGciOiJlZDI1NTE5LW5rZXkifQ.eyJqdGkiOiJRWFBKRFBaNUFVWlBQWk5MWDJBVUI3M1lLU1VZQ0RRNEhVTkpNQ0dTREhaVDNNU1hERlNRIiwiaWF0IjoxNzIwNzIwMzU5LCJpc3MiOiJBQTI3RjI0SUFVREtTNU9EUVFFNktMVUJOU1ZZSFVOTktHSVk0VjJKV0wyRFNXSjRRUFpaN0xONyIsInN1YiI6IlVDTUpLUFJZR1kzUUNaQVhBVUJWWFhJSVIySElPVFJHS1FDTUFERkZHVjNORFZWUkxHNUpLSUxKIiwibmF0cyI6eyJwdWIiOnt9LCJzdWIiOnt9LCJzdWJzIjotMSwiZGF0YSI6LTEsInBheWxvYWQiOi0xLCJpc3N1ZXJfYWNjb3VudCI6IkFBRkgzT1U0VFAyM0JWS0xBV1JKRUEzUzNUWUlPQUJSNTVCR0JEWU9INzRVVUw2SjZQU0pIR0lCIiwidHlwZSI6InVzZXIiLCJ2ZXJzaW9uIjoyfX0.EOj8fO8TshAjXYUxvyg1msy4sg7T250_FK_Jd4qmsOXCu2LrI3-dKeXfj2W3LWegiSvJL09uC2FQ8LEWSHD5BA";
        // Assert.Equal(userJwt1, userJwt);

        const string userSeed1 = "SUAOT7W25IDZJYUCOMTNXZORZZCQM6HNYMCPYRAIP7JXAMWJT72IURZHBY";
        Assert.Equal(userSeed1, ukp.GetSeed());
    }

    [Fact]
    public async Task Connect_with_generated_JWTs()
    {
        var jwt = new NatsJwt();

        var okp = KeyPair.CreatePair(PrefixByte.Operator);
        var opk = okp.GetPublicKey();

        var oc = jwt.NewOperatorClaims(opk);
        oc.Name = "O";

        var oskp = KeyPair.CreatePair(PrefixByte.Operator);
        var ospk = oskp.GetPublicKey();
        oc.Operator.SigningKeys = [ospk];
        var operatorJwt = jwt.EncodeOperatorClaims(oc, okp);

        var akp = KeyPair.CreatePair(PrefixByte.Account);
        var apk = akp.GetPublicKey();
        var ac = jwt.NewAccountClaims(apk);
        ac.Name = "A";

        var askp = KeyPair.CreatePair(PrefixByte.Account);
        var aspk = askp.GetPublicKey();
        ac.Account.SigningKeys = [aspk];
        var accountJwt = jwt.EncodeAccountClaims(ac, oskp);

        var ukp = KeyPair.CreatePair(PrefixByte.User);
        var upk = ukp.GetPublicKey();
        var uc = jwt.NewUserClaims(upk);
        uc.User.IssuerAccount = apk;
        var userJwt = jwt.EncodeUserClaims(uc, askp);

        var userSeed = ukp.GetSeed();

        var conf = $$"""
                     operator: {{operatorJwt}}

                     resolver: MEMORY
                     resolver_preload: {
                             {{apk}}: {{accountJwt}}
                     }
                     """;

        const string confPath = $"server_{nameof(Connect_with_generated_JWTs)}.conf";

        File.WriteAllText(confPath, conf);
        await using var server = await NatsServerProcess.StartAsync(config: confPath);

        // Connect as user
        {
            var authOpts = new NatsAuthOpts { Jwt = userJwt, Seed = userSeed };
            var opts = new NatsOpts { Url = server.Url, AuthOpts = authOpts };
            await using var nats = new NatsConnection(opts);
            await nats.PingAsync();
        }

        // Wrong user credentials
        var exception = await Assert.ThrowsAsync<NatsException>(async () =>
        {
            var authOpts = new NatsAuthOpts { Jwt = userJwt };
            var opts = new NatsOpts { Url = server.Url, AuthOpts = authOpts };
            await using var nats = new NatsConnection(opts);
            await nats.PingAsync();
        });
        _output.WriteLine($"{exception.GetType().Name}: {exception.Message}");
        _output.WriteLine($"{exception.InnerException?.GetType().Name}: {exception.InnerException?.Message}");
        Assert.Contains("Authorization Violation", exception.InnerException?.Message);
    }

    [Fact]
    public async Task Readme_example()
    {
        var jwt = new NatsJwt();

        var okp = KeyPair.CreatePair(PrefixByte.Operator);
        var opk = okp.GetPublicKey();

        var oc = jwt.NewOperatorClaims(opk);
        oc.Name = "Example Operator";

        var oskp = KeyPair.CreatePair(PrefixByte.Operator);
        var ospk = oskp.GetPublicKey();
        oc.Operator.SigningKeys = [ospk];
        var operatorJwt = jwt.EncodeOperatorClaims(oc, okp);

        var akp = KeyPair.CreatePair(PrefixByte.Account);
        var apk = akp.GetPublicKey();
        var ac = jwt.NewAccountClaims(apk);
        ac.Name = "Example Org";

        var askp = KeyPair.CreatePair(PrefixByte.Account);
        var aspk = askp.GetPublicKey();
        ac.Account.SigningKeys = [aspk];
        var accountJwt = jwt.EncodeAccountClaims(ac, oskp);

        var ukp = KeyPair.CreatePair(PrefixByte.User);
        var upk = ukp.GetPublicKey();
        var uc = jwt.NewUserClaims(upk);
        uc.User.IssuerAccount = apk;
        var userJwt = jwt.EncodeUserClaims(uc, askp);

        var userSeed = ukp.GetSeed();

        var conf = $$"""
                     operator: {{operatorJwt}}

                     resolver: MEMORY
                     resolver_preload: {
                             {{apk}}: {{accountJwt}}
                     }
                     """;

        const string confPath = $"example_server.conf";
        File.WriteAllText(confPath, conf);
        await using var server = await NatsServerProcess.StartAsync(config: confPath);

        const string credsPath = $"example_user.creds";
        File.WriteAllText(credsPath, jwt.FormatUserConfig(userJwt, userSeed));

        // Connect as user
        {
            var authOpts = new NatsAuthOpts { CredsFile = credsPath };
            var opts = new NatsOpts { Url = server.Url, AuthOpts = authOpts };
            await using var nats = new NatsConnection(opts);
            await nats.PingAsync();
        }
    }
}
