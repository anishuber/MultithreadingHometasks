namespace FileReading.Common;

public static class FileChunkPlanner
{
    public static IReadOnlyList<FileChunk> CreateChunks(long fileLength, int workerCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(fileLength);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(workerCount);

        long chunkSize = fileLength / workerCount;
        var chunks = new List<FileChunk>(workerCount);

        for (int i = 0; i < workerCount; i++)
        {
            long start = i * chunkSize;
            long length = i == workerCount - 1
                ? fileLength - start
                : chunkSize;

            chunks.Add(new FileChunk(i, start, length));
        }

        return chunks;
    }
}
