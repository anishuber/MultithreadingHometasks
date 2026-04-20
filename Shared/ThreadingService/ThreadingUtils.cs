using System.Diagnostics;
using System.Text;

namespace ThreadingService
{
    public static class ThreadingUtils
    {
        public static byte[] ReadFilePart(string filePath, long startPosition, long lengthToRead)
        {
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

        public static string ReadFileMultipleThreads(
            string filePath,
            int threadCount,
            SemaphoreSlim? semaphore = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(threadCount);

            long fileLength = new FileInfo(filePath).Length;
            long batchSize = fileLength / threadCount;

            byte[][] results = new byte[threadCount][];
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
                    semaphore?.Wait();

                    try
                    {
                        results[index] = ReadFilePart(filePath, localStart, localLength);
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

            byte[] flattenedBytes = FlattenJaggedByteArray(results);
            return Encoding.UTF8.GetString(flattenedBytes, 0, flattenedBytes.Length);
        }

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
