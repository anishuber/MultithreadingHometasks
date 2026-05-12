using Common.IO;
using FileReading.Common;
using FileReading.Concurrent.Abstractions;

namespace FileReading.Concurrent.Threads
{
    public static class ThreadFileReader
    {
        public static string ReadAllText(string filePath, FileReadOptions options)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
            ArgumentNullException.ThrowIfNull(options);

            PathValidator.ValidateFilePath(filePath);

            long fileLength = new FileInfo(filePath).Length;
            var chunks = FileChunkPlanner.CreateChunks(fileLength, options.WorkerCount);

            byte[][] results = new byte[chunks.Count][];
            Exception?[] exceptions = new Exception?[chunks.Count];
            Thread[] threads = new Thread[chunks.Count];

            using SemaphoreSlim? semaphore = options.MaxConcurrency is null
                ? null
                : new SemaphoreSlim(options.MaxConcurrency.Value);

            foreach (FileChunk chunk in chunks)
            {
                threads[chunk.Index] = new Thread(() =>
                {
                    try
                    {
                        semaphore?.Wait();
                        results[chunk.Index] = FilePartReader.Read(
                            filePath,
                            chunk.Start,
                            chunk.Length);
                    }
                    catch (Exception ex)
                    {
                        exceptions[chunk.Index] = ex;
                        results[chunk.Index] = Array.Empty<byte>();
                    }
                    finally
                    {
                        semaphore?.Release();
                    }
                });

                threads[chunk.Index].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            if (exceptions.Any(x => x is not null))
            {
                throw new AggregateException(
                    exceptions.Where(x => x is not null).Cast<Exception>());
            }

            return options.Encoding.GetString(ByteArrayMerger.Merge(results));
        }
    }
}
