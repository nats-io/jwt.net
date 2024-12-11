using NATS.Client.Core;
using NATS.NKeys;

namespace NATS.Jwt.DocsExamples;

public class IntroPage
{
    public static async Task Run()
    {
        Console.WriteLine("____________________________________________________________");
        Console.WriteLine("NATS.Jwt.DocsExamples.IntroPage");

        {
            #region nats-jwt

            var okp = KeyPair.CreatePair(PrefixByte.Operator);
            var opk = okp.GetPublicKey();

            var oc = NatsJwt.NewOperatorClaims(opk);
            oc.Name = "Example Operator";

            var oskp = KeyPair.CreatePair(PrefixByte.Operator);
            var ospk = oskp.GetPublicKey();

            oc.Operator.SigningKeys = [ospk];

            var operatorJwt = NatsJwt.EncodeOperatorClaims(oc, okp);

            var akp = KeyPair.CreatePair(PrefixByte.Account);
            var apk = akp.GetPublicKey();

            var ac = NatsJwt.NewAccountClaims(apk);
            ac.Name = "Example Account";

            var askp = KeyPair.CreatePair(PrefixByte.Account);
            var aspk = askp.GetPublicKey();

            ac.Account.SigningKeys = [aspk];
            var accountJwt = NatsJwt.EncodeAccountClaims(ac, oskp);

            var ukp = KeyPair.CreatePair(PrefixByte.User);
            var upk = ukp.GetPublicKey();
            var uc = NatsJwt.NewUserClaims(upk);

            uc.User.IssuerAccount = apk;
            var userJwt = NatsJwt.EncodeUserClaims(uc, askp);

            var userSeed = ukp.GetSeed();

            var conf = $$"""
                         operator: {{operatorJwt}}

                         resolver: MEMORY
                         resolver_preload: {
                                 {{apk}}: {{accountJwt}}
                         }
                         """;

            // generate a creds formatted file that can be used by a NATS client
            const string credsPath = "example_user.creds";
            await File.WriteAllTextAsync(credsPath, NatsJwt.FormatUserConfig(userJwt, userSeed));

            // now we are going to put it together into something that can be run
            // we create a file to store the server configuration, the creds
            // file and a small program that uses the creds file
            const string confPath = "example_server.conf";
            await File.WriteAllTextAsync(confPath, conf);

            // run the server:
            // > nats-server -c example_server.conf

            // Connect as user
            var serverUrl = "nats://localhost:4222";
            var authOpts = new NatsAuthOpts { CredsFile = credsPath };
            var opts = new NatsOpts { Url = serverUrl, AuthOpts = authOpts };
            await using var nats = new NatsConnection(opts);
            await nats.PingAsync();

            #endregion
        }
    }
}
