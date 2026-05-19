// <copyright file="TurnBasedSerializer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FileReading.Concurrent.Shared;

using System.Xml;
using System.Xml.Serialization;
using Serialization.Xml;

/// <summary>
/// Class responsible for merging XML files in a turn-based manner. It uses synchronization primitives.
/// </summary>
/// <typeparam name="T">The type of objects to serialize.</typeparam>
public class TurnBasedSerializer<T>
{
    private readonly object writerLock = new();

    /// <summary>
    /// Merges XML files by reading from the provided XmlReader and writing to the provided XmlWriter in a turn-based manner between threads.
    /// </summary>
    /// <param name="reader">The XmlReader to read from.</param>
    /// <param name="serializer">The XmlSerializer to use for deserialization and serialization.</param>
    /// <param name="writer">The XmlWriter to write to.</param>
    /// <param name="initialTurn">The ManualResetEventSlim representing the initial turn for this thread.</param>
    /// <param name="consequentTurn">The ManualResetEventSlim representing the consequent turn for the other thread.</param>
    /// <exception cref="Exception">Throws any exception that occurs during reading or writing, ensuring that the consequent turn is set to avoid deadlocks.</exception>
    public void MergeFilesByTurn(
        XmlReader reader,
        XmlSerializer serializer,
        XmlWriter writer,
        ManualResetEventSlim initialTurn,
        ManualResetEventSlim consequentTurn)
    {
        try
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

                lock (this.writerLock)
                {
                    serializer.Serialize(writer, elem);
                }

                consequentTurn.Set();
            }
        }
        catch
        {
            consequentTurn.Set();
            throw;
        }
    }
}
