// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

namespace NATS.Jwt.CheckNativeAot;

/// <summary>
/// The Assert class provides methods for performing assertions in tests.
/// </summary>
public static class Assert
{
    /// <summary>
    /// Checks if two strings are equal.
    /// </summary>
    /// <param name="expected">The first string to compare.</param>
    /// <param name="actual">The second string to compare.</param>
    /// <param name="name">The name of the comparison for error reporting.</param>
    /// <exception cref="Exception">Thrown when the strings are not equal.</exception>
    public static void Equal(string expected, string actual, string name)
    {
        if (!string.Equals(expected, actual))
        {
            throw new Exception($"Strings are not equal ({name}).\n---\nExpected:\n{expected}\nActual:\n{actual}\n---");
        }

        Console.WriteLine($"OK {name}");
    }
}
