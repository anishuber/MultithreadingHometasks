using SerializationService;
using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Serialization;
using ThreadingService;

namespace Hometask3.ThreadingClassLibrary
{
    /// <summary>
    /// Utility class providing methods for parallel serialization and multi-threaded file operations.
    /// </summary>
    public class ThreadingClass
    {
        private readonly object _lock = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadingClass"/> class.
        /// </summary>
        public ThreadingClass() { }

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
            ArgumentException.ThrowIfNullOrWhiteSpace(directory);

            var ranges = Partitioner.Create(0, objects.Count, 10);
            ConcurrentBag<string> resultFiles = new ConcurrentBag<string>();

            Parallel.ForEach(ranges, range =>
            {
                var chunk = objects.GetRange(range.Item1, range.Item2 - range.Item1);
                string curFileName = $"serializedobjects_{range.Item1}_{range.Item2 - range.Item1}.xml";
                resultFiles.Add(curFileName);
                string curChunkPath = Path.Combine(directory, curFileName);
                XmlSerializationUtils.SerializeObjectsFromList(curChunkPath, chunk);
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
        public string ReadObjectsParallel<T>(string file1, string file2, string resultFileName)
        {
            var serializer = new XmlSerializer(typeof(T));
            string resultFilePath = Path.Combine(Path.Combine(
                Directory.GetCurrentDirectory(),
                @"..\..\..\..",
                "SerializedObjects"), resultFileName);

            using XmlReader reader1 = XmlReader.Create(file1);
            using XmlReader reader2 = XmlReader.Create(file2);

            Directory.CreateDirectory(Path.GetDirectoryName(resultFilePath)!);

            using XmlWriter writer = XmlWriter.Create(resultFilePath);

            writer.WriteStartDocument();
            writer.WriteStartElement("Root");

            var turn1 = new ManualResetEvent(true);
            var turn2 = new ManualResetEvent(false);

            Thread thread1 = new Thread(() =>
                {
                    reader1.MoveToContent();
                    reader1.ReadStartElement();

                    while (true)
                    {
                        turn1.WaitOne();

                        T? elem = XmlSerializationUtils.DeserializeObjectFromXml<T>(reader1, serializer);

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
                }
            );

            Thread thread2 = new Thread(() =>
                {
                    reader2.MoveToContent();
                    reader2.ReadStartElement();

                    while (true)
                    {
                        turn2.WaitOne();

                        T? elem = XmlSerializationUtils.DeserializeObjectFromXml<T>(reader2, serializer);

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
                }
            );

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
            string result = ThreadingUtils.ReadFileMultipleThreads(filePath, 1);
            return result;
        }

        /// <summary>
        /// Reads a file using two threads and returns its contents.
        /// </summary>
        /// <param name="filePath">Path to the file to read.</param>
        /// <returns>File contents as a string.</returns>
        public static string ReadFileTwoThreads(string filePath)
        {
            string result = ThreadingUtils.ReadFileMultipleThreads(filePath, 2);
            return result;
        }

        /// <summary>
        /// Reads a file using ten threads and a semaphore to limit concurrency, then returns its contents.
        /// </summary>
        /// <param name="filePath">Path to the file to read.</param>
        /// <returns>File contents as a string.</returns>
        public static string ReadFileTenThreads(string filePath)
        {
            SemaphoreSlim semaphore = new SemaphoreSlim(5, 5);
            string result = ThreadingUtils.ReadFileMultipleThreads(filePath, 10, semaphore);

            return result;
        }
    }
}
