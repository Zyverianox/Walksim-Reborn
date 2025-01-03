using HarmonyLib;

namespace WalkSim.Patches;

[HarmonyPatch(typeof(GorillaTagger))]
[HarmonyPatch(/*Could not decode attribute arguments.*/)]
internal static class PostInitializedPatch
{
	private static void Postfix()
	{
		UtillaNetworkController.events.TriggerGameInitialized();
	}
}
