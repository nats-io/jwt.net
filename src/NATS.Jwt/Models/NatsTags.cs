// Copyright (c) The NATS Authors.
// Licensed under the Apache License, Version 2.0.

using System.Collections;
using System.Collections.Generic;

namespace NATS.Jwt.Models;

/// <summary>
/// Represents a collection of tags associated with NATS entities.
/// This class can be used to define, retrieve, or manipulate tags
/// that may hold metadata or descriptors relevant to the associated entities.
/// </summary>
public class NatsTags : IEnumerable<string>
{
    private readonly List<string> _tags = new();

    /// <summary>
    /// Adds a new tag to the collection after trimming and converting it to lowercase.
    /// </summary>
    /// <param name="tag">The tag to be added to the collection.</param>
    public void Add(string tag)
    {
        _tags.Add(tag.Trim().ToLowerInvariant());
    }

    /// <summary>
    /// Removes a tag from the collection after trimming and converting it to lowercase.
    /// </summary>
    /// <param name="tag">The tag to be removed from the collection.</param>
    public void Remove(string tag)
    {
        _tags.Remove(tag.Trim().ToLowerInvariant());
    }

    /// <inheritdoc />
    public IEnumerator<string> GetEnumerator()
    {
        return _tags.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Returns a string representation of the NatsTags object,
    /// where the tags are concatenated into a single string separated by commas.
    /// </summary>
    /// <returns>String containing all tags separated by commas.</returns>
    public override string ToString()
    {
        return string.Join(",", _tags);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        int hash = 17;
        foreach (string? tag in _tags)
        {
            hash = (hash * 31) + tag.GetHashCode();
        }

        return hash;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((NatsTags)obj);
    }

    /// <summary>
    /// Determines whether the current NatsTags instance is equal to another NatsTags instance
    /// by comparing their tags collection for exact match.
    /// </summary>
    /// <param name="other">The NatsTags instance to compare with the current instance.</param>
    /// <returns>True if both NatsTags instances have identical tags in the same order; otherwise, false.</returns>
    private bool Equals(NatsTags other)
    {
        if (_tags.Count != other._tags.Count)
        {
            return false;
        }

        for (int i = 0; i < _tags.Count; i++)
        {
            if (_tags[i] != other._tags[i])
            {
                return false;
            }
        }

        return true;
    }
}
