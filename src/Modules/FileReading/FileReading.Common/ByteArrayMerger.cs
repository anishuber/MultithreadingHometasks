// <copyright file="ByteArrayMerger.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FileReading.Shared;

/// <summary>
/// Merges multiple byte arrays into a single byte array. This is useful for reconstructing the original file from its chunks after reading them concurrently.
/// </summary>
public static class ByteArrayMerger
{
    /// <summary>
    /// Merges a list of byte arrays into a single byte array.
    /// </summary>
    /// <param name="parts">The list of byte arrays to merge.</param>
    /// <returns>A single byte array containing all the bytes from the input arrays.</returns>
    public static byte[] Merge(IReadOnlyList<byte[]> parts)
    {
        int totalLength = parts.Sum(x => x.Length);
        byte[] result = new byte[totalLength];

        int offset = 0;

        foreach (byte[] part in parts)
        {
            Buffer.BlockCopy(part, 0, result, offset, part.Length);
            offset += part.Length;
        }

        return result;
    }
}
