using SerializationService;
using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Serialization;
using ThreadingService;

namespace Hometask3.ThreadingClassLibrary
{
    public class ThreadingClass
    {
        private readonly object _lock = new();

        public ThreadingClass() { }

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

        public string ReadObjectsParallel<T>(string file1, string file2)
        {
            var serializer = new XmlSerializer(typeof(T));
            string resultFileName = "resultFile.xml";
            string resultFilePath = Path.Combine(Directory.GetCurrentDirectory(), resultFileName);

            using XmlReader reader1 = XmlReader.Create(file1);
            using XmlReader reader2 = XmlReader.Create(file2);
            using XmlWriter writer = XmlWriter.Create(resultFileName);

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

            return resultFilePath;
        }

        public static string ReadFileOneThread(string filePath)
        {
            string result = ThreadingUtils.ReadFileMultipleThreads(filePath, 1);
            return result;
        }

        public static string ReadFileTwoThreads(string filePath)
        {
            string result = ThreadingUtils.ReadFileMultipleThreads(filePath, 2);
            return result;
        }

        public static string ReadFileTenThreads(string filePath)
        {
            SemaphoreSlim semaphore = new SemaphoreSlim(5, 5);
            string result = ThreadingUtils.ReadFileMultipleThreads(filePath, 10, semaphore);

            return result;
        }
    }
}
