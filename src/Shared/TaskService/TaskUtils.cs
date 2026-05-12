using System.Collections.Concurrent;
using Helpers;
using SerializationService;

namespace TaskService
{
    public static class TaskUtils
    {
        public static string[] SerializeObjectsParallel<T>(List<T> objects, string directory)
        {
            ArgumentNullException.ThrowIfNull(objects);

            Validators.ValidateDirectoryPath(directory);

            var tasks = new Task<string>[objects.Count / 10 + (objects.Count % 10 == 0 ? 0 : 1)];


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
    }
}
