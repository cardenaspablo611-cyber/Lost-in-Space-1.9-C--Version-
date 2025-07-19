using System;
using System.IO;
using UnityEngine;

public static class MoneyLogger
{
    private static string logFilePath;
    private static string logsDirectory;
    private static bool logsDirectoryCreated = false;

    public static void StartNewGameSession()
    {
        // Always create the logs directory if it doesn't exist
        if (!logsDirectoryCreated) {
            logsDirectory = Path.Combine(System.Environment.CurrentDirectory, "Money Logs");
            if (!Directory.Exists(logsDirectory))
                Directory.CreateDirectory(logsDirectory);
            logsDirectoryCreated = true;
        }

        // Always create a new log file with a new timestamp
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        logFilePath = Path.Combine(logsDirectory, $"Money_Log_#{timestamp}.txt");

        addMessageToLog("GAME STARTED");
    }

    public static void Log(string message) {
        if (string.IsNullOrEmpty(logFilePath)) {
            StartNewGameSession();
        }

        addMessageToLog(message);
    }

    public static void LogGameOver() {
        if (string.IsNullOrEmpty(logFilePath)) {
            StartNewGameSession();
        }

        addMessageToLog("GAME OVER");
    }

    private static void addMessageToLog(string message) {
        string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}";
        try {
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        } catch (Exception e) {
            Debug.LogError("Failed to write to log file: " + e.Message);
        }
    }  
}
