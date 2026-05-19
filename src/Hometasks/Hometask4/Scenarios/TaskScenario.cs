// <copyright file="TaskScenario.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Scenarios;

using Common.IO;
using FileReading.Concurrent.Tasks;
using Samples.CarLibrary;
using Samples.Storages;

/// <summary>
/// Concrete scenarios demonstrating the use of tasks to perform concurrent operations such as serialization and file reading.
/// </summary>
public static class TaskScenario
{
    /// <summary>
    /// Executes Task 1.
    /// </summary>
    public static void RunTask1()
    {
        Console.WriteLine("Running task 1\n");
        string directoryPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            @"..\..\..\..\..\..\..",
            "artifacts");

        PathValidator.ValidateDirectoryPath(directoryPath);

        var carObjects = InMemoryCarStorage.Cars.ToList();

        try
        {
            var resultFiles = TaskClass.SerializeObjectsParallel(carObjects, directoryPath);
            foreach (var file in resultFiles)
            {
                Console.WriteLine($"\nContents of file {file}:");
                PrintFile(Path.Combine(directoryPath, file));
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"IO error during serialization: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Access denied during serialization: {ex.Message}");
        }
         catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error during serialization: {ex.Message}");
        }
    }

    /// <summary>
    /// Executes Task 2.
    /// </summary>
    /// <returns>The path of the resulting XML file.</returns>
    /// <exception cref="InvalidOperationException">Thrown when there are not enough XML files to perform the task.</exception>
    public static string RunTask2()
    {
        Console.WriteLine("\nRunning task 2\n");

        string directoryPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            @"..\..\..\..\..\..\..",
            "artifacts");

        PathValidator.ValidateDirectoryPath(directoryPath);

        string resultFileName = "resultFile.xml";
        var files = Directory.EnumerateFiles(directoryPath, "*.xml", SearchOption.TopDirectoryOnly)
            .Where(path => !string.Equals(
                Path.GetFileName(path),
                resultFileName,
                StringComparison.OrdinalIgnoreCase))
            .Take(2)
            .ToArray();

        if (files.Length < 2)
        {
            throw new InvalidOperationException("Not enough XML files found in the directory to perform the task.");
        }

        var resultFilePath = TaskClass.SerializeObjectsInTurns<Car>(files[0], files[1], resultFileName);
        PrintFile(resultFilePath);

        return resultFilePath;
    }

    /// <summary>
    /// Executes Task 3.
    /// </summary>
    /// <param name="filePath">The path of the file to read.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task RunTask3(string filePath)
    {
        try
        {
            string contents = await TaskFileReader.ReadAllTextAsync(filePath);
            Console.WriteLine(contents);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Invalid file path: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Access denied: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"File reading error: {ex.Message}");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("File reading was cancelled.");
        }
    }

    private static void PrintFile(string filePath)
    {
        if (!FileAccessValidator.TryReadFile(filePath, out string contents))
        {
            Console.WriteLine($"Failed to read the file: {filePath}");
            return;
        }

        Console.WriteLine(contents);
    }
}
