using System.Collections;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.InputSystem;
using WalkSim.Rigging;
using WalkSim.Tools;

namespace WalkSim.Animators;

public class InteractAnimator : AnimatorBase
{
	public enum State
	{
		IDLE,
		WAIT,
		BUTTON,
		GRAB
	}

	private Transform reticle;

	private State state;

	public override void Animate()
	{
		((Component)reticle).gameObject.SetActive(true);
		HeadDriver.Instance.FirstPerson = true;
		AnimateBody();
		AnimateHands();
	}

	private void Update()
	{
		if (state <= State.IDLE)
		{
			if (Mouse.current.leftButton.isPressed)
			{
				state = State.WAIT;
				((MonoBehaviour)this).StartCoroutine(Raycast(leftHand));
			}
			else if (Mouse.current.rightButton.isPressed)
			{
				state = State.WAIT;
				((MonoBehaviour)this).StartCoroutine(Raycast(rightHand));
			}
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

	private IEnumerator Raycast(HandDriver main)
	{
		Ray ray = new Ray(((Component)Camera.main).transform.position, reticle.position - ((Component)Camera.main).transform.position);
		int buttonLayer = LayerMask.GetMask(new string[2] { "GorillaInteractable", "TransparentFX" });
		RaycastHit[] hits = Physics.RaycastAll(ray, 0.82f, buttonLayer);
		RaycastHit[] array = hits;
		for (int i = 0; i < array.Length; i++)
		{
			RaycastHit hit = array[i];
			if (Object.op_Implicit((Object)(object)((Component)((RaycastHit)(ref hit)).transform).GetComponent<GorillaPressableButton>()) || Object.op_Implicit((Object)(object)((Component)((RaycastHit)(ref hit)).transform).GetComponent<GorillaKeyboardButton>()) || Object.op_Implicit((Object)(object)((Component)((RaycastHit)(ref hit)).transform).GetComponent<GorillaPlayerLineButton>()) || ((Object)((RaycastHit)(ref hit)).transform).name.ToLower().Contains("button"))
			{
				yield return PressButton(main, ((RaycastHit)(ref hit)).point - ((Component)Camera.main).transform.forward * 0.05f);
			}
			hit = default(RaycastHit);
		}
		state = State.IDLE;
	}

	private IEnumerator PressButton(HandDriver hand, Vector3 targetPosition)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		state = State.BUTTON;
		hand.grip = true;
		hand.targetPosition = reticle.position;
		hand.lookAt = targetPosition;
		hand.up = (hand.isLeft ? head.right : (-head.right));
		yield return (object)new WaitForSeconds(0.1f);
		hand.targetPosition = targetPosition;
		while (((Component)hand).transform.position.Distance(targetPosition) > 0.05f)
		{
			yield return (object)new WaitForFixedUpdate();
		}
		hand.targetPosition = reticle.position + ((Component)Camera.main).transform.forward * 0.05f;
		yield return (object)new WaitForSeconds(0.1f);
		hand.targetPosition = hand.DefaultPosition;
		state = State.IDLE;
	}

	private void AnimateHands()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		if (state == State.IDLE)
		{
			leftHand.targetPosition = leftHand.DefaultPosition;
			rightHand.targetPosition = rightHand.DefaultPosition;
			leftHand.lookAt = leftHand.targetPosition + head.forward;
			rightHand.lookAt = rightHand.targetPosition + head.forward;
			leftHand.up = head.right;
			rightHand.up = -head.right;
		}
	}

	protected override void Start()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		base.Start();
		reticle = GameObject.CreatePrimitive((PrimitiveType)0).transform;
		reticle.localScale = Vector3.one * 0.001f;
		((Renderer)((Component)reticle).GetComponent<MeshRenderer>()).material.color = Color.white;
		((Renderer)((Component)reticle).GetComponent<MeshRenderer>()).material.shader = Shader.Find("GorillaTag/UberShader");
		((Collider)((Component)reticle).GetComponent<SphereCollider>()).enabled = false;
		((Component)reticle).transform.SetParent(((Component)Camera.main).transform);
		((Component)reticle).transform.localPosition = Vector3.forward * 0.1f;
		((Component)reticle).gameObject.SetActive(false);
	}

	public override void Setup()
	{
		HeadDriver.Instance.LockCursor = true;
	}

	public override void Cleanup()
	{
		base.Cleanup();
		((Component)reticle).gameObject.SetActive(false);
		state = State.IDLE;
		((MonoBehaviour)this).StopAllCoroutines();
	}

	private void OnDestory()
	{
		Object.Destroy((Object)(object)((Component)reticle).gameObject);
	}
}
