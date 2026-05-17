using Common.IO;
using FileReading.Concurrent.Common;
using FileReading.Concurrent.Tasks;
using Serialization.Xml;
using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Serialization;

namespace Scenarios
{
    public static class TaskClass
    {
        public static string[] SerializeObjectsParallel<T>(List<T> objects, string directory)
        {
            ArgumentNullException.ThrowIfNull(objects);

            PathValidator.ValidateDirectoryPath(directory);

            const int batchSize = 10;

            var ranges = Partitioner.Create(0, objects.Count, batchSize).GetDynamicPartitions();
            var taskFileNames = new List<Task<string>>();

            foreach (var range in ranges)
            {
                taskFileNames.Add(Task.Run(() =>
                {
                    var chunk = objects.GetRange(range.Item1, range.Item2 - range.Item1);

                    string fileName = $"serializedobjects_{range.Item1}_{range.Item2 - range.Item1}.xml";
                    string filePath = Path.Combine(directory, fileName);

                    XmlSerializerCustom.SerializeObjectsFromList(filePath, chunk);

                    return fileName;
                }));
            }

            Task.WaitAll(taskFileNames.ToArray());

            return taskFileNames
                .Select(task => task.Result)
                .ToArray();
        }

        public static string SerializeObjectsInTurns<T>(string file1, string file2, string resultFileName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(resultFileName);

            PathValidator.ValidateFilePath(file1);
            PathValidator.ValidateFilePath(file2);

            string resultFilePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                @"..\..\..\..\..\..\..",
                "artifacts",
                "SerializedObjects",
                resultFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(resultFilePath)!);

            using XmlReader reader1 = XmlReader.Create(file1);
            using XmlReader reader2 = XmlReader.Create(file2);
            using XmlWriter writer = XmlWriter.Create(resultFilePath);

            var serializer = new XmlSerializer(typeof(T));
            var turn = new TurnBasedSerializer<T>();

            writer.WriteStartDocument();
            writer.WriteStartElement("Root");

            using var turn1 = new ManualResetEventSlim(true);
            using var turn2 = new ManualResetEventSlim(false);

            Task task1 = Task.Run(() =>
                turn.MergeFilesByTurn(
                    reader1,
                    serializer,
                    writer,
                    turn1,
                    turn2));

            Task task2 = Task.Run(() =>
                turn.MergeFilesByTurn(
                    reader2,
                    serializer,
                    writer,
                    turn2,
                    turn1));

            Task.WaitAll(task1, task2);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            return resultFilePath;
        }

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
