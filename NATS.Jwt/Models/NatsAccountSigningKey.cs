// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Text;

namespace NATS.Jwt.Models
{
    /// <summary>
    /// Represents an simple signing Key.
    /// </summary>
    public record NatsAccountSigningKey
    {
        private string _signingKey;

        /// <summary>
        /// An implicit operator to convert to a string.
        /// </summary>
        /// <param name="sk">A signing key.</param>
        public static implicit operator string(NatsAccountSigningKey sk) => sk._signingKey;

        /// <summary>
        /// An implicit operator to convert from a string.
        /// </summary>
        /// <param name="value">A signing key.</param>
        public static implicit operator NatsAccountSigningKey(string value)
        {
            return new NatsAccountSigningKey() { _signingKey = value };
        }

        /// <summary>
        /// Returns the signing key as a string.
        /// </summary>
        /// <returns>The basic signing key.</returns>
        public override string ToString()
        {
            return _signingKey;
        }
    }
}
