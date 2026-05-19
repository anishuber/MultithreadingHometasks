// <copyright file="FileChunkPlanner.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FileReading.Shared;

/// <summary>
/// Slices a file into chunks based on the file length and the number of worker threads.
/// </summary>
public static class FileChunkPlanner
{
    /// <summary>
    /// Creates a list of file chunks that can be processed in parallel by worker threads.
    /// </summary>
    /// <param name="fileLength">The length of the file to be chunked.</param>
    /// <param name="workerCount">The number of worker threads that will process the chunks.</param>
    /// <returns>A list of file chunks.</returns>
    public static IReadOnlyList<FileChunk> CreateChunks(long fileLength, int workerCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(fileLength);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(workerCount);

        long chunkSize = fileLength / workerCount;
        var chunks = new List<FileChunk>(workerCount);

        for (int i = 0; i < workerCount; i++)
        {
            long start = i * chunkSize;
            long length = i == workerCount - 1
                ? fileLength - start
                : chunkSize;

            chunks.Add(new FileChunk(i, start, length));
        }

        return chunks;
    }
}
