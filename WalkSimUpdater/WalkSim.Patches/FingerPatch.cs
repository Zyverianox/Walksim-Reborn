using HarmonyLib;
using WalkSim.Plugin;

namespace WalkSim.Patches
{
    [HarmonyPatch(typeof(ControllerInputPoller), "Update")]
    public class FingerPatch
    {
        public static bool forceLeftGrip;
        public static bool forceRightGrip;
        public static bool forceLeftTrigger;
        public static bool forceRightTrigger;
        public static bool forceLeftPrimary;
        public static bool forceRightPrimary;
        public static bool forceLeftSecondary;
        public static bool forceRightSecondary;

        private static void Postfix(ControllerInputPoller __instance)
        {
            if (!Plugin.Instance.Enabled) return;

            UpdateControllerState(__instance, forceLeftGrip, forceLeftTrigger, forceLeftPrimary, forceLeftSecondary, true);
            UpdateControllerState(__instance, forceRightGrip, forceRightTrigger, forceRightPrimary, forceRightSecondary, false);
        }

        private static void UpdateControllerState(ControllerInputPoller instance, bool grip, bool trigger, bool primary, bool secondary, bool isLeft)
        {
            if (grip)
            {
                if (isLeft)
                {
                    instance.leftControllerGripFloat = 1f;
                    instance.leftGrab = true;
                    instance.leftGrabRelease = false;
                }
                else
                {
                    instance.rightControllerGripFloat = 1f;
                    instance.rightGrab = true;
                    instance.rightGrabRelease = false;
                }
            }

            if (trigger)
            {
                if (isLeft)
                {
                    instance.leftControllerIndexFloat = 1f;
                }
                else
                {
                    instance.rightControllerIndexFloat = 1f;
                }
            }

            if (primary)
            {
                if (isLeft)
                {
                    instance.leftControllerPrimaryButton = true;
                }
                else
                {
                    instance.rightControllerPrimaryButton = true;
                }
            }

            if (secondary)
            {
                if (isLeft)
                {
                    instance.leftControllerSecondaryButton = true;
                }
                else
                {
                    instance.rightControllerSecondaryButton = true;
                }
            }
        }
    }
}
