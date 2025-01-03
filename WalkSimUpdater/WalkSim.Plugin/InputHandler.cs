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

    private void Awake()
    {
        Instance = this;
        ValidateDevices();
    }

    private void Update()
    {
        try
        {
            if (ComputerGUI.Instance != null && Plugin.Instance.Enabled && !ComputerGUI.Instance.IsInUse)
            {
                GetInputDirection();
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    HeadDriver.Instance.LockCursor = !HeadDriver.Instance.LockCursor;
                }
                if (Keyboard.current.cKey.wasPressedThisFrame)
                {
                    HeadDriver.Instance.ToggleCam();
                }
                RadialMenu radialMenu = Plugin.Instance.radialMenu;
                radialMenu.enabled = Keyboard.current.tabKey.isPressed;
                radialMenu.gameObject.SetActive(Keyboard.current.tabKey.isPressed);
                if (Keyboard.current.digit1Key.wasPressedThisFrame)
                {
                    EnableEmote(EmoteAnimator.Emote.Wave);
                }
                if (Keyboard.current.digit2Key.wasPressedThisFrame)
                {
                    EnableEmote(EmoteAnimator.Emote.Point);
                }
                if (Keyboard.current.digit3Key.wasPressedThisFrame)
                {
                    EnableEmote(EmoteAnimator.Emote.ThumbsUp);
                }
                if (Keyboard.current.digit4Key.wasPressedThisFrame)
                {
                    EnableEmote(EmoteAnimator.Emote.ThumbsDown);
                }
                if (Keyboard.current.digit5Key.wasPressedThisFrame)
                {
                    EnableEmote(EmoteAnimator.Emote.Shrug);
                }
                if (Keyboard.current.digit6Key.wasPressedThisFrame)
                {
                    EnableEmote(EmoteAnimator.Emote.Dance);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred in InputHandler.Update: {ex}");
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
        Vector3 val = KeyboardInput();
        if (val.magnitude > 0f)
        {
            inputDirection = val.normalized;
            inputDirectionNoY = val;
            inputDirectionNoY.y = 0f;
            inputDirectionNoY.Normalize();
            return;
        }
        InputDevice leftControllerDevice = ControllerInputPoller.instance.leftControllerDevice;
        InputDevice rightControllerDevice = ControllerInputPoller.instance.rightControllerDevice;
        leftControllerDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 val2);
        rightControllerDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 val3);
        float x = val2.x;
        float y = val3.y;
        float y2 = val2.y;
        Vector3 val4 = new Vector3(x, y, y2);
        inputDirection = val4.normalized;
        inputDirectionNoY = new Vector3(x, 0f, y2);
        inputDirectionNoY.Normalize();
    }

    private Vector3 KeyboardInput()
    {
        float num = 0f;
        float num2 = 0f;
        float num3 = 0f;
        if (Keyboard.current.aKey.isPressed)
        {
            num -= 1f;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            num += 1f;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            num2 -= 1f;
        }
        if (Keyboard.current.wKey.isPressed)
        {
            num2 += 1f;
        }
        if (Keyboard.current.ctrlKey.isPressed)
        {
            num3 -= 1f;
        }
        if (Keyboard.current.spaceKey.isPressed)
        {
            num3 += 1f;
        }
        return new Vector3(num, num3, num2);
    }

    private void ValidateDevices()
    {
        Events.RoomJoined += delegate(object _, Events.RoomJoinedArgs args)
        {
            List<string> list = new List<string>();
            foreach (var device in InputSystem.devices)
            {
                list.Add(device.name);
            }
            string gamemode = args.Gamemode;
            Logging.Debug(args.Gamemode);
            if (!gamemode.Contains(devicePrefix + deviceName) && Rig.Instance.enabled)
            {
                while (Application.isFocused)
                {
                    GorillaControllerType controllerType = ControllerInputPoller.instance.controllerType;
                    if (controllerType.Equals(deviceName))
                    {
                        deviceName = controllerType.ToString();
                    }
                }
            }
        };
    }
}
