using Common.IO;

namespace FileReading.Concurrent.Tasks
{
    public static class TaskFileReader
    {
        public static Task<string> ReadAllTextAsync(
            string filePath,
            CancellationToken cancellationToken = default)
        {
            PathValidator.ValidateFilePath(filePath);

            Task<string> readTask = new Task<string>(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return File.ReadAllText(filePath);
            }, cancellationToken);

            readTask.Start();

            return readTask;
        }
    }
}
