using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using WalkSim.Animators;
using WalkSim.Plugin;
using WalkSim.Rigging;

namespace WalkSim.Menus;

public class RadialMenu : MonoBehaviour
{
	public struct Icon
	{
		public Image image;

		public Vector2 direction;

		public AnimatorBase animator;
	}

	public List<Icon> icons;

	private AnimatorBase selectedAnimator;

	private bool cursorWasLocked;

	private bool wasTurning;

	private void Awake()
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		Image component = ((Component)((Component)this).transform.Find("Icons/Walk")).GetComponent<Image>();
		Image component2 = ((Component)((Component)this).transform.Find("Icons/Pose")).GetComponent<Image>();
		Image component3 = ((Component)((Component)this).transform.Find("Icons/Interact")).GetComponent<Image>();
		Image component4 = ((Component)((Component)this).transform.Find("Icons/Fly")).GetComponent<Image>();
		icons = new List<Icon>
		{
			new Icon
			{
				image = component,
				direction = Vector2.up,
				animator = WalkSim.Plugin.Plugin.Instance.walkAnimator
			},
			new Icon
			{
				image = component3,
				direction = Vector2.left,
				animator = WalkSim.Plugin.Plugin.Instance.grabAnimator
			},
			new Icon
			{
				image = component2,
				direction = Vector2.down,
				animator = WalkSim.Plugin.Plugin.Instance.handAnimator
			},
			new Icon
			{
				image = component4,
				direction = Vector2.right,
				animator = WalkSim.Plugin.Plugin.Instance.flyAnimator
			}
		};
	}

	private unsafe void Update()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		Vector2 value = Unsafe.Read<Vector2>((void*)((InputControl<Vector2>)(object)((Pointer)Mouse.current).position).value);
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector((float)(Screen.width / 2), (float)(Screen.height / 2));
		Vector2 val2 = value - val;
		if (((Vector2)(ref val2)).magnitude < (float)Screen.width / 20f)
		{
			return;
		}
		Icon icon = default(Icon);
		float num = float.PositiveInfinity;
		foreach (Icon icon2 in icons)
		{
			float num2 = Vector2.Distance(value - val, icon2.direction);
			if (num2 < num)
			{
				icon = icon2;
				num = num2;
			}
		}
		selectedAnimator = icon.animator;
		foreach (Icon icon3 in icons)
		{
			((Graphic)icon3.image).color = (icon3.Equals(icon) ? Color.white : Color.gray);
			((Component)icon3.image).transform.localScale = Vector3.one * (icon3.Equals(icon) ? 1.5f : 1f);
		}
	}

	private void OnEnable()
	{
		cursorWasLocked = HeadDriver.Instance.LockCursor;
		wasTurning = HeadDriver.Instance.turn;
		HeadDriver.Instance.LockCursor = false;
		HeadDriver.Instance.turn = false;
	}

	private void OnDisable()
	{
		Logging.Debug("RadialMenu disabled");
		HeadDriver.Instance.LockCursor = cursorWasLocked;
		HeadDriver.Instance.turn = wasTurning;
		Rig.Instance.Animator = selectedAnimator;
		Logging.Debug("--Finished");
	}
}
