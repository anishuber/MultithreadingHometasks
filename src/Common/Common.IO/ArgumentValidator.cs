// <copyright file="ArgumentValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Common.IO;

/// <summary>
/// Aggregates validation methods for method arguments.
/// </summary>
public static class ArgumentValidator
{
    /// <summary>
    /// Validates that the provided string arguments are not null, empty, or whitespace.
    /// </summary>
    /// <param name="arguments">The string arguments to validate.</param>
    /// <exception cref="ArgumentException">Thrown when any of the string arguments is null, empty, or whitespace.</exception>
    public static void ValidateNonEmptyStrings(params string?[] arguments)
    {
        foreach (string? argument in arguments)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(argument);
        }
    }

    /// <summary>
    /// Validates that the provided arguments of reference types are not null.
    /// </summary>
    /// <typeparam name="T">The type of the arguments to validate.</typeparam>
    /// <param name="arguments">The arguments to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the arguments is null.</exception>
    public static void ValidateNotNullArguments<T>(params T?[] arguments)
        where T : class
    {
        foreach (var argument in arguments)
        {
            ArgumentNullException.ThrowIfNull(argument);
        }
    }
}
