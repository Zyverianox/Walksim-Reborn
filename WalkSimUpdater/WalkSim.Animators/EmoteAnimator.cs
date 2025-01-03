using System;
using UnityEngine;
using WalkSim.Plugin;
using WalkSim.Rigging;

namespace WalkSim.Animators;

public class EmoteAnimator : AnimatorBase
{
	public enum Emote
	{
		Wave,
		Point,
		ThumbsUp,
		ThumbsDown,
		Shrug,
		Dance
	}

	private struct HandPositionInfo
	{
		public Vector3 position;

		public Vector3 lookAt;

		public Vector3 up;

		public bool grip;

		public bool trigger;

		public bool thumb;

		public bool used;
	}

	public Emote emote;

	private float startTime;

	private Func<HandDriver, float, HandPositionInfo> handPositioner;

	private Vector3 startingPosition;

	private float lastDanceSwitch;

	private float danceSwitchRate = 1.5f;

	private int dance;

	public override void Animate()
	{
		switch (emote)
		{
		case Emote.Wave:
			handPositioner = WavePositioner;
			break;
		case Emote.Point:
			handPositioner = PointPositioner;
			break;
		case Emote.ThumbsUp:
			handPositioner = ThumbsUpPositioner;
			break;
		case Emote.ThumbsDown:
			handPositioner = ThumbsDownPositioner;
			break;
		case Emote.Shrug:
			handPositioner = ShrugPositioner;
			break;
		case Emote.Dance:
			handPositioner = DancePositioner;
			break;
		}
		AnimateBody();
		AnimateHand(leftHand);
		AnimateHand(rightHand);
	}

	private void AnimateBody()
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		rig.active = true;
		rig.useGravity = false;
		if (emote == Emote.Dance)
		{
			float num = Time.time - startTime;
			Vector3 val = default(Vector3);
			((Vector3)(ref val))._002Ector(0f, Mathf.Sin(num * 10f) * 0.1f, 0f);
			rig.targetPosition = startingPosition + val;
		}
		else
		{
			rig.targetPosition = startingPosition;
		}
	}

	private void AnimateHand(HandDriver hand)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		HandPositionInfo handPositionInfo = handPositioner(hand, Time.time - startTime);
		if (handPositionInfo.used)
		{
			hand.grip = handPositionInfo.grip;
			hand.trigger = handPositionInfo.trigger;
			hand.primary = handPositionInfo.thumb;
			hand.targetPosition = handPositionInfo.position;
			hand.lookAt = handPositionInfo.lookAt;
			hand.up = handPositionInfo.up;
		}
	}

	private HandPositionInfo DancePositioner(HandDriver hand, float t)
	{
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		if (Time.time - lastDanceSwitch > danceSwitchRate)
		{
			dance = Random.Range(0, 4);
			lastDanceSwitch = Time.time;
		}
		HandPositionInfo result = default(HandPositionInfo);
		float num = (hand.isLeft ? 0f : MathF.PI);
		switch (dance)
		{
		case 0:
		{
			Vector3 lookAt2 = (hand.isLeft ? (hand.targetPosition + head.right + head.forward) : (hand.targetPosition - head.right + head.forward));
			Vector3 val2 = default(Vector3);
			((Vector3)(ref val2))._002Ector(hand.isLeft ? (-0.2f) : 0.2f, -0.3f, 0.2f);
			val2.y += Mathf.Sin(t * 10f + num) * 0.1f;
			HandPositionInfo result2 = default(HandPositionInfo);
			result2.position = head.TransformPoint(val2);
			result2.lookAt = lookAt2;
			result2.up = head.up;
			result2.grip = true;
			result2.trigger = true;
			result2.thumb = true;
			result2.used = true;
			return result2;
		}
		case 1:
		{
			Vector3 lookAt4 = hand.targetPosition + head.up;
			Vector3 val4 = default(Vector3);
			((Vector3)(ref val4))._002Ector(hand.isLeft ? (-0.2f) : 0.2f, -0.2f, 0.3f);
			val4.z += Mathf.Sin(t * 10f + num) * 0.1f;
			HandPositionInfo result2 = default(HandPositionInfo);
			result2.position = head.TransformPoint(val4);
			result2.lookAt = lookAt4;
			result2.up = (hand.isLeft ? head.right : (-head.right));
			result2.grip = false;
			result2.trigger = false;
			result2.thumb = false;
			result2.used = true;
			return result2;
		}
		case 2:
		{
			Vector3 lookAt3 = hand.targetPosition + head.up;
			Vector3 val3 = default(Vector3);
			((Vector3)(ref val3))._002Ector(hand.isLeft ? (-0.2f) : 0.2f, -0.3f, 0.2f);
			val3.y += Mathf.Sin(t * 10f + num) * 0.1f;
			HandPositionInfo result2 = default(HandPositionInfo);
			result2.position = head.TransformPoint(val3);
			result2.lookAt = lookAt3;
			result2.up = (hand.isLeft ? head.right : (-head.right));
			result2.grip = true;
			result2.trigger = false;
			result2.thumb = true;
			result2.used = true;
			return result2;
		}
		case 3:
		{
			Vector3 lookAt = hand.targetPosition + head.up;
			Vector3 val = default(Vector3);
			((Vector3)(ref val))._002Ector(hand.isLeft ? (-0.2f) : 0.2f, -0.2f, 0.3f);
			val.x += Mathf.Cos(t * 10f) * 0.1f;
			val.z += Mathf.Sin(t * 10f) * 0.1f;
			HandPositionInfo result2 = default(HandPositionInfo);
			result2.position = head.TransformPoint(val);
			result2.lookAt = lookAt;
			result2.up = (hand.isLeft ? head.right : (-head.right));
			result2.grip = true;
			result2.trigger = true;
			result2.thumb = true;
			result2.used = true;
			return result2;
		}
		default:
			return result;
		}
	}

	private HandPositionInfo ThumbsDownPositioner(HandDriver hand, float _)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		Vector3 lookAt = (hand.isLeft ? (hand.targetPosition + head.right) : (hand.targetPosition - head.right));
		HandPositionInfo result = default(HandPositionInfo);
		result.position = head.TransformPoint(new Vector3(hand.isLeft ? (-0.2f) : 0.2f, 0f, 0.4f));
		result.lookAt = lookAt;
		result.up = -head.up;
		result.grip = true;
		result.trigger = true;
		result.used = true;
		return result;
	}

	private HandPositionInfo ThumbsUpPositioner(HandDriver hand, float _)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		Vector3 lookAt = (hand.isLeft ? (hand.targetPosition + head.right) : (hand.targetPosition - head.right));
		HandPositionInfo result = default(HandPositionInfo);
		result.position = head.TransformPoint(new Vector3(hand.isLeft ? (-0.2f) : 0.2f, 0f, 0.4f));
		result.lookAt = lookAt;
		result.up = head.up;
		result.grip = true;
		result.trigger = true;
		result.used = true;
		return result;
	}

	private HandPositionInfo ShrugPositioner(HandDriver hand, float _)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		Vector3 lookAt = (hand.isLeft ? (hand.targetPosition - head.right + head.forward) : (hand.targetPosition + head.right + head.forward));
		HandPositionInfo result = default(HandPositionInfo);
		result.position = body.TransformPoint(new Vector3(hand.isLeft ? (-0.4f) : 0.4f, 0f, 0.2f));
		result.lookAt = lookAt;
		result.up = -head.forward;
		result.used = true;
		return result;
	}

	private HandPositionInfo PointPositioner(HandDriver hand, float __)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		HandPositionInfo result;
		if (hand.isLeft)
		{
			result = default(HandPositionInfo);
			result.used = true;
			result.position = hand.DefaultPosition;
			result.lookAt = hand.targetPosition + head.forward;
			result.up = head.right;
			return result;
		}
		result = default(HandPositionInfo);
		result.position = head.TransformPoint(new Vector3(0.25f, 0f, 0.7f));
		result.lookAt = hand.targetPosition + head.forward;
		result.up = -head.right;
		result.grip = true;
		result.trigger = false;
		result.used = true;
		return result;
	}

	private HandPositionInfo WavePositioner(HandDriver hand, float time)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		HandPositionInfo result;
		if (hand.isLeft)
		{
			result = default(HandPositionInfo);
			result.used = true;
			result.position = hand.DefaultPosition;
			result.lookAt = hand.targetPosition + head.forward;
			result.up = head.right;
			return result;
		}
		Vector3 val = default(Vector3);
		((Vector3)(ref val))._002Ector(0.25f, 0f, 0.2f);
		float num = 0.25f;
		float num2 = 0.25f;
		float num3 = 5f;
		float num4 = Mathf.Sin(Time.time * num3);
		float num5 = Mathf.Cos(Time.time * num3);
		Vector3 zero = Vector3.zero;
		zero.x = num4 * num;
		zero.y = Mathf.Abs(num5) * num2;
		zero += val;
		Vector3 val2 = hand.targetPosition - head.TransformPoint(val - Vector3.up * 0.25f);
		result = default(HandPositionInfo);
		result.position = head.TransformPoint(zero);
		result.lookAt = hand.targetPosition + val2;
		result.up = -head.right;
		result.used = true;
		return result;
	}

	public override void Setup()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		Logging.Debug("===SETUP===");
		base.Start();
		startingPosition = body.position;
	}
}
