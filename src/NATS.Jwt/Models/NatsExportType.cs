// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

namespace NATS.Jwt.Models;

/// <summary>
/// Defines the type of export.
/// </summary>
public enum NatsExportType
{
    /// <summary>
    /// Unknown is used if we don't know the type.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Stream defines the type field value for a stream "stream".
    /// </summary>
    Stream = 1,

    /// <summary>
    /// Service defines the type field value for a service "service".
    /// </summary>
    Service = 2,
}
