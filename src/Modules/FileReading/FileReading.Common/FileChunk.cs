// <copyright file="FileChunk.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FileReading.Shared;

/// <summary>
/// Represents a chunk of a file, defined by its index, starting byte position, and length in bytes.
/// </summary>
/// <param name="Index">The index of the chunk.</param>
/// <param name="Start">The starting byte position of the chunk.</param>
/// <param name="Length">The length of the chunk in bytes.</param>
public readonly record struct FileChunk(int Index, long Start, long Length);