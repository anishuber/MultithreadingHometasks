using Serialization.Xml;
using System.Xml;
using System.Xml.Serialization;

namespace FileReading.Concurrent.Common
{
    public class TurnBasedSerializer<T>
    {
        private readonly object writerLock = new();

        public void MergeFilesByTurn(
            XmlReader reader,
            XmlSerializer serializer,
            XmlWriter writer,
            ManualResetEventSlim initialTurn,
            ManualResetEventSlim consequentTurn)
        {
            reader.MoveToContent();
            reader.ReadStartElement();

            while (true)
            {
                initialTurn.Wait();
                initialTurn.Reset();

                T? elem = XmlSerializerCustom.DeserializeObjectFromXml<T>(
                    reader,
                    serializer);

                if (elem is null)
                {
                    consequentTurn.Set();
                    return;
                }

                lock (writerLock)
                {
                    serializer.Serialize(writer, elem);
                }

                consequentTurn.Set();
            }
        }
    }
}
