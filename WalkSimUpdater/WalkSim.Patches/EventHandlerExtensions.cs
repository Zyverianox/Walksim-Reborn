using System;
using UnityEngine;

namespace WalkSim.Patches;

public static class EventHandlerExtensions
{
	public static void SafeInvoke(this EventHandler handler, object sender, EventArgs e)
	{
		Delegate[] array = handler?.GetInvocationList();
		for (int i = 0; i < array.Length; i++)
		{
			EventHandler eventHandler = (EventHandler)array[i];
			try
			{
				eventHandler?.Invoke(sender, e);
			}
			catch (Exception ex)
			{
				Debug.LogError((object)ex);
			}
		}
	}

	public static void SafeInvoke<T>(this EventHandler<T> handler, object sender, T e) where T : EventArgs
	{
		Delegate[] array = handler?.GetInvocationList();
		for (int i = 0; i < array.Length; i++)
		{
			EventHandler<T> eventHandler = (EventHandler<T>)array[i];
			try
			{
				eventHandler?.Invoke(sender, e);
			}
			catch (Exception ex)
			{
				Debug.LogError((object)ex);
			}
		}
	}
}
