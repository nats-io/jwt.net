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
            var operatorJwt = jwt.EncodeOperatorClaims(oc, okp);

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
            var accountJwt = jwt.EncodeAccountClaims(ac, oskp);

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
            var userJwt = jwt.EncodeUserClaims(uc, askp);

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
            await File.WriteAllTextAsync(credsPath, jwt.FormatUserConfig(userJwt, userSeed));

            // now we are going to put it together into something that can be run
            // we create a file to store the server configuration, the creds
            // file and a small program that uses the creds file
            const string confPath = $"example_server.conf";
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
