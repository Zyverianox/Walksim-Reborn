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
        get => _lockCursor;
        set
        {
            _lockCursor = value;
            Cursor.lockState = _lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !_lockCursor;
        }
    }

    public bool FirstPerson
    {
        get => overrideCam.enabled;
        set => overrideCam.enabled = value;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        var thirdPersonFollow = FindObjectOfType<Cinemachine3rdPersonFollow>();
        thirpyTarget = thirdPersonFollow.VirtualCamera.Follow;
        var componentInParent = thirdPersonFollow.GetComponentInParent<Camera>();
        cameraObject = Instantiate(WalkSim.Plugin.Plugin.Instance.bundle.LoadAsset<GameObject>("Override Camera"));
        overrideCam = cameraObject.GetComponent<Camera>();
        overrideCam.nearClipPlane = componentInParent.nearClipPlane;
        overrideCam.farClipPlane = componentInParent.farClipPlane;
        overrideCam.cullingMask = componentInParent.cullingMask;
        overrideCam.depth = componentInParent.depth + 1f;
        overrideCam.targetDisplay = componentInParent.targetDisplay;
        overrideCam.fieldOfView = 90f;
        overrideCam.enabled = false;
    }

    private void LateUpdate()
    {
        cameraObject.transform.position = Player.Instance.headCollider.transform.TransformPoint(offset);
        cameraObject.transform.forward = head.forward;
        if (turn)
        {
            var mouseDelta = Mouse.current.delta.ReadValue();
            Player.Instance.Turn(mouseDelta.x / 10f);
            var eulerAngles = GorillaTagger.Instance.offlineVRRig.headConstraint.eulerAngles;
            eulerAngles.x -= mouseDelta.y / 10f;
            if (eulerAngles.x > 180f)
            {
                eulerAngles.x -= 360f;
            }
            eulerAngles.x = Mathf.Clamp(eulerAngles.x, -60f, 60f);
            GorillaTagger.Instance.offlineVRRig.headConstraint.eulerAngles = eulerAngles;
            eulerAngles.y += 90f;
            thirpyTarget.localEulerAngles = new Vector3(eulerAngles.x, 0f, 0f);
            Player.Instance.headCollider.transform.localEulerAngles = new Vector3(eulerAngles.x, 0f, 0f);
        }
    }

    private void OverrideHeadMovement()
    {
        head = GorillaTagger.Instance.offlineVRRig.head.rigTarget;
    }

    internal void ToggleCam()
    {
        overrideCam.enabled = !overrideCam.enabled;
    }

    private void OnEnable()
    {
        Logging.Debug("Enabled");
        if (Rig.Instance.Animator != null)
        {
            LockCursor = true;
            OverrideHeadMovement();
        }
    }

    private void OnDisable()
    {
        if (head != null)
        {
            LockCursor = false;
            GorillaTagger.Instance.offlineVRRig.head.rigTarget = head;
        }
    }
}
