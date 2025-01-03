using System;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using WalkSim.Plugin;
using WalkSim.Rigging;
using WalkSim.Tools;

namespace WalkSim.Animators;

public class WalkAnimator : AnimatorBase
{
	private float speed = 10f;

	private float height = 0.35f;

	private float targetHeight;

	private bool hasJumped;

	private bool onJumpCooldown;

	private float jumpTime;

	private float walkCycleTime = 0f;

	private bool IsSprinting => ((ButtonControl)Keyboard.current.leftShiftKey).isPressed;

	private bool NotMoving => InputHandler.inputDirectionNoY == Vector3.zero;

	public override void Animate()
	{
		MoveBody();
		AnimateHands();
	}

	private void Update()
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		if (WalkSim.Plugin.Plugin.Instance.Enabled)
		{
			if (!hasJumped && rig.onGround && ((ButtonControl)Keyboard.current.spaceKey).wasPressedThisFrame)
			{
				hasJumped = true;
				onJumpCooldown = true;
				jumpTime = Time.time;
				rig.active = false;
				rigidbody.AddForce(Vector3.up * 250f * Player.Instance.scale, (ForceMode)1);
			}
			if ((hasJumped && !rig.onGround) || Time.time - jumpTime > 1f)
			{
				onJumpCooldown = false;
			}
			if (rig.onGround && !onJumpCooldown)
			{
				hasJumped = false;
			}
		}
	}

	public void MoveBody()
	{
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		rig.active = rig.onGround && !hasJumped;
		rig.useGravity = !rig.onGround;
		if (rig.onGround)
		{
			float num;
			float num2;
			float num3;
			if (NotMoving)
			{
				num = 0.5f;
				num2 = 0.55f;
				num3 = Time.time * MathF.PI * 2f;
			}
			else
			{
				num = 0.3f;
				num2 = 0.8f;
				num3 = walkCycleTime * MathF.PI * 2f;
			}
			if (Keyboard.current.ctrlKey.isPressed)
			{
				num -= 0.3f;
				num2 -= 0.3f;
			}
			targetHeight = Extensions.Map(Mathf.Sin(num3), -1f, 1f, num, num2);
			height = targetHeight;
			Vector3 val = rig.lastGroundPosition + Vector3.up * height * Player.Instance.scale;
			Vector3 val2 = body.TransformDirection(InputHandler.inputDirectionNoY);
			val2.y = 0f;
			if (Vector3.Dot(rig.lastNormal, Vector3.up) > 0.3f)
			{
				val2 = Vector3.ProjectOnPlane(val2, rig.lastNormal);
			}
			val2 *= Player.Instance.scale;
			float num4 = (IsSprinting ? (speed * 3f) : speed);
			val += val2 * num4 / 10f;
			rig.targetPosition = val;
		}
	}

	private void AnimateHands()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		leftHand.lookAt = leftHand.targetPosition + body.forward;
		rightHand.lookAt = rightHand.targetPosition + body.forward;
		leftHand.up = body.right;
		rightHand.up = -body.right;
		if (!rig.onGround)
		{
			leftHand.grounded = false;
			rightHand.grounded = false;
			Vector3 val = Vector3.up * 0.2f * Player.Instance.scale;
			leftHand.targetPosition = leftHand.DefaultPosition;
			rightHand.targetPosition = rightHand.DefaultPosition + val;
			return;
		}
		UpdateHitInfo(leftHand);
		UpdateHitInfo(rightHand);
		if (NotMoving)
		{
			leftHand.targetPosition = leftHand.hit;
			rightHand.targetPosition = rightHand.hit;
			return;
		}
		if (!leftHand.grounded && !rightHand.grounded)
		{
			leftHand.grounded = true;
			leftHand.lastSnap = leftHand.hit;
			leftHand.targetPosition = leftHand.hit;
			rightHand.lastSnap = rightHand.hit;
			rightHand.targetPosition = rightHand.hit;
		}
		AnimateHand(leftHand, rightHand);
		AnimateHand(rightHand, leftHand);
	}

	private void UpdateHitInfo(HandDriver hand)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		float scale = Player.Instance.scale;
		float num = 0.5f * scale;
		Vector3 smoothedGroundPosition = rig.smoothedGroundPosition;
		Vector3 lastNormal = rig.lastNormal;
		float x = Mathf.Abs(Vector3.Dot(InputHandler.inputDirectionNoY, Vector3.forward));
		float num2 = Extensions.Map(x, 0f, 1f, 0.4f, 0.5f);
		Vector3 val = body.TransformDirection(InputHandler.inputDirectionNoY * num2);
		val.y = 0f;
		val *= scale;
		Vector3 val2 = Vector3.ProjectOnPlane(hand.DefaultPosition - smoothedGroundPosition + val, lastNormal);
		val2 += smoothedGroundPosition + lastNormal * 0.3f * scale;
		RaycastHit val3 = default(RaycastHit);
		if (!Physics.Raycast(val2, -lastNormal, ref val3, num, LayerMask.op_Implicit(Player.Instance.locomotionEnabledLayers)))
		{
			if (NotMoving)
			{
				hand.targetPosition = hand.DefaultPosition;
			}
		}
		else
		{
			hand.hit = ((RaycastHit)(ref val3)).point;
			hand.normal = ((RaycastHit)(ref val3)).normal;
			hand.lookAt = ((Component)hand).transform.position + body.forward;
		}
	}

	private void AnimateHand(HandDriver hand, HandDriver otherHand)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		float x = Mathf.Abs(Vector3.Dot(InputHandler.inputDirectionNoY, Vector3.forward));
		float x2 = Vector3.Dot(rig.lastNormal, Vector3.up);
		float num = Extensions.Map(x2, 0f, 1f, 0.1f, 0.6f);
		float num2 = Extensions.Map(x, 0f, 1f, 0.5f, 1.25f);
		num2 *= num * Player.Instance.scale;
		float num3 = 0.2f * Player.Instance.scale;
		float num4 = otherHand.hit.Distance(otherHand.lastSnap) / num2;
		if (otherHand.grounded && num4 >= 1f)
		{
			hand.targetPosition = hand.hit;
			hand.lastSnap = hand.hit;
			hand.grounded = true;
			otherHand.grounded = false;
		}
		else if (otherHand.grounded)
		{
			walkCycleTime = num4;
			hand.targetPosition = Vector3.Slerp(hand.lastSnap, hand.hit, walkCycleTime);
			hand.targetPosition += hand.normal * num3 * Mathf.Sin(walkCycleTime);
			hand.grounded = false;
		}
		if (hand.targetPosition.Distance(hand.DefaultPosition) > 1f)
		{
			hand.targetPosition = hand.DefaultPosition;
		}
	}

	public override void Setup()
	{
		HeadDriver.Instance.LockCursor = true;
		HeadDriver.Instance.turn = true;
	}
}
