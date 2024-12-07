// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using NATS.Jwt.Models;

namespace NATS.Jwt.Internal;

/// <inheritdoc />
internal class NatsExportComparer : IComparer<NatsExport>
{
    /// <inheritdoc />
    public int Compare(NatsExport? x, NatsExport? y)
    {
        return string.Compare(x?.Subject, y?.Subject, StringComparison.Ordinal);
    }
}
