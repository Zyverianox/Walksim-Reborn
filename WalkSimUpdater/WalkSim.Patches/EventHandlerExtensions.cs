using System;
using UnityEngine;

namespace WalkSim.Patches
{
    public static class EventHandlerExtensions
    {
        public static void SafeInvoke(this EventHandler handler, object sender, EventArgs e)
        {
            if (handler == null) return;

            foreach (Delegate del in handler.GetInvocationList())
            {
                try
                {
                    ((EventHandler)del).Invoke(sender, e);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Exception in event handler: {del.Method.Name}, Exception: {ex}");
                }
            }
        }

        public static void SafeInvoke<T>(this EventHandler<T> handler, object sender, T e) where T : EventArgs
        {
            if (handler == null) return;

            foreach (Delegate del in handler.GetInvocationList())
            {
                try
                {
                    ((EventHandler<T>)del).Invoke(sender, e);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Exception in event handler: {del.Method.Name}, Exception: {ex}");
                }
            }
        }
    }
}
