using System;
using UnityEngine;

public static class MoneyLogger
{
    // Session-only logging to Unity Console instead of files for WebGL compatibility
    // Logs will appear in browser console for WebGL builds
    private static bool sessionStarted = false;

    public static void StartNewGameSession()
    {
        if (!sessionStarted) {
            sessionStarted = true;
            Log("GAME SESSION STARTED");
        }
    }

    public static void Log(string message) {
        if (!sessionStarted) {
            StartNewGameSession();
        }
        
        // Log to Unity console (appears in browser console for WebGL)
        string logEntry = $"[MoneyLogger {DateTime.Now:HH:mm:ss}] {message}";
        Debug.Log(logEntry);
    }

    public static void LogGameOver() {
        if (!sessionStarted) {
            StartNewGameSession();
        }
        
        Log("GAME OVER");
    }
}
