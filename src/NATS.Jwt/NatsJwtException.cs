// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;

namespace NATS.Jwt;

/// <summary>
/// Represents an exception thrown when an error related to NATS JWT occurs.
/// </summary>
public class NatsJwtException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NatsJwtException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public NatsJwtException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NatsJwtException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public NatsJwtException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
