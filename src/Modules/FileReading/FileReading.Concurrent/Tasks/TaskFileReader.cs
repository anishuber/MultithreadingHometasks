// <copyright file="TaskFileReader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FileReading.Concurrent.Tasks;

using Common.IO;

/// <summary>
/// Task-based file reader that provides asynchronous methods for reading file contents. It validates file paths and supports cancellation through CancellationToken.
/// </summary>
public static class TaskFileReader
{
    /// <summary>
    /// Reads the contents of a file asynchronously. Validates the file path and supports cancellation through CancellationToken.
    /// </summary>
    /// <param name="filePath">The path of the file to read.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation. The task result contains the file contents as a string.</returns>
    public static Task<string> ReadAllTextAsync(
        string filePath,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(
            () =>
        {
            PathValidator.ValidateFilePath(filePath);
            cancellationToken.ThrowIfCancellationRequested();

            return File.ReadAllText(filePath);
        },
            cancellationToken);
    }
}
