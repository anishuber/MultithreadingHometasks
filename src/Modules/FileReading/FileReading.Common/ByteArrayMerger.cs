namespace FileReading.Common
{
    public static class ByteArrayMerger
    {
        public static byte[] Merge(IReadOnlyList<byte[]> parts)
        {
            int totalLength = parts.Sum(x => x.Length);
            byte[] result = new byte[totalLength];

            int offset = 0;

            foreach (byte[] part in parts)
            {
                Buffer.BlockCopy(part, 0, result, offset, part.Length);
                offset += part.Length;
            }

            return result;
        }
    }
}
