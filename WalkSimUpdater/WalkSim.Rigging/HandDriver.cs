using System;
using System.Collections;
using GorillaLocomotion;
using UnityEngine;
using WalkSim.Patches;
using WalkSim.Plugin;

namespace WalkSim.Rigging;

public class HandDriver : MonoBehaviour
{
	public bool grip;

	public bool trigger;

	public bool primary;

	public bool secondary;

	public bool isLeft;

	public bool grounded;

	public bool hideControllerTransform = true;

	public Vector3 targetPosition;

	public Vector3 lookAt;

	public Vector3 up;

	public float followRate = 0.1f;

	public Vector3 hit;

	public Vector3 lastSnap;

	public Vector3 normal;

	private VRMap handMap;

	private Vector3 defaultOffset;

	private Transform head;

	private Transform body;

	private Transform controller;

	public Vector3 DefaultPosition => body.TransformPoint(defaultOffset);

	public void Init(bool isLeft)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		this.isLeft = isLeft;
		defaultOffset = new Vector3(isLeft ? (-0.25f) : 0.25f, -0.45f, 0.2f);
		handMap = (isLeft ? GorillaTagger.Instance.offlineVRRig.leftHand : GorillaTagger.Instance.offlineVRRig.rightHand);
		head = Rig.Instance.head;
		body = Rig.Instance.body;
		controller = (isLeft ? Player.Instance.leftControllerTransform : Player.Instance.rightControllerTransform);
		handMap = (isLeft ? GorillaTagger.Instance.offlineVRRig.leftHand : GorillaTagger.Instance.offlineVRRig.rightHand);
		((Component)this).transform.position = DefaultPosition;
		targetPosition = DefaultPosition;
		lastSnap = DefaultPosition;
		hit = DefaultPosition;
		up = Vector3.up;
	}

	private void FixedUpdate()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.position = Vector3.Lerp(((Component)this).transform.position, targetPosition, followRate);
		((Component)this).transform.LookAt(lookAt, up);
		if (hideControllerTransform)
		{
			controller.position = body.position;
		}
		else
		{
			controller.position = ((Component)this).transform.position;
		}
		if (isLeft)
		{
			FingerPatch.forceLeftGrip = grip;
			FingerPatch.forceLeftPrimary = primary;
			FingerPatch.forceLeftSecondary = secondary;
			FingerPatch.forceLeftTrigger = trigger;
		}
		else
		{
			FingerPatch.forceRightGrip = grip;
			FingerPatch.forceRightPrimary = primary;
			FingerPatch.forceRightSecondary = secondary;
			FingerPatch.forceRightTrigger = trigger;
		}
	}

	private void OnEnable()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)body) && !((Object)(object)Rig.Instance.Animator == (Object)null))
		{
			((Component)this).transform.position = DefaultPosition;
			targetPosition = DefaultPosition;
			try
			{
				handMap.overrideTarget = ((Component)this).transform;
			}
			catch (Exception e)
			{
				Logging.Exception(e);
			}
		}
	}

	private IEnumerator Disable(Action<HandDriver> onDisable)
	{
		((Component)this).transform.position = DefaultPosition;
		yield return (object)new WaitForSeconds(0.1f);
		handMap.overrideTarget = null;
		((Behaviour)this).enabled = false;
		onDisable?.Invoke(this);
	}

	public void Reset()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		grip = (trigger = (primary = false));
		targetPosition = DefaultPosition;
		lookAt = targetPosition + body.forward;
		up = (isLeft ? body.right : (-body.right));
		hideControllerTransform = true;
		grounded = false;
	}
}
