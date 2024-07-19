using System.Threading.Tasks;
using NATS.NKeys;
using Xunit;
using Xunit.Abstractions;

namespace NATS.Jwt.Tests;

public class NatsJwtTests
{
    private readonly ITestOutputHelper _output;

    public NatsJwtTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void T()
    {
        var okp = KeyPair.CreatePair(PrefixByte.Operator);
        var opk = okp.GetPublicKey();
        var jwt = new NatsJwt();
        var oc = jwt.NewOperatorClaims(opk);
        oc.Name = "O";

        var oskp = KeyPair.CreatePair(PrefixByte.Operator);
        var ospk = oskp.GetPublicKey();

        oc.Operator.SigningKeys = [ospk];
    }
}
