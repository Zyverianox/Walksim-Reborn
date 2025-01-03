using HarmonyLib;
using GorillaLocomotion;

namespace WalkSim.Patches
{
    [HarmonyPatch(typeof(GorillaTagger), "Awake")]  // or the appropriate method name to patch
    internal static class PostInitializedPatch
    {
        private static void Postfix()
        {
            UtillaNetworkController.events.TriggerGameInitialized();
        }
    }
}
