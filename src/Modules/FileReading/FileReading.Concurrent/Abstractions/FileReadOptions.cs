// <copyright file="FileReadOptions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FileReading.Concurrent.Abstractions;

using System.Text;

/// <summary>
/// Options for configuring the behavior of concurrent file reading operations, including worker count, maximum concurrency, and text encoding.
/// </summary>
public sealed class FileReadOptions
{
    /// <summary>
    /// Gets or initializes the number of worker threads to use for concurrent file reading. Default is 1, which means no concurrency.
    /// </summary>
    public int WorkerCount { get; init; } = 1;

    /// <summary>
    /// Gets or initializes the maximum number of concurrent file read operations allowed.
    /// If not set, it defaults to the value of <see cref="WorkerCount"/>.
    /// Setting this to a value less than <see cref="WorkerCount"/> will limit the concurrency accordingly.
    /// </summary>
    public int? MaxConcurrency { get; init; }

    /// <summary>
    /// Gets or initializes the text encoding to use when reading files. Default is UTF-8.
    /// </summary>
    public Encoding Encoding { get; init; } = Encoding.UTF8;
}
