// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Demo;

using Scenarios;

/// <summary>
/// Main entry point of the application, demonstrating the use of Task-based asynchronous programming to run multiple tasks in sequence.
/// </summary>
internal static class Program
{
    private static async Task Main()
    {
        TaskScenario.RunTask1();
        var filePath = TaskScenario.RunTask2();
        await TaskScenario.RunTask3(filePath);
    }
}
