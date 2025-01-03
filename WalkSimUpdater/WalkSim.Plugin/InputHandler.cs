using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.XR;
using WalkSim.Animators;
using WalkSim.Menus;
using WalkSim.Patches;
using WalkSim.Rigging;

namespace WalkSim.Plugin;

public class InputHandler : MonoBehaviour
{
	public static InputHandler Instance;

	public static Vector3 inputDirection;

	public static Vector3 inputDirectionNoY;

	public static string deviceName = "";

	public static string devicePrefix = "";

	private bool Jump => ((ButtonControl)Keyboard.current.spaceKey).wasPressedThisFrame;

	private void Awake()
	{
		Instance = this;
		ValidateDevices();
	}

	private void Update()
	{
		try
		{
			if ((Object)(object)ComputerGUI.Instance != (Object)null && Plugin.Instance.Enabled && !ComputerGUI.Instance.IsInUse)
			{
				GetInputDirection();
				if (((ButtonControl)Keyboard.current.escapeKey).wasPressedThisFrame)
				{
					HeadDriver.Instance.LockCursor = !HeadDriver.Instance.LockCursor;
				}
				if (((ButtonControl)Keyboard.current.cKey).wasPressedThisFrame)
				{
					HeadDriver.Instance.ToggleCam();
				}
				RadialMenu radialMenu = Plugin.Instance.radialMenu;
				((Behaviour)radialMenu).enabled = ((ButtonControl)Keyboard.current.tabKey).isPressed;
				((Component)radialMenu).gameObject.SetActive(((ButtonControl)Keyboard.current.tabKey).isPressed);
				if (((ButtonControl)Keyboard.current.digit1Key).wasPressedThisFrame)
				{
					EnableEmote(EmoteAnimator.Emote.Wave);
				}
				if (((ButtonControl)Keyboard.current.digit2Key).wasPressedThisFrame)
				{
					EnableEmote(EmoteAnimator.Emote.Point);
				}
				if (((ButtonControl)Keyboard.current.digit3Key).wasPressedThisFrame)
				{
					EnableEmote(EmoteAnimator.Emote.ThumbsUp);
				}
				if (((ButtonControl)Keyboard.current.digit4Key).wasPressedThisFrame)
				{
					EnableEmote(EmoteAnimator.Emote.ThumbsDown);
				}
				if (((ButtonControl)Keyboard.current.digit5Key).wasPressedThisFrame)
				{
					EnableEmote(EmoteAnimator.Emote.Shrug);
				}
				if (((ButtonControl)Keyboard.current.digit6Key).wasPressedThisFrame)
				{
					EnableEmote(EmoteAnimator.Emote.Dance);
				}
			}
		}
		catch
		{
		}
	}

	private void EnableEmote(EmoteAnimator.Emote emote)
	{
		EmoteAnimator emoteAnimator = Plugin.Instance.emoteAnimator as EmoteAnimator;
		Rig.Instance.Animator = emoteAnimator;
		emoteAnimator.emote = emote;
	}

	private void GetInputDirection()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = KeyboardInput();
		if (((Vector3)(ref val)).magnitude > 0f)
		{
			inputDirection = ((Vector3)(ref val)).normalized;
			inputDirectionNoY = val;
			inputDirectionNoY.y = 0f;
			((Vector3)(ref inputDirectionNoY)).Normalize();
			return;
		}
		InputDevice leftControllerDevice = ((ControllerInputPoller)ControllerInputPoller.instance).leftControllerDevice;
		InputDevice rightControllerDevice = ((ControllerInputPoller)ControllerInputPoller.instance).rightControllerDevice;
		Vector2 val2 = default(Vector2);
		((InputDevice)(ref leftControllerDevice)).TryGetFeatureValue(CommonUsages.primary2DAxis, ref val2);
		Vector2 val3 = default(Vector2);
		((InputDevice)(ref rightControllerDevice)).TryGetFeatureValue(CommonUsages.primary2DAxis, ref val3);
		float x = val2.x;
		float y = val3.y;
		float y2 = val2.y;
		Vector3 val4 = new Vector3(x, y, y2);
		inputDirection = ((Vector3)(ref val4)).normalized;
		inputDirectionNoY = new Vector3(x, 0f, y2);
		((Vector3)(ref inputDirectionNoY)).Normalize();
	}

	private Vector3 KeyboardInput()
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		if (((ButtonControl)Keyboard.current.aKey).isPressed)
		{
			num -= 1f;
		}
		if (((ButtonControl)Keyboard.current.dKey).isPressed)
		{
			num += 1f;
		}
		if (((ButtonControl)Keyboard.current.sKey).isPressed)
		{
			num2 -= 1f;
		}
		if (((ButtonControl)Keyboard.current.wKey).isPressed)
		{
			num2 += 1f;
		}
		if (Keyboard.current.ctrlKey.isPressed)
		{
			num3 -= 1f;
		}
		if (((ButtonControl)Keyboard.current.spaceKey).isPressed)
		{
			num3 += 1f;
		}
		return new Vector3(num, num3, num2);
	}

	private void ValidateDevices()
	{
		Events.RoomJoined += delegate(object _, Events.RoomJoinedArgs args)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			List<string> list = new List<string>();
			Enumerator<InputDevice> enumerator = InputSystem.devices.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					InputDevice current = enumerator.Current;
					list.Add(((InputControl)current).name);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			string gamemode = args.Gamemode;
			Logging.Debug(args.Gamemode);
			if (!gamemode.Contains(devicePrefix + deviceName) && ((Behaviour)Rig.Instance).enabled)
			{
				while (Application.isFocused)
				{
					GorillaControllerType controllerType = ((ControllerInputPoller)ControllerInputPoller.instance).controllerType;
					if (((object)(GorillaControllerType)(ref controllerType)).Equals((object?)deviceName))
					{
						controllerType = ((ControllerInputPoller)ControllerInputPoller.instance).controllerType;
						deviceName = ((object)(GorillaControllerType)(ref controllerType)).ToString();
					}
				}
			}
		};
	}
}
