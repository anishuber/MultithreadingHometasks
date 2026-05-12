using FileReading.Concurrent.Tasks;

namespace Scenarios
{
    public static class TaskReadingScenario
    {
        public static async Task ReadAndPrintFileAsync(string file3Path)
        {
            Task<string> readTask = TaskFileReader.ReadAllTextAsync(file3Path);

            Task printTask = readTask.ContinueWith(task =>
            {
                Console.WriteLine(task.Result);
            });

            await Task.WhenAll(readTask, printTask);
        }
    }
}
