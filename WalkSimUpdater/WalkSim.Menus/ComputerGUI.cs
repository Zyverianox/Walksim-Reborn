using System;
using System.Collections.Generic;
using Cinemachine;
using GorillaLocomotion;
using GorillaNetworking;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using WalkSim.Animators;
using WalkSim.Plugin;
using WalkSim.Rigging;
using WalkSim.Tools;
using WalkSimulatorClassic;

namespace WalkSim.Menus;

public class ComputerGUI : MonoBehaviour
{
    private static float PcRange;

    public static ComputerGUI Instance;

    private GorillaComputerTerminal[] terminals = new GorillaComputerTerminal[0];

    private AnimatorBase cachedAnimator;

    private Transform currentTerminal;

    private Camera overrideCam;

    private bool inRange;

    private Dictionary<KeyControl, GorillaKeyboardBindings> buttonMapping = new Dictionary<KeyControl, GorillaKeyboardBindings>();

    private Dictionary<GorillaKeyboardBindings, Key> keyMapping = new Dictionary<GorillaKeyboardBindings, Key>
    {
        { (GorillaKeyboardBindings)17, (Key)15 },
        { (GorillaKeyboardBindings)18, (Key)16 },
        { (GorillaKeyboardBindings)19, (Key)17 },
        { (GorillaKeyboardBindings)20, (Key)18 },
        { (GorillaKeyboardBindings)21, (Key)19 },
        { (GorillaKeyboardBindings)22, (Key)20 },
        { (GorillaKeyboardBindings)23, (Key)21 },
        { (GorillaKeyboardBindings)24, (Key)22 },
        { (GorillaKeyboardBindings)25, (Key)23 },
        { (GorillaKeyboardBindings)26, (Key)24 },
        { (GorillaKeyboardBindings)27, (Key)25 },
        { (GorillaKeyboardBindings)28, (Key)26 },
        { (GorillaKeyboardBindings)29, (Key)27 },
        { (GorillaKeyboardBindings)30, (Key)28 },
        { (GorillaKeyboardBindings)31, (Key)29 },
        { (GorillaKeyboardBindings)32, (Key)30 },
        { (GorillaKeyboardBindings)33, (Key)31 },
        { (GorillaKeyboardBindings)34, (Key)32 },
        { (GorillaKeyboardBindings)35, (Key)33 },
        { (GorillaKeyboardBindings)36, (Key)34 },
        { (GorillaKeyboardBindings)37, (Key)35 },
        { (GorillaKeyboardBindings)38, (Key)36 },
        { (GorillaKeyboardBindings)39, (Key)37 },
        { (GorillaKeyboardBindings)40, (Key)38 },
        { (GorillaKeyboardBindings)41, (Key)39 },
        { (GorillaKeyboardBindings)42, (Key)40 },
        { (GorillaKeyboardBindings)0, (Key)50 },
        { (GorillaKeyboardBindings)1, (Key)41 },
        { (GorillaKeyboardBindings)2, (Key)42 },
        { (GorillaKeyboardBindings)3, (Key)43 },
        { (GorillaKeyboardBindings)4, (Key)44 },
        { (GorillaKeyboardBindings)5, (Key)45 },
        { (GorillaKeyboardBindings)6, (Key)46 },
        { (GorillaKeyboardBindings)7, (Key)47 },
        { (GorillaKeyboardBindings)8, (Key)48 },
        { (GorillaKeyboardBindings)9, (Key)49 },
        { (GorillaKeyboardBindings)14, (Key)94 },
        { (GorillaKeyboardBindings)15, (Key)95 },
        { (GorillaKeyboardBindings)16, (Key)96 },
        { (GorillaKeyboardBindings)13, (Key)2 },
        { (GorillaKeyboardBindings)12, (Key)65 },
        { (GorillaKeyboardBindings)10, (Key)63 },
        { (GorillaKeyboardBindings)11, (Key)64 }
    };

    public bool IsInUse => overrideCam.enabled;

    private bool IsInRange()
    {
        Vector3 position = Player.Instance.bodyCollider.transform.position;
        bool result;
        if (Vector3.Distance(position, GorillaComputer.instance.transform.position) < PcRange)
        {
            currentTerminal = GorillaComputer.instance.transform;
            result = true;
        }
        else
        {
            foreach (GorillaComputerTerminal terminal in terminals)
            {
                if (Vector3.Distance(position, terminal.transform.position) < PcRange)
                {
                    currentTerminal = terminal.transform;
                    return true;
                }
            }
            currentTerminal = null;
            result = false;
        }
        return result;
    }

    private void Awake()
    {
        Instance = this;
        GameObject cameraObject = new GameObject("WalkSim First Person Camera");
        Cinemachine3rdPersonFollow follow = FindObjectOfType<Cinemachine3rdPersonFollow>();
        Camera parentCamera = follow.gameObject.GetComponentInParent<Camera>();
        overrideCam = cameraObject.AddComponent<Camera>();
        overrideCam.fieldOfView = 90f;
        overrideCam.nearClipPlane = parentCamera.nearClipPlane;
        overrideCam.farClipPlane = parentCamera.farClipPlane;
        overrideCam.targetDisplay = parentCamera.targetDisplay;
        overrideCam.cullingMask = parentCamera.cullingMask;
        overrideCam.depth = parentCamera.depth + 1f;
        overrideCam.targetDisplay = parentCamera.targetDisplay;
        overrideCam.enabled = false;
    }

    private void Start()
    {
        BuildButtonMap();
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && inRange && !overrideCam.enabled)
        {
            overrideCam.enabled = true;
            cachedAnimator = Rig.Instance.Animator;
            Rig.Instance.Animator = null;
        }
        else
        {
            if (!overrideCam.enabled) return;

            if (!Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                foreach (KeyControl key in buttonMapping.Keys)
                {
                    try
                    {
                        if (key == null)
                        {
                            Logging.Debug("Key is null");
                            continue;
                        }
                        if (key.wasPressedThisFrame)
                        {
                            Logging.Debug("Pressed key: " + key.name);
                            GorillaComputer.instance.PressButton(buttonMapping[key]);
                            Sounds.Play(66, 0.5f);
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.Exception(e);
                    }
                }
                return;
            }
            overrideCam.enabled = false;
            Rig.Instance.Animator = cachedAnimator;
        }
    }

    private void FixedUpdate()
    {
        if (Time.frameCount % 60 == 0)
        {
            inRange = IsInRange();
        }
        if (!inRange && Time.frameCount % 600 == 0)
        {
            terminals = FindObjectsOfType<GorillaComputerTerminal>();
        }
        if (inRange)
        {
            Transform transform = Traverse.Create(GorillaComputer.instance.screenText).Field("text").GetValue<Text>().transform;
            overrideCam.transform.position = transform.TransformPoint(Vector3.forward * -0.3f);
            overrideCam.transform.LookAt(transform);
        }
        else
        {
            overrideCam.enabled = false;
        }
    }

    private void OnGUI()
    {
        string text = inRange ? (overrideCam.enabled ? "Press [Escape] to exit" : "Press [E] to use computer") : "";
        GUI.Label(new Rect(20f, 20f, 200f, 200f), text, new GUIStyle
        {
            fontSize = 20,
            normal = new GUIStyleState
            {
                textColor = Color.white
            }
        });
    }

    private void BuildButtonMap()
    {
        foreach (var item in keyMapping)
        {
            buttonMapping.Add(Keyboard.current[item.Value], item.Key);
        }
    }

    static ComputerGUI()
    {
        PcRange = 4f;
    }
}
