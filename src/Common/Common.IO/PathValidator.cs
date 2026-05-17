namespace Common.IO;

/// <summary>
/// Validation helper for file and directory paths used across the project.
/// </summary>
public static class PathValidator
{
    /// <summary>
    /// Validates that the supplied directory path is non-empty, absolute, and exists.
    /// </summary>
    /// <param name="directory">Directory path to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the path is null/whitespace, not absolute, or does not exist.</exception>
    public static void ValidateDirectoryPath(string? directory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);

        if (!Path.IsPathFullyQualified(directory))
        {
            throw new ArgumentException("The path should be absolute.", nameof(directory));
        }

        if (!Directory.Exists(directory))
        {
            throw new ArgumentException("The directory does not exist.", nameof(directory));
        }
    }

    // TODO: make private after refactoring the code to use ValidateExistingFilePath and ValidateOrCreateFilePath instead of ValidateFilePath.

    /// <summary>
    /// Validates that the supplied file path is non-empty, absolute and points to a file (not a directory).
    /// Also validates that the file's directory exists.
    /// </summary>
    /// <param name="filePath">File path to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the path is null/whitespace, not absolute, points to a directory</exception>
    public static void ValidateFilePath(string filePath)
    {
        if (!Path.IsPathFullyQualified(filePath))
        {
            throw new ArgumentException("The path should be absolute.", nameof(filePath));
        }

        if (Directory.Exists(filePath))
        {
            throw new ArgumentException("The path points to a directory, not a file.", nameof(filePath));
        }

        string? directory = Path.GetDirectoryName(filePath);
        if (string.IsNullOrWhiteSpace(directory))
        {
            throw new ArgumentException("The file path must include a directory.", nameof(filePath));
        }

        string fileName = Path.GetFileName(filePath);
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("The file path must include a file name.", nameof(filePath));
        }
    }

    /// <summary>
    /// Validates that the supplied file path is non-empty, absolute and points to a file (not a directory).
    /// Also validates that the file's directory exists. If the file does not exist, an exception is thrown.
    /// </summary>
    /// <param name="filePath">File path to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the path is null/whitespace, not absolute, points to a directory</exception>
    /// <exception cref="FileNotFoundException">Thrown when the file does not exist.</exception>
    public static void ValidateExistingFilePath(string? filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        ValidateFilePath(filePath);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The file does not exist.", filePath);
        }
    }

    /// <summary>
    /// Validates that the supplied file path is non-empty, absolute and points to a file (not a directory).
    /// Also validates that the file's directory exists. If the file does not exist, it will be created.
    /// </summary>
    /// <param name="filePath">File path to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the path is null/whitespace, not absolute, points to a directory</exception>
    public static void ValidateOrCreateFilePath(string? filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        ValidateFilePath(filePath);

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Dispose();
        }
    }
}
