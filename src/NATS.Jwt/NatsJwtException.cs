// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;

namespace NATS.Jwt;

/// <summary>
/// Represents an exception thrown when an error related to NATS JWT occurs.
/// </summary>
public class NatsJwtException(string message) : Exception(message);
