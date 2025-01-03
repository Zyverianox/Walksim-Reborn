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
		Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
		AssetBundle result = AssetBundle.LoadFromStream(manifestResourceStream);
		manifestResourceStream.Close();
		return result;
	}

	public static string[] GetResourceNames()
	{
		Assembly callingAssembly = Assembly.GetCallingAssembly();
		string[] manifestResourceNames = callingAssembly.GetManifestResourceNames();
		if (manifestResourceNames == null)
		{
			Console.WriteLine("No manifest resources found.");
			return null;
		}
		return manifestResourceNames;
	}
}
