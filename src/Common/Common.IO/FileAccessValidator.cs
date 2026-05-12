namespace Common.IO
{
    public static class FileAccessValidator
    {
        // TODO: Add logging for exceptions that occur during file access validation.
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
}
