using System;
using System.Diagnostics;
using System.Reflection;
using BepInEx.Logging;

namespace WalkSim.Plugin;

public static class Logging
{
    private static ManualLogSource logger;

    public static int DebuggerLines = 20;

    public static void Init()
    {
        logger = Logger.CreateLogSource("WalkSimulator");
    }

    public static void LogException(Exception e)
    {
        Log(LogLevel.Warning, e.Message, e.StackTrace);
    }

    public static void LogFatal(params object[] content)
    {
        Log(LogLevel.Fatal, content);
    }

    public static void LogWarning(params object[] content)
    {
        Log(LogLevel.Warning, content);
    }

    public static void LogInfo(params object[] content)
    {
        Log(LogLevel.Info, content);
    }

    public static void LogDebug(params object[] content)
    {
        Log(LogLevel.Debug, content);
    }

    private static void Log(LogLevel level, params object[] content)
    {
        MethodBase method = new StackTrace().GetFrame(2).GetMethod();
        logger.Log(level, $"({method.ReflectedType.Name}.{method.Name}()) {string.Join(" ", content)}");
    }

    public static void Debugger(params object[] content)
    {
        LogDebug(content);
    }

    public static string PrependTextToLog(string log, string text)
    {
        log = text + "\n" + log;
        string[] array = log.Split('\n');
        if (array.Length > DebuggerLines)
        {
            log = string.Join("\n", array, 0, DebuggerLines);
        }
        return log;
    }
}
