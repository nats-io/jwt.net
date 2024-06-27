using System.IO;
using System.Threading.Tasks;
using NATS.Client.Core;
using Xunit;
using Xunit.Abstractions;

namespace NATS.Jwt.Tests;

public class ConnectTests
{
    private readonly ITestOutputHelper _output;

    public ConnectTests(ITestOutputHelper output) => _output = output;

    [Fact]
    public async Task ConnectAsync()
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

        const string confPath = $"{nameof(ConnectAsync)}.conf";

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
}
