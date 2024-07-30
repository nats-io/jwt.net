using System.Text.Json.Serialization;
using NATS.Jwt.Models;

namespace NATS.Jwt;

[JsonSerializable(typeof(JwtClaimsData))]
[JsonSerializable(typeof(JwtHeader))]
[JsonSerializable(typeof(JetStreamLimits))]
[JsonSerializable(typeof(NatsAccount))]
[JsonSerializable(typeof(NatsAccountClaims))]
[JsonSerializable(typeof(NatsExport))]
[JsonSerializable(typeof(NatsExternalAuthorization))]
[JsonSerializable(typeof(NatsGenericFields))]
[JsonSerializable(typeof(NatsImport))]
[JsonSerializable(typeof(NatsMsgTrace))]
[JsonSerializable(typeof(NatsOperator))]
[JsonSerializable(typeof(NatsOperatorClaims))]
[JsonSerializable(typeof(NatsOperatorLimits))]
[JsonSerializable(typeof(NatsPermission))]
[JsonSerializable(typeof(NatsPermissions))]
[JsonSerializable(typeof(NatsResponsePermission))]
[JsonSerializable(typeof(NatsServiceLatency))]
[JsonSerializable(typeof(NatsUser))]
[JsonSerializable(typeof(NatsUserClaims))]
[JsonSerializable(typeof(NatsWeightedMapping))]
[JsonSerializable(typeof(TimeRange))]
[JsonSerializable(typeof(NatsAuthorizationRequestClaims))]
[JsonSerializable(typeof(NatsAuthorizationRequest))]
[JsonSerializable(typeof(NatsServerId))]
[JsonSerializable(typeof(NatsClientInformation))]
[JsonSerializable(typeof(NatsConnectOptions))]
[JsonSerializable(typeof(NatsClientTls))]
[JsonSerializable(typeof(NatsGenericClaims))]
[JsonSerializable(typeof(NatsActivationClaims))]
[JsonSerializable(typeof(NatsActivation))]
[JsonSerializable(typeof(NatsAuthorizationResponseClaims))]
[JsonSerializable(typeof(NatsAuthorizationResponse))]
[JsonSerializable(typeof(NatsGenericFieldsClaims))]
internal sealed partial class JsonContext : JsonSerializerContext
{
}
