﻿// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace NATS.Jwt.Internal;

/// <summary>
/// Provides extension methods for the <see cref="ArrayPool{T}"/> class
/// to resize and manipulate arrays.
/// </summary>
internal static class NatsBufferWriterExtensions
{
    /// <summary>
    /// Changes the number of elements of a rented one-dimensional array to the specified new size.
    /// </summary>
    /// <typeparam name="T">The type of items into the target array to resize.</typeparam>
    /// <param name="pool">The target <see cref="ArrayPool{T}"/> instance to use to resize the array.</param>
    /// <param name="array">The rented <typeparamref name="T"/> array to resize, or <see langword="null"/> to create a new array.</param>
    /// <param name="newSize">The size of the new array.</param>
    /// <param name="clearArray">Indicates whether the contents of the array should be cleared before reuse.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="newSize"/> is less than 0.</exception>
    /// <remarks>When this method returns, the caller must not use any references to the old array anymore.</remarks>
    public static void Resize<T>(this ArrayPool<T> pool, [NotNull] ref T[]? array, int newSize, bool clearArray = false)
    {
        // If the old array is null, just create a new one with the requested size
        if (array is null)
        {
            array = pool.Rent(newSize);

            return;
        }

        // If the new size is the same as the current size, do nothing
        if (array.Length == newSize)
        {
            return;
        }

        // Rent a new array with the specified size, and copy as many items from the current array
        // as possible to the new array. This mirrors the behavior of the Array.Resize API from
        // the BCL: if the new size is greater than the length of the current array, copy all the
        // items from the original array into the new one. Otherwise, copy as many items as possible,
        // until the new array is completely filled, and ignore the remaining items in the first array.
        var newArray = pool.Rent(newSize);
        var itemsToCopy = Math.Min(array.Length, newSize);

        Array.Copy(array, 0, newArray, 0, itemsToCopy);

        pool.Return(array, clearArray);

        array = newArray;
    }
}
