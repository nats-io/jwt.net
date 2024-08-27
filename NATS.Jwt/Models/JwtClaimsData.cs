// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Text.Json.Serialization;
using NATS.Jwt.Internal;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the data of a JWT claims.
/// </summary>
public record JwtClaimsData
{
    /// <summary>
    /// Gets or sets represents the "aud" claim in a JWT token.
    /// </summary>
    [JsonPropertyName("aud")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Audience { get; set; }

    /// <summary>
    /// Gets or sets represents the unique identifier (ID) claim in a JWT token.
    /// </summary>
    [JsonPropertyName("jti")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the "iat" claim in a JWT token.
    /// </summary>
    /// <remarks>
    /// The "iat" (issued at) claim identifies the time at which the JWT token was issued.
    /// It is a reserved claim in the JWT token standard.
    /// </remarks>
    [JsonPropertyName("iat")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(NatsJsonDateTimeOffsetConverter))]
    public DateTimeOffset? IssuedAt { get; set; }

    /// <summary>
    /// Gets or sets the issuer claim in a JWT token.
    /// </summary>
    [JsonPropertyName("iss")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Issuer { get; set; }

    /// <summary>
    /// Gets or sets the name of a person or entity.
    /// </summary>
    /// <remarks>
    /// This property represents the name of a person or entity. It is typically used in
    /// the context of JWT claims. The "name" claim is a string value that provides a
    /// human-readable name associated with the JWT.
    /// </remarks>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets represents the "sub" claim in a JWT token.
    /// </summary>
    [JsonPropertyName("sub")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Subject { get; set; }

    /// <summary>
    /// Gets or sets the expiration time of a JWT token.
    /// </summary>
    [JsonPropertyName("exp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(NatsJsonDateTimeOffsetConverter))]
    public DateTimeOffset? Expires { get; set; }

    /// <summary>
    /// Gets or sets the "nbf" claim in a JWT token.
    /// Represents the time before which the JWT token is not considered valid.
    /// This property specifies the Unix timestamp (in seconds) of the time before which the token must not be accepted.
    /// </summary>
    [JsonPropertyName("nbf")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(NatsJsonDateTimeOffsetConverter))]
    public DateTimeOffset? NotBefore { get; set; }
}
