using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace WalkSim.Tools;

public static class AssetUtils
{
    private static string FormatPath(string path)
    {
        return path.Replace("/", ".").Replace("\\", ".");
    }

    public static AssetBundle LoadAssetBundle(string path)
    {
        path = FormatPath(path);
        using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
        {
            return AssetBundle.LoadFromStream(manifestResourceStream);
        }
    }

    public static string[] GetResourceNames()
    {
        Assembly callingAssembly = Assembly.GetCallingAssembly();
        string[] manifestResourceNames = callingAssembly.GetManifestResourceNames();
        if (manifestResourceNames == null || manifestResourceNames.Length == 0)
        {
            Debug.LogWarning("No manifest resources found.");
            return Array.Empty<string>();
        }
        return manifestResourceNames;
    }
}
