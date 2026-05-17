using Common.IO;
using FileReading.Concurrent.Abstractions;
using FileReading.Concurrent.Common;
using FileReading.Concurrent.Threads;
using Serialization.Xml;
using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Serialization;

namespace Scenarios
{
    /// <summary>
    /// Utility class providing methods for parallel serialization and multi-threaded file operations.
    /// </summary>
    public class ThreadingClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadingClass"/> class.
        /// </summary>
        protected ThreadingClass() { }

        /// <summary>
        /// Serializes objects from a list in parallel into separate files (chunks).
        /// </summary>
        /// <typeparam name="T">Type of objects to serialize.</typeparam>
        /// <param name="objects">List of objects to serialize.</param>
        /// <param name="directory">Directory in which chunk files will be created.</param>
        /// <returns>Array of filenames created for the serialized chunks.</returns>
        public static string[] SerializeObjectsParallel<T>(List<T> objects, string directory)
        {
            ArgumentNullException.ThrowIfNull(objects);

            PathValidator.ValidateDirectoryPath(directory);

            var ranges = Partitioner.Create(0, objects.Count, 10);
            ConcurrentBag<string> resultFiles = new ConcurrentBag<string>();

            Parallel.ForEach(ranges, range =>
            {
                var chunk = objects.GetRange(range.Item1, range.Item2 - range.Item1);
                string curFileName = $"serializedobjects_{range.Item1}_{range.Item2 - range.Item1}.xml";
                resultFiles.Add(curFileName);
                string curChunkPath = Path.Combine(directory, curFileName);
                XmlSerializerCustom.SerializeObjectsFromList(curChunkPath, chunk);
            });

            return resultFiles.ToArray();
        }

        /// <summary>
        /// Reads two XML files concurrently, merges their serialized elements into a single XML result file,
        /// and returns the resulting file path. Thread synchronization ensures elements are written alternately.
        /// </summary>
        /// <typeparam name="T">Type of elements in the XML files.</typeparam>
        /// <param name="file1">Path to the first input XML file.</param>
        /// <param name="file2">Path to the second input XML file.</param>
        /// <param name="resultFileName">Name of the resulting merged file.</param>
        /// <returns>Path to the merged XML file created in the project's SerializedObjects directory.</returns>
        public static string SerializeObjectsInTurns<T>(
            string file1,
            string file2,
            string resultFileName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(resultFileName);

            PathValidator.ValidateFilePath(file1);
            PathValidator.ValidateFilePath(file2);

            var serializer = new XmlSerializer(typeof(T));

            string resultFilePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                @"..\..\..\..\..\..\..",
                "artifacts",
                resultFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(resultFilePath)!);

            using XmlReader reader1 = XmlReader.Create(file1);
            using XmlReader reader2 = XmlReader.Create(file2);
            using XmlWriter writer = XmlWriter.Create(resultFilePath);

            writer.WriteStartDocument();
            writer.WriteStartElement("Root");

            using var turn1 = new ManualResetEventSlim(true);
            using var turn2 = new ManualResetEventSlim(false);

            var turnBasedSerializer = new TurnBasedSerializer<T>();

            Thread thread1 = new(() =>
            {
                turnBasedSerializer.MergeFilesByTurn(
                    reader1,
                    serializer,
                    writer,
                    turn1,
                    turn2);
            });

            Thread thread2 = new(() =>
            {
                turnBasedSerializer.MergeFilesByTurn(
                    reader2,
                    serializer,
                    writer,
                    turn2,
                    turn1);
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            return resultFilePath;
        }

        /// <summary>
        /// Reads a file using a single thread and returns its contents.
        /// </summary>
        /// <param name="filePath">Path to the file to read.</param>
        /// <returns>File contents as a string.</returns>
        public static string ReadFileOneThread(string filePath)
        {
            PathValidator.ValidateFilePath(filePath);

            var options = new FileReadOptions {WorkerCount = 1 };

            string result = ThreadFileReader.ReadAllText(filePath, options);
            return result;
        }

        /// <summary>
        /// Reads a file using two threads and returns its contents.
        /// </summary>
        /// <param name="filePath">Path to the file to read.</param>
        /// <returns>File contents as a string.</returns>
        public static string ReadFileTwoThreads(string filePath)
        {
            PathValidator.ValidateFilePath(filePath);

            var options = new FileReadOptions { WorkerCount = 2 };

            string result = ThreadFileReader.ReadAllText(filePath, options);
            return result;
        }

        /// <summary>
        /// Reads a file using ten threads and a semaphore to limit concurrency, then returns its contents.
        /// </summary>
        /// <param name="filePath">Path to the file to read.</param>
        /// <returns>File contents as a string.</returns>
        public static string ReadFileTenThreads(string filePath)
        {
            PathValidator.ValidateFilePath(filePath);

            var options = new FileReadOptions { WorkerCount = 10, MaxConcurrency = 5 };

            string result = ThreadFileReader.ReadAllText(filePath, options);
            return result;
        }
    }
}
