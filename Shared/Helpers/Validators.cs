namespace Helpers
{
    /// <summary>
    /// Validation helpers for file and directory paths used across the project.
    /// </summary>
    public static class Validators
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
                throw new ArgumentException("Путь должен быть абсолютным", nameof(directory));
            }

            if (!Directory.Exists(directory))
            {
                throw new ArgumentException("Данная директория не существует");
            }
        }

        /// <summary>
        /// Validates that the supplied file path is non-empty, absolute and points to a file (not a directory).
        /// Also validates that the file's directory exists.
        /// </summary>
        /// <param name="filePath">File path to validate.</param>
        /// <exception cref="ArgumentException">Thrown when the path is null/whitespace, not absolute, points to a directory,
        /// or the containing directory does not exist.</exception>
        public static void ValidateFilePath(string? filePath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

            if (!Path.IsPathFullyQualified(filePath))
            {
                throw new ArgumentException("Путь должен быть абсолютным", nameof(filePath));
            }

            if (Directory.Exists(filePath))
            {
                throw new ArgumentException("Данный путь указывает на директорию, а не на файл", nameof(filePath));
            }

            string? directory = Path.GetDirectoryName(filePath);
            ValidateDirectoryPath(directory);
        }
    }
}
