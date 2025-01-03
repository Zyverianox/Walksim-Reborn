using System.Reflection;
using HarmonyLib;

namespace WalkSim.Plugin;

public class HarmonyPatches
{
	private static Harmony instance;

	public const string InstanceId = "com.kylethescientist.gorillatag.walksimulator";

	public static bool IsPatched { get; private set; }

	internal static void ApplyHarmonyPatches()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		if (!IsPatched)
		{
			if (instance == null)
			{
				instance = new Harmony("com.kylethescientist.gorillatag.walksimulator");
			}
			instance.PatchAll(Assembly.GetExecutingAssembly());
			IsPatched = true;
		}
	}

	internal static void RemoveHarmonyPatches()
	{
		if (instance != null && IsPatched)
		{
			instance.UnpatchSelf();
			IsPatched = false;
		}
	}
}
