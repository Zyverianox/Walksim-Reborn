using GorillaLocomotion;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using WalkSim.Rigging;

namespace WalkSim.Animators;

public class PoseAnimator : AnimatorBase
{
	private Vector3 offsetLeft;

	private Vector3 lookAtLeft = Vector3.forward;

	private Vector3 offsetRight;

	private Vector3 lookAtRight = Vector3.forward;

	private float zRotationLeft;

	private float zRotationRight;

	private HandDriver main;

	private HandDriver secondary;

	private Vector3 eulerAngles;

	public override void Animate()
	{
		rig.headDriver.turn = false;
		AnimateBody();
		AnimateHands();
	}

	private void Update()
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		if (((ButtonControl)Keyboard.current.qKey).wasPressedThisFrame)
		{
			HandDriver handDriver = main;
			main = secondary;
			secondary = handDriver;
		}
		if (((ButtonControl)Keyboard.current.rKey).isPressed)
		{
			RotateHand();
		}
		else
		{
			PositionHand();
		}
		Vector3 val = (main.isLeft ? lookAtLeft : lookAtRight);
		float num = (main.isLeft ? zRotationLeft : zRotationRight);
		float num2 = ((!main.isLeft) ? 1 : (-1));
		main.up = Quaternion.AngleAxis(num * num2, val) * head.up;
		main.trigger = Mouse.current.leftButton.isPressed;
		main.grip = Mouse.current.rightButton.isPressed;
		main.primary = Mouse.current.backButton.isPressed || ((ButtonControl)Keyboard.current.leftBracketKey).isPressed;
		main.secondary = Mouse.current.forwardButton.isPressed || ((ButtonControl)Keyboard.current.rightBracketKey).isPressed;
	}

	private void RotateHand()
	{
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		eulerAngles.x -= ((Vector2)((InputControl<Vector2>)(object)((Pointer)Mouse.current).delta).value).y / 10f;
		if (eulerAngles.x > 180f)
		{
			eulerAngles.x -= 360f;
		}
		eulerAngles.x = Mathf.Clamp(eulerAngles.x, -85f, 85f);
		eulerAngles.y += ((Vector2)((InputControl<Vector2>)(object)((Pointer)Mouse.current).delta).value).x / 10f;
		if (eulerAngles.y > 180f)
		{
			eulerAngles.y -= 360f;
		}
		eulerAngles.y = Mathf.Clamp(eulerAngles.y, -85f, 85f);
		if (main.isLeft)
		{
			lookAtLeft = Quaternion.Euler(eulerAngles) * head.forward;
			zRotationLeft += ((InputControl<Vector2>)(object)Mouse.current.scroll).ReadValue().y / 5f;
		}
		else
		{
			lookAtRight = Quaternion.Euler(eulerAngles) * head.forward;
			zRotationRight += ((InputControl<Vector2>)(object)Mouse.current.scroll).ReadValue().y / 5f;
		}
	}

	private void PositionHand()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = (main.isLeft ? offsetLeft : offsetRight);
		val.z += ((InputControl<Vector2>)(object)Mouse.current.scroll).ReadValue().y / 1000f;
		if (((ButtonControl)Keyboard.current.upArrowKey).wasPressedThisFrame)
		{
			val.z += 0.1f;
		}
		if (((ButtonControl)Keyboard.current.downArrowKey).wasPressedThisFrame)
		{
			val.z -= 0.1f;
		}
		val.z = Mathf.Clamp(val.z, -0.25f, 0.75f);
		val.x += ((InputControl<Vector2>)(object)((Pointer)Mouse.current).delta).ReadValue().x / 1000f;
		val.x = Mathf.Clamp(val.x, -0.5f, 0.5f);
		val.y += ((InputControl<Vector2>)(object)((Pointer)Mouse.current).delta).ReadValue().y / 1000f;
		val.y = Mathf.Clamp(val.y, -0.5f, 0.5f);
		if (main.isLeft)
		{
			offsetLeft = val;
		}
		else
		{
			offsetRight = val;
		}
	}

	private void AnimateBody()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		rig.active = true;
		rig.useGravity = false;
		rig.targetPosition = body.position;
	}

	private void AnimateHands()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		float num = ((!main.isLeft) ? 1 : (-1));
		Vector3 val = (main.isLeft ? offsetLeft : offsetRight);
		Vector3 val2 = (main.isLeft ? lookAtLeft : lookAtRight);
		main.targetPosition = body.TransformPoint(new Vector3(num * 0.2f, 0.1f, 0.3f) + val);
		main.lookAt = main.targetPosition + val2;
		main.hideControllerTransform = false;
	}

	public override void Setup()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		base.Start();
		HeadDriver.Instance.LockCursor = true;
		main = rightHand;
		secondary = leftHand;
		offsetLeft = Vector3.zero;
		lookAtLeft = head.forward;
		offsetRight = Vector3.zero;
		lookAtRight = head.forward;
		secondary.targetPosition = secondary.DefaultPosition + Vector3.up * 0.2f * Player.Instance.scale;
		secondary.lookAt = secondary.targetPosition + head.forward;
		secondary.up = body.right * (float)((!main.isLeft) ? 1 : (-1));
	}
}
