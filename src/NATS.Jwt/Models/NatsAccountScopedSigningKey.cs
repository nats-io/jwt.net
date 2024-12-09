// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Text.Json.Serialization;

namespace NATS.Jwt.Models
{
    /// <summary>
    /// Represents an Account Scoped Signing Key.
    /// </summary>
    public record NatsAccountScopedSigningKey : NatsAccountSigningKey
    {
        /// <summary>
        /// Gets or sets the kind of scoped key.
        /// </summary>
        [JsonPropertyName("kind")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Kind { get; set; } = "user_scope";

        /// <summary>
        /// Gets or sets the Key, usually the public key.
        /// </summary>
        [JsonPropertyName("key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets Role.
        /// </summary>
        [JsonPropertyName("role")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the User Template to use.
        /// </summary>
        [JsonPropertyName("template")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public NatsUser Template { get; set; } = new();
    }
}
