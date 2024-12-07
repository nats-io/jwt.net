// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents the external authorization settings for NATS.
/// </summary>
public record NatsExternalAuthorization
{
    /// <summary>
    /// Gets or sets the list of authorized users for external authorization.
    /// </summary>
    [JsonPropertyName("auth_users")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string> AuthUsers { get; set; }

    /// <summary>
    /// Gets or sets the list of allowed accounts for the external authorization.
    /// </summary>
    [JsonPropertyName("allowed_accounts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string> AllowedAccounts { get; set; }

    /// <summary>
    /// Gets or sets XKey for external authorization.
    /// </summary>
    [JsonPropertyName("xkey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string XKey { get; set; }
}
