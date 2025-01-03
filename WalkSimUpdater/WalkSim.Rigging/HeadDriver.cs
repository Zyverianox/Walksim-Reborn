using Cinemachine;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.InputSystem;
using WalkSim.Plugin;

namespace WalkSim.Rigging;

public class HeadDriver : MonoBehaviour
{
	public static HeadDriver Instance;

	public Transform thirpyTarget;

	public Transform head;

	public GameObject cameraObject;

	private Vector3 offset = new Vector3(0f, 0f, 0f);

	private Camera overrideCam;

	public bool turn = true;

	private bool _lockCursor;

	public bool LockCursor
	{
		get
		{
			return _lockCursor;
		}
		set
		{
			_lockCursor = value;
			if (_lockCursor)
			{
				Cursor.lockState = (CursorLockMode)1;
				Cursor.visible = false;
			}
			else
			{
				Cursor.lockState = (CursorLockMode)0;
				Cursor.visible = true;
			}
		}
	}

	public bool FirstPerson
	{
		get
		{
			return ((Behaviour)overrideCam).enabled;
		}
		set
		{
			((Behaviour)overrideCam).enabled = value;
		}
	}

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		Cinemachine3rdPersonFollow val = Object.FindObjectOfType<Cinemachine3rdPersonFollow>();
		thirpyTarget = ((CinemachineComponentBase)val).VirtualCamera.Follow;
		Camera componentInParent = ((Component)val).gameObject.GetComponentInParent<Camera>();
		cameraObject = WalkSim.Plugin.Plugin.Instance.bundle.LoadAsset<GameObject>("Override Camera");
		cameraObject = Object.Instantiate<GameObject>(cameraObject);
		overrideCam = cameraObject.GetComponent<Camera>();
		overrideCam.nearClipPlane = componentInParent.nearClipPlane;
		overrideCam.farClipPlane = componentInParent.farClipPlane;
		overrideCam.cullingMask = componentInParent.cullingMask;
		overrideCam.depth = componentInParent.depth + 1f;
		overrideCam.targetDisplay = componentInParent.targetDisplay;
		overrideCam.fieldOfView = 90f;
		((Behaviour)overrideCam).enabled = false;
	}

	private void LateUpdate()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		cameraObject.transform.position = ((Component)Player.Instance.headCollider).transform.TransformPoint(offset);
		cameraObject.transform.forward = head.forward;
		if (turn)
		{
			Player.Instance.Turn(((Vector2)((InputControl<Vector2>)(object)((Pointer)Mouse.current).delta).value).x / 10f);
			Vector3 eulerAngles = GorillaTagger.Instance.offlineVRRig.headConstraint.eulerAngles;
			eulerAngles.x -= ((Vector2)((InputControl<Vector2>)(object)((Pointer)Mouse.current).delta).value).y / 10f;
			if (eulerAngles.x > 180f)
			{
				eulerAngles.x -= 360f;
			}
			eulerAngles.x = Mathf.Clamp(eulerAngles.x, -60f, 60f);
			GorillaTagger.Instance.offlineVRRig.headConstraint.eulerAngles = eulerAngles;
			eulerAngles.y += 90f;
			thirpyTarget.localEulerAngles = new Vector3(eulerAngles.x, 0f, 0f);
			((Component)Player.Instance.headCollider).transform.localEulerAngles = new Vector3(eulerAngles.x, 0f, 0f);
		}
	}

	private void OverrideHeadMovement()
	{
		head = GorillaTagger.Instance.offlineVRRig.head.rigTarget;
	}

	internal void ToggleCam()
	{
		((Behaviour)overrideCam).enabled = !((Behaviour)overrideCam).enabled;
	}

	private void OnEnable()
	{
		Logging.Debug("Enabled");
		if (!((Object)(object)Rig.Instance.Animator == (Object)null))
		{
			LockCursor = true;
			OverrideHeadMovement();
		}
	}

	private void OnDisable()
	{
		if (Object.op_Implicit((Object)(object)head))
		{
			LockCursor = false;
			GorillaTagger.Instance.offlineVRRig.head.rigTarget = head;
		}
	}
}
