using System.Text;
using Helpers;

namespace ThreadingService
{
    /// <summary>
    /// Provides helper methods for reading file parts and reading files using multiple threads.
    /// </summary>
    public static class ThreadingUtils
    {
        /// <summary>
        /// Reads a portion of a file starting at <paramref name="startPosition"/> with the specified <paramref name="lengthToRead"/>.
        /// </summary>
        /// <param name="filePath">Path to the file to read.</param>
        /// <param name="startPosition">Start position in bytes.</param>
        /// <param name="lengthToRead">Number of bytes to read.</param>
        /// <returns>Byte array containing the requested file segment.</returns>
        public static byte[] ReadFilePart(string filePath, long startPosition, long lengthToRead)
        {
            ArgumentNullException.ThrowIfNull(filePath);
            Validators.ValidateFilePath(filePath);

            ArgumentOutOfRangeException.ThrowIfNegative(startPosition);
            ArgumentOutOfRangeException.ThrowIfNegative(lengthToRead);

            byte[] buffer = new byte[lengthToRead];

            using FileStream stream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read
                );

            stream.Seek(startPosition, SeekOrigin.Begin);

            int totalRead = 0;

            while (totalRead < lengthToRead)
            {
                int bytesRead = stream.Read(buffer, totalRead, (int)(lengthToRead - totalRead));
                if (bytesRead == 0)
                {
                    break;
                }

                totalRead += bytesRead;
            }

            return buffer;
        }

        /// <summary>
        /// Reads a file using multiple worker threads and concatenates the results into a single string.
        /// </summary>
        /// <param name="filePath">Path to the file to read.</param>
        /// <param name="threadCount">Number of worker threads to use.</param>
        /// <param name="semaphore">Optional semaphore to limit concurrent reads (can be null).</param>
        /// <returns>The combined file contents as a UTF-8 string.</returns>
        public static string ReadFileMultipleThreads(
            string filePath,
            int threadCount,
            SemaphoreSlim? semaphore = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(threadCount);

            Validators.ValidateFilePath(filePath);

            long fileLength = new FileInfo(filePath).Length;
            long batchSize = fileLength / threadCount;

            byte[][] results = new byte[threadCount][];
            Exception?[] exceptions = new Exception?[threadCount];
            Thread[] threads = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                int index = i;
                long localStart = index * batchSize;
                long localLength = (index == threadCount - 1)
                    ? fileLength - localStart
                    : batchSize;

                threads[index] = new Thread(() =>
                {
                    try
                    {
                        semaphore?.Wait();
                        results[index] = ReadFilePart(filePath, localStart, localLength);
                    }
                    catch(Exception ex)
                    {
                        exceptions[index] = ex;
                        results[index] = Array.Empty<byte>();
                    }
                    finally
                    {
                        semaphore?.Release();
                    }
                });

                threads[index].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            
            var hasExceptions = exceptions.Any(e => e is not null);

            if (hasExceptions)
            {
                var inner = exceptions.Where(e => e is not null).Cast<Exception>().ToArray();
                throw new AggregateException($"Ошибка в одном или нескольких потоках", inner);
            }

            byte[] flattenedBytes = FlattenJaggedByteArray(results);
            return Encoding.UTF8.GetString(flattenedBytes, 0, flattenedBytes.Length);
        }

        /// <summary>
        /// Flattens a jagged byte array into a single contiguous byte array.
        /// </summary>
        /// <param name="jaggedByteArray">Array of byte arrays to flatten.</param>
        /// <returns>The flattened byte array.</returns>
        private static byte[] FlattenJaggedByteArray(byte[][] jaggedByteArray)
        {
            ArgumentNullException.ThrowIfNull(jaggedByteArray);

            long totalLength = jaggedByteArray.Sum(b => b.Length);
            byte[] result = new byte[totalLength];

            int step = 0;
            foreach (byte[] byteArray in jaggedByteArray)
            {
                Buffer.BlockCopy(byteArray, 0, result, step, byteArray.Length);
                step += byteArray.Length;
            }

            return result;
        }
    }
}
