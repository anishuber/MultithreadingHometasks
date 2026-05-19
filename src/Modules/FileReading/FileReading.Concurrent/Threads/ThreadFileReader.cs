// <copyright file="ThreadFileReader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FileReading.Concurrent.Threads;

using Common.IO;
using FileReading.Concurrent.Abstractions;
using FileReading.Shared;

/// <summary>
/// Class responsible for reading files concurrently using threads.
/// </summary>
public static class ThreadFileReader
{
    /// <summary>
    /// Reads the entire content of a file as text using multiple threads based on the provided options.
    /// </summary>
    /// <param name="filePath">The path to the file to read.</param>
    /// <param name="options">The options for reading the file, including worker count and encoding.</param>
    /// <returns>The content of the file as a string.</returns>
    /// <exception cref="AggregateException">Thrown if one or more errors occur during file reading.</exception>
    public static string ReadAllText(string filePath, FileReadOptions options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        ArgumentNullException.ThrowIfNull(options);

        PathValidator.ValidateFilePath(filePath);

        long fileLength = new FileInfo(filePath).Length;
        var chunks = FileChunkPlanner.CreateChunks(fileLength, options.WorkerCount);

        byte[][] results = new byte[chunks.Count][];
        Exception?[] exceptions = new Exception?[chunks.Count];
        Thread[] threads = new Thread[chunks.Count];

        using SemaphoreSlim? semaphore = options.MaxConcurrency is null
            ? null
            : new SemaphoreSlim(options.MaxConcurrency.Value);

        foreach (FileChunk chunk in chunks)
        {
            threads[chunk.Index] = new Thread(() =>
            {
                try
                {
                    semaphore?.Wait();
                    results[chunk.Index] = FilePartReader.Read(
                        filePath,
                        chunk.Start,
                        chunk.Length);
                }
                catch (Exception ex)
                {
                    exceptions[chunk.Index] = ex;
                    results[chunk.Index] = Array.Empty<byte>();
                }
                finally
                {
                    semaphore?.Release();
                }
            });

            threads[chunk.Index].Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        if (exceptions.Any(x => x is not null))
        {
            throw new AggregateException(
                exceptions.Where(x => x is not null).Cast<Exception>());
        }

        return options.Encoding.GetString(ByteArrayMerger.Merge(results));
    }
}
