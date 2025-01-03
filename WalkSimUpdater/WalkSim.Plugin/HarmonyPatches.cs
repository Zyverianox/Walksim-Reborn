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
        if (!IsPatched)
        {
            if (instance == null)
            {
                instance = new Harmony(InstanceId);
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
