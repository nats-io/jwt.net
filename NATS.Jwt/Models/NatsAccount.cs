// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents a NATS account.
/// </summary>
public record NatsAccount : NatsGenericFields
{
    /// <summary>
    /// Gets or sets the list of NATS imports for the account.
    /// </summary>
    /// <value>
    /// The list of NATS imports for the account.
    /// </value>
    [JsonPropertyName("imports")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<NatsImport>? Imports { get; set; }

    /// <summary>
    /// Gets or sets the list of NATS exports for the account.
    /// </summary>
    /// <value>
    /// The list of NATS exports for the account.
    /// </value>
    [JsonPropertyName("exports")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<NatsExport>? Exports { get; set; }

    /// <summary>
    /// Gets or sets the limits for the NATS account.
    /// </summary>
    /// <value>
    /// The limits for the NATS account.
    /// </value>
    [JsonPropertyName("limits")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsOperatorLimits Limits { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of signing keys for the NATS account.
    /// </summary>
    /// <value>
    /// The list of signing keys for the NATS account.
    /// </value>
    [JsonPropertyName("signing_keys")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string> SigningKeys { get; set; }

    /// <summary>
    /// Gets or sets the dictionary of revocations for the account.
    /// </summary>
    /// <value>
    /// The dictionary of revocations for the account.
    /// </value>
    [JsonPropertyName("revocations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Dictionary<string, long> Revocations { get; set; }

    /// <summary>
    /// Gets or sets the default permissions for the NATS account.
    /// </summary>
    /// <value>
    /// The default permissions for the NATS account.
    /// </value>
    [JsonPropertyName("default_permissions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsPermissions DefaultPermissions { get; set; } = new();

    /// <summary>
    /// Gets or sets the dictionary of NATS weighted mappings for the account.
    /// </summary>
    /// <value>
    /// The dictionary of NATS weighted mappings for the account.
    /// </value>
    [JsonPropertyName("mappings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Dictionary<string, NatsWeightedMapping> Mappings { get; set; }

    /// <summary>
    /// Gets or sets the external authorization for the NATS account.
    /// </summary>
    /// <value>
    /// The external authorization for the NATS account.
    /// </value>
    [JsonPropertyName("authorization")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsExternalAuthorization Authorization { get; set; } = new();

    /// <summary>
    /// Gets or sets the NATS message trace for the account.
    /// </summary>
    /// <value>
    /// The NATS message trace for the account.
    /// </value>
    [JsonPropertyName("trace")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NatsMsgTrace Trace { get; set; }

    /// <summary>
    /// Gets or sets the description of the NATS account.
    /// </summary>
    /// <value>
    /// The description of the NATS account.
    /// </value>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the URL that provides information about the NATS account.
    /// </summary>
    /// <value>
    /// The URL that provides information about the NATS account.
    /// </value>
    [JsonPropertyName("info_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string InfoUrl { get; set; }
}
