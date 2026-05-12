namespace FileReading.Common;

public readonly record struct FileChunk(int Index, long Start, long Length);

