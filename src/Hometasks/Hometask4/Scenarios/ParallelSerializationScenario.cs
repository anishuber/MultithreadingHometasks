using Common.IO;
using Serialization.Xml;
using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Serialization;

namespace Scenarios
{
    public class ParallelSerializationScenario
    {
        private readonly object _lock = new();

        public static string[] SerializeObjectsWithTasks<T>(List<T> objects, string directory)
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

        public string ReadObjectsWithTasks<T>(string file1, string file2, string resultFileName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(resultFileName);

            PathValidator.ValidateFilePath(file1);
            PathValidator.ValidateFilePath(file2);

            var serializer = new XmlSerializer(typeof(T));

            string resultFilePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                @"..\..\..\..",
                "SerializedObjects",
                resultFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(resultFilePath)!);

            using XmlReader reader1 = XmlReader.Create(file1);
            using XmlReader reader2 = XmlReader.Create(file2);
            using XmlWriter writer = XmlWriter.Create(resultFilePath);

            writer.WriteStartDocument();
            writer.WriteStartElement("Root");

            using var turn1 = new ManualResetEventSlim(true);
            using var turn2 = new ManualResetEventSlim(false);

            Task task1 = Task.Run(() =>
            {
                reader1.MoveToContent();
                reader1.ReadStartElement();

                while (true)
                {
                    turn1.Wait();

                    T? elem = XmlSerializerCustom.DeserializeObjectFromXml<T>(
                        reader1,
                        serializer);

                    if (elem is null)
                    {
                        turn2.Set();
                        break;
                    }

                    lock (_lock)
                    {
                        serializer.Serialize(writer, elem);
                    }

                    turn1.Reset();
                    turn2.Set();
                }
            });

            Task task2 = Task.Run(() =>
            {
                reader2.MoveToContent();
                reader2.ReadStartElement();

                while (true)
                {
                    turn2.Wait();

                    T? elem = XmlSerializerCustom.DeserializeObjectFromXml<T>(
                        reader2,
                        serializer);

                    if (elem is null)
                    {
                        turn1.Set();
                        break;
                    }

                    lock (_lock)
                    {
                        serializer.Serialize(writer, elem);
                    }

                    turn2.Reset();
                    turn1.Set();
                }
            });

            Task.WaitAll(task1, task2);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            return resultFilePath;
        }
    }
}
