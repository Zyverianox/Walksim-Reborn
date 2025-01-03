using GorillaLocomotion;
using UnityEngine;
using WalkSim.Animators;

namespace WalkSim.Rigging;

public class Rig : MonoBehaviour
{
	public Transform head;

	public Transform body;

	public HeadDriver headDriver;

	public HandDriver leftHand;

	public HandDriver rightHand;

	public Rigidbody rigidbody;

	public Vector3 targetPosition;

	public Vector3 lastNormal;

	public Vector3 lastGroundPosition;

	public bool onGround;

	public bool active;

	public bool useGravity = true;

	private float scale = 1f;

	private float speed = 4f;

	private float raycastLength = 1.3f;

	private float raycastRadius = 0.3f;

	private Vector3 raycastOffset = new Vector3(0f, 0.4f, 0f);

	private AnimatorBase _animator;

	public static Rig Instance { get; private set; }

	public Vector3 smoothedGroundPosition { get; set; }

	public AnimatorBase Animator
	{
		get
		{
			return _animator;
		}
		set
		{
			if ((Object)(object)_animator != (Object)null && (Object)(object)value != (Object)(object)_animator)
			{
				_animator.Cleanup();
			}
			_animator = value;
			if (Object.op_Implicit((Object)(object)_animator))
			{
				((Behaviour)_animator).enabled = true;
				_animator.Setup();
			}
			((Behaviour)leftHand).enabled = Object.op_Implicit((Object)(object)_animator);
			((Behaviour)rightHand).enabled = Object.op_Implicit((Object)(object)_animator);
			((Behaviour)headDriver).enabled = Object.op_Implicit((Object)(object)_animator);
		}
	}

	private void Awake()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		Instance = this;
		head = ((Component)Player.Instance.headCollider).transform;
		body = ((Component)Player.Instance.bodyCollider).transform;
		rigidbody = ((Collider)Player.Instance.bodyCollider).attachedRigidbody;
		leftHand = new GameObject("WalkSim Left Hand Driver").AddComponent<HandDriver>();
		leftHand.Init(isLeft: true);
		((Behaviour)leftHand).enabled = false;
		rightHand = new GameObject("WalkSim Right Hand Driver").AddComponent<HandDriver>();
		rightHand.Init(isLeft: false);
		((Behaviour)rightHand).enabled = false;
		headDriver = new GameObject("WalkSim Head Driver").AddComponent<HeadDriver>();
		((Behaviour)headDriver).enabled = false;
	}

	private void FixedUpdate()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		scale = Player.Instance.scale;
		smoothedGroundPosition = Vector3.Lerp(smoothedGroundPosition, lastGroundPosition, 0.2f);
		OnGroundRaycast();
		AnimatorBase animator = Animator;
		if ((Object)(object)animator != (Object)null)
		{
			animator.Animate();
		}
		Move();
	}

	private void Move()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		if (active)
		{
			rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, (targetPosition - body.position) * speed, 1f);
			if (!useGravity)
			{
				rigidbody.AddForce(-Physics.gravity * rigidbody.mass * scale);
			}
		}
	}

	private void OnGroundRaycast()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		float num = raycastLength * scale;
		Vector3 val = body.TransformPoint(raycastOffset);
		float num2 = raycastRadius * scale;
		RaycastHit val2 = default(RaycastHit);
		bool flag = Physics.Raycast(val, Vector3.down, ref val2, num, LayerMask.op_Implicit(Player.Instance.locomotionEnabledLayers));
		RaycastHit val3 = default(RaycastHit);
		bool flag2 = Physics.SphereCast(val, num2, Vector3.down, ref val3, num, LayerMask.op_Implicit(Player.Instance.locomotionEnabledLayers));
		RaycastHit val4;
		if (flag && flag2)
		{
			val4 = ((((RaycastHit)(ref val2)).distance <= ((RaycastHit)(ref val3)).distance) ? val2 : val3);
		}
		else if (flag2)
		{
			val4 = val3;
		}
		else
		{
			if (!flag)
			{
				onGround = false;
				return;
			}
			val4 = val2;
		}
		lastNormal = ((RaycastHit)(ref val4)).normal;
		onGround = true;
		lastGroundPosition = ((RaycastHit)(ref val4)).point;
		lastGroundPosition.x = body.position.x;
		lastGroundPosition.z = body.position.z;
	}
}
