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
        get => _animator;
        set
        {
            if (_animator != null && value != _animator)
            {
                _animator.Cleanup();
            }
            _animator = value;
            if (_animator != null)
            {
                _animator.enabled = true;
                _animator.Setup();
            }
            leftHand.enabled = _animator != null;
            rightHand.enabled = _animator != null;
            headDriver.enabled = _animator != null;
        }
    }

    private void Awake()
    {
        Instance = this;
        head = Player.Instance.headCollider.transform;
        body = Player.Instance.bodyCollider.transform;
        rigidbody = Player.Instance.bodyCollider.attachedRigidbody;
        leftHand = new GameObject("WalkSim Left Hand Driver").AddComponent<HandDriver>();
        leftHand.Init(isLeft: true);
        leftHand.enabled = false;
        rightHand = new GameObject("WalkSim Right Hand Driver").AddComponent<HandDriver>();
        rightHand.Init(isLeft: false);
        rightHand.enabled = false;
        headDriver = new GameObject("WalkSim Head Driver").AddComponent<HeadDriver>();
        headDriver.enabled = false;
    }

    private void FixedUpdate()
    {
        scale = Player.Instance.scale;
        smoothedGroundPosition = Vector3.Lerp(smoothedGroundPosition, lastGroundPosition, 0.2f);
        OnGroundRaycast();
        Animator?.Animate();
        Move();
    }

    private void Move()
    {
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
        float num = raycastLength * scale;
        Vector3 rayOrigin = body.TransformPoint(raycastOffset);
        float num2 = raycastRadius * scale;
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hitInfo, num, Player.Instance.locomotionEnabledLayers) ||
            Physics.SphereCast(rayOrigin, num2, Vector3.down, out hitInfo, num, Player.Instance.locomotionEnabledLayers))
        {
            lastNormal = hitInfo.normal;
            onGround = true;
            lastGroundPosition = hitInfo.point;
            lastGroundPosition.x = body.position.x;
            lastGroundPosition.z = body.position.z;
        }
        else
        {
            onGround = false;
        }
    }
}
