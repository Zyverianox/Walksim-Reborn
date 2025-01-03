using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using BepInEx;
using UnityEngine;
using WalkSim.Animators;
using WalkSim.Menus;
using WalkSim.Patches;
using WalkSim.Rigging;
using WalkSim.Tools;

namespace WalkSim.Plugin;

[BepInPlugin("com.drperky.gorillatag.walksimpatch", "BetterWalkSimulator", "2.0.1")]
public class Plugin : BaseUnityPlugin
{
	public static Plugin Instance;

	private static bool _enabled;

	public AnimatorBase walkAnimator;

	public AnimatorBase flyAnimator;

	public AnimatorBase emoteAnimator;

	public AnimatorBase handAnimator;

	public AnimatorBase grabAnimator;

	public ComputerGUI computerGUI;

	public AssetBundle bundle;

	public RadialMenu radialMenu;

	public Dictionary<string, float> sliders = new Dictionary<string, float>();

	public Dictionary<string, string> labels = new Dictionary<string, string>();

	public static bool IsSteam { get; protected set; }

	public bool Enabled
	{
		get
		{
			return _enabled;
		}
		protected set
		{
			_enabled = value;
			if (value)
			{
				((Component)this).gameObject.GetOrAddComponent<InputHandler>();
				((Component)this).gameObject.GetOrAddComponent<Rig>();
				walkAnimator = ((Component)this).gameObject.GetOrAddComponent<WalkAnimator>();
				flyAnimator = ((Component)this).gameObject.GetOrAddComponent<FlyAnimator>();
				emoteAnimator = ((Component)this).gameObject.GetOrAddComponent<EmoteAnimator>();
				handAnimator = ((Component)this).gameObject.GetOrAddComponent<PoseAnimator>();
				grabAnimator = ((Component)this).gameObject.GetOrAddComponent<InteractAnimator>();
				if (!Object.op_Implicit((Object)(object)radialMenu))
				{
					radialMenu = Object.Instantiate<GameObject>(bundle.LoadAsset<GameObject>("Radial Menu")).AddComponent<RadialMenu>();
				}
				this.computerGUI = ((Component)this).gameObject.GetOrAddComponent<ComputerGUI>();
				((Behaviour)walkAnimator).enabled = false;
				((Behaviour)flyAnimator).enabled = false;
				((Behaviour)emoteAnimator).enabled = false;
				((Behaviour)handAnimator).enabled = false;
				((Behaviour)grabAnimator).enabled = false;
				return;
			}
			InputHandler instance = InputHandler.Instance;
			if ((Object)(object)instance != (Object)null)
			{
				((Component)(object)instance).Obliterate();
			}
			Rig instance2 = Rig.Instance;
			if ((Object)(object)instance2 != (Object)null)
			{
				((Component)(object)instance2).Obliterate();
			}
			AnimatorBase animatorBase = walkAnimator;
			if ((Object)(object)animatorBase != (Object)null)
			{
				((Component)(object)animatorBase).Obliterate();
			}
			AnimatorBase animatorBase2 = flyAnimator;
			if ((Object)(object)animatorBase2 != (Object)null)
			{
				((Component)(object)animatorBase2).Obliterate();
			}
			AnimatorBase animatorBase3 = emoteAnimator;
			if ((Object)(object)animatorBase3 != (Object)null)
			{
				((Component)(object)animatorBase3).Obliterate();
			}
			AnimatorBase animatorBase4 = handAnimator;
			if ((Object)(object)animatorBase4 != (Object)null)
			{
				((Component)(object)animatorBase4).Obliterate();
			}
			AnimatorBase animatorBase5 = grabAnimator;
			if ((Object)(object)animatorBase5 != (Object)null)
			{
				((Component)(object)animatorBase5).Obliterate();
			}
			ComputerGUI computerGUI = this.computerGUI;
			if ((Object)(object)computerGUI != (Object)null)
			{
				((Component)(object)computerGUI).Obliterate();
			}
		}
	}

	private void Awake()
	{
		Instance = this;
		Logging.Init();
		try
		{
			string path = Paths.ConfigPath + "/BepInEx.cfg";
			string input = File.ReadAllText(path);
			input = Regex.Replace(input, "HideManagerGameObject = .+", "HideManagerGameObject = true");
			File.WriteAllText(path, input);
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
		bundle = AssetUtils.LoadAssetBundle("WalkSim/Resources/WalkSimulator");
	}

	private void Start()
	{
		Events.GameInitialized += OnGameInitialized;
		Events.RoomJoined += OnRoomJoined;
		Events.RoomLeft += OnRoomLeft;
	}

	private void OnRoomLeft(object sender, Events.RoomJoinedArgs e)
	{
		Enabled = true;
	}

	private void OnRoomJoined(object sender, Events.RoomJoinedArgs args)
	{
		Enabled = true;
		Debug.Log((object)"rad's walksim unlock");
	}

	private void OnEnable()
	{
		HarmonyPatches.ApplyHarmonyPatches();
	}

	private void OnDisable()
	{
		HarmonyPatches.RemoveHarmonyPatches();
	}

	private void OnGameInitialized(object sender, EventArgs e)
	{
		IsSteam = true;
		Enabled = true;
	}

	private void FixedUpdate()
	{
		TryInitializeGamemode();
	}

	public void TryInitializeGamemode()
	{
	}

	private void OnGUI()
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		float num = 400f;
		float num2 = 40f;
		float num3 = (float)Screen.width - num - 10f;
		float num4 = 10f;
		GUI.skin.label.fontSize = 20;
		foreach (string item in new List<string>(labels.Keys))
		{
			num4 += num2;
			GUI.Label(new Rect(num3, num4, num, num2), item + ": " + labels[item]);
		}
		foreach (string item2 in new List<string>(sliders.Keys))
		{
			num4 += num2;
			sliders[item2] = GUI.HorizontalSlider(new Rect(num3, num4, num, num2), sliders[item2], 0f, 10f);
			GUI.Label(new Rect(num3 - num, num4, num, num2), item2 + ": " + sliders[item2]);
		}
	}

	static Plugin()
	{
		_enabled = true;
	}
}
