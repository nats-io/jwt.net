// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the claims of a NATS user in a JSON Web Token (JWT).
/// </summary>
public record NatsUserClaims : JwtClaimsData
{
    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    [JsonPropertyName("nats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    public NatsUser User { get; set; } = new();

    /// <summary>
    /// Sets the user claims as scoped or not.
    /// </summary>
    /// <param name="scoped">Indicates whether the user claims should be scoped or not.</param>
    public void SetScoped(bool scoped)
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        User.Src = default;
        User.Times = default;
        User.Locale = default;

        if (scoped)
        {
            User.Pub = new NatsPermission();
            User.Sub = new NatsPermission();
            User.Resp = default;
            User.Subs = 0;
            User.Data = 0;
            User.Payload = 0;
            User.BearerToken = default;
            User.AllowedConnectionTypes = default;
        }
        else
        {
            User.Subs = NatsJwt.NoLimit;
            User.Data = NatsJwt.NoLimit;
            User.Payload = NatsJwt.NoLimit;
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
}
