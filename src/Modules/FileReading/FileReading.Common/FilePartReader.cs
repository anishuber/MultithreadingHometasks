// <copyright file="FilePartReader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FileReading.Shared;

/// <summary>
/// Class for reading a specific part of a file. It provides a method to read a portion of a file based on the specified start position and length to read.
/// </summary>
public static class FilePartReader
{
    /// <summary>
    /// Reads a portion of a file starting at <paramref name="startPosition"/> with the specified <paramref name="lengthToRead"/>.
    /// </summary>
    /// <param name="filePath">Path to the file to read.</param>
    /// <param name="startPosition">Start position in bytes.</param>
    /// <param name="lengthToRead">Number of bytes to read.</param>
    /// <returns>Byte array containing the requested file segment.</returns>
    public static byte[] Read(string filePath, long startPosition, long lengthToRead)
    {
        byte[] buffer = new byte[lengthToRead];

        using var stream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read);

        stream.Seek(startPosition, SeekOrigin.Begin);

        int totalRead = 0;

        while (totalRead < lengthToRead)
        {
            int read = stream.Read(buffer, totalRead, checked((int)(lengthToRead - totalRead)));

            if (read == 0)
            {
                break;
            }

            totalRead += read;
        }

        return buffer;
    }
}
