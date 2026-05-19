// <copyright file="FileAccessValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Common.IO;

/// <summary>
/// Validation helper for file access operations, ensuring that file paths are valid and that files can be read without throwing exceptions.
/// </summary>
public static class FileAccessValidator
{
    /// <summary>
    /// +-Tries to read the contents of a file at the specified path, returning false if the path is invalid or if any exceptions occur during the read operation.
    /// </summary>
    /// <param name="filePath">The path of the file to read.</param>
    /// <param name="contents">The contents of the file, if read successfully.</param>
    /// <returns>True if the file was read successfully; otherwise, false.</returns>
    public static bool TryReadFile(string filePath, out string contents)
    {
        contents = string.Empty;

        if (string.IsNullOrWhiteSpace(filePath))
        {
            return false;
        }

        try
        {
            PathValidator.ValidateFilePath(filePath);
            contents = File.ReadAllText(filePath);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
        catch (IOException)
        {
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
