namespace Helpers
{
    public static class Validators
    {
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
