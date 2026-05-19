// <copyright file="TaskClass.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Scenarios;

using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Serialization;
using Common.IO;
using FileReading.Concurrent.Shared;
using FileReading.Concurrent.Tasks;
using Serialization.Xml;

/// <summary>
/// Class responsible for demonstrating the use of tasks to perform concurrent serialization of objects into XML files.
/// </summary>
public static class TaskClass
{
    /// <summary>
    /// Serializes a list of objects into multiple XML files in parallel. Each file will contain a batch of serialized objects.
    /// </summary>
    /// <typeparam name="T">The type of objects to serialize.</typeparam>
    /// <param name="objects">The list of objects to serialize.</param>
    /// <param name="directory">The directory where the XML files will be saved.</param>
    /// <exception cref="ArgumentNullException">Thrown when the objects list is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the directory path is invalid.</exception>
    /// <exception cref="AggregateException">Thrown when one or more tasks fail during serialization.</exception>
    /// <returns>An array of file names of the serialized XML files.</returns>
    public static string[] SerializeObjectsParallel<T>(List<T> objects, string directory)
    {
        ArgumentNullException.ThrowIfNull(objects);

        PathValidator.ValidateDirectoryPath(directory);

        const int batchSize = 10;

        var ranges = Partitioner.Create(0, objects.Count, batchSize).GetDynamicPartitions();
        var taskFileNames = new List<Task<string>>();

        foreach (var range in ranges)
        {
            taskFileNames.Add(Task.Run(() =>
            {
                var chunk = objects.GetRange(range.Item1, range.Item2 - range.Item1);

                string fileName = $"serializedobjects_{range.Item1}_{range.Item2 - range.Item1}.xml";
                string filePath = Path.Combine(directory, fileName);

                XmlSerializerCustom.SerializeObjectsFromList(filePath, chunk);

                return fileName;
            }));
        }

        try
        {
            Task.WaitAll(taskFileNames.ToArray());
        }
        catch (AggregateException ex)
        {
            foreach (var inner in ex.InnerExceptions)
            {
                Console.WriteLine($"Serialization failed: {inner.Message}");
            }

            throw;
        }

        return taskFileNames
            .Select(task => task.Result)
            .ToArray();
    }

    /// <summary>
    /// Serializes objects from two XML files in turns, ensuring that the serialization process is synchronized between the two files.
    /// </summary>
    /// <typeparam name="T">The type of objects to serialize.</typeparam>
    /// <param name="file1">The path of the first XML file.</param>
    /// <param name="file2">The path of the second XML file.</param>
    /// <param name="resultFileName">The name of the resulting XML file.</param>
    /// <exception cref="ArgumentException">Thrown when the any of the input paths is invalid or the result file name is invalid.</exception>
    /// <exception cref="AggregateException">Thrown when one or more tasks fail during serialization.</exception>"
    /// <returns>The path of the resulting XML file.</returns>
    public static string SerializeObjectsInTurns<T>(string file1, string file2, string resultFileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resultFileName);

        PathValidator.ValidateFilePath(file1);
        PathValidator.ValidateFilePath(file2);

        string resultFilePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            @"..\..\..\..\..\..\..",
            "artifacts",
            "SerializedObjects",
            resultFileName);

        Directory.CreateDirectory(Path.GetDirectoryName(resultFilePath)!);

        using XmlReader reader1 = XmlReader.Create(file1);
        using XmlReader reader2 = XmlReader.Create(file2);
        using XmlWriter writer = XmlWriter.Create(resultFilePath);

        var serializer = new XmlSerializer(typeof(T));
        var turn = new TurnBasedSerializer<T>();

        writer.WriteStartDocument();
        writer.WriteStartElement("Root");

        using var turn1 = new ManualResetEventSlim(true);
        using var turn2 = new ManualResetEventSlim(false);

        Task task1 = Task.Run(() =>
            turn.MergeFilesByTurn(
                reader1,
                serializer,
                writer,
                turn1,
                turn2));

        Task task2 = Task.Run(() =>
            turn.MergeFilesByTurn(
                reader2,
                serializer,
                writer,
                turn2,
                turn1));

        try
        {
            Task.WaitAll(task1, task2);
        }
        catch (AggregateException ex)
        {
            foreach (var inner in ex.InnerExceptions)
            {
                Console.WriteLine($"Serialization failed: {inner.Message}");
            }

            throw;
        }

        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Flush();

        return resultFilePath;
    }

    /// <summary>
    /// Reads the content of a file asynchronously and prints it to the console once the reading is complete.
    /// </summary>
    /// <param name="file3Path">The path of the file to read.</param>
    /// <returns>A task object representing the asynchronous operation.</returns>
    public static async Task ReadAndPrintFileAsync(string file3Path)
    {
        Task<string> readTask = TaskFileReader.ReadAllTextAsync(file3Path);

        Task printTask = readTask.ContinueWith(task =>
        {
            Console.WriteLine(task.Result);
        });

        await Task.WhenAll(readTask, printTask);
    }
}
