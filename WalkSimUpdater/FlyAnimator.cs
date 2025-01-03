using GorillaLocomotion;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using WalkSim.Plugin;
using WalkSim.Rigging;
using WalkSim.Tools;

namespace WalkSim.Animators;

public class FlyAnimator : AnimatorBase
{
	private float speed = 1f;

	private float minSpeed = 0f;

	private float maxSpeed = 5f;

	private int layersBackup;

	private bool noClipActive = false;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		layersBackup = LayerMask.op_Implicit(Player.Instance.locomotionEnabledLayers);
	}

	public override void Animate()
	{
		AnimateBody();
		AnimateHands();
	}

	private void Update()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		speed += ((InputControl<Vector2>)(object)Mouse.current.scroll).ReadValue().y / 1000f;
		speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
		if (((ButtonControl)Keyboard.current.nKey).wasPressedThisFrame)
		{
			noClipActive = !noClipActive;
			Player.Instance.locomotionEnabledLayers = LayerMask.op_Implicit(noClipActive ? 536870912 : layersBackup);
			((Collider)Player.Instance.headCollider).isTrigger = noClipActive;
			((Collider)Player.Instance.bodyCollider).isTrigger = noClipActive;
		}
	}

	private void AnimateBody()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		rig.active = true;
		rig.useGravity = false;
		rig.targetPosition = body.TransformPoint(InputHandler.inputDirection * speed);
	}

	private void AnimateHands()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		leftHand.followRate = (rightHand.followRate = Extensions.Map(speed, minSpeed, maxSpeed, 0f, 1f));
		leftHand.targetPosition = leftHand.DefaultPosition;
		rightHand.targetPosition = rightHand.DefaultPosition;
		leftHand.lookAt = leftHand.targetPosition + body.forward;
		rightHand.lookAt = rightHand.targetPosition + body.forward;
		leftHand.up = body.right;
		rightHand.up = -body.right;
	}

	public override void Cleanup()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		base.Cleanup();
		leftHand.followRate = (rightHand.followRate = 0.1f);
		Player.Instance.locomotionEnabledLayers = LayerMask.op_Implicit(layersBackup);
	}

	public override void Setup()
	{
		HeadDriver.Instance.LockCursor = true;
		HeadDriver.Instance.turn = true;
	}
}
