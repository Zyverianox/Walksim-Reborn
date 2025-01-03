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

    private bool IsSprinting => Keyboard.current.leftShiftKey.isPressed;
    private bool NotMoving => InputHandler.inputDirectionNoY == Vector3.zero;

    public override void Animate()
    {
        MoveBody();
        AnimateHands();
    }

    private void Update()
    {
        if (WalkSim.Plugin.Plugin.Instance.Enabled)
        {
            if (!hasJumped && rig.onGround && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                hasJumped = true;
                onJumpCooldown = true;
                jumpTime = Time.time;
                rig.active = false;
                rigidbody.AddForce(Vector3.up * 250f * Player.Instance.scale, ForceMode.Impulse);
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
        rig.active = rig.onGround && !hasJumped;
        rig.useGravity = !rig.onGround;

        if (rig.onGround)
        {
            float num, num2, num3;
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
            Vector3 targetPosition = rig.lastGroundPosition + Vector3.up * height * Player.Instance.scale;

            Vector3 direction = body.TransformDirection(InputHandler.inputDirectionNoY);
            direction.y = 0f;
            if (Vector3.Dot(rig.lastNormal, Vector3.up) > 0.3f)
            {
                direction = Vector3.ProjectOnPlane(direction, rig.lastNormal);
            }

            direction *= Player.Instance.scale;
            float moveSpeed = IsSprinting ? (speed * 3f) : speed;
            targetPosition += direction * moveSpeed / 10f;
            rig.targetPosition = Vector3.Lerp(rig.targetPosition, targetPosition, Time.deltaTime * 5f);
        }
    }

    private void AnimateHands()
    {
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
        if (!Physics.Raycast(val2, -lastNormal, out RaycastHit hit, num, Player.Instance.locomotionEnabledLayers))
        {
            if (NotMoving)
            {
                hand.targetPosition = hand.DefaultPosition;
            }
        }
        else
        {
            hand.hit = hit.point;
            hand.normal = hit.normal;
            hand.lookAt = hand.transform.position + body.forward;
        }
    }

    private void AnimateHand(HandDriver hand, HandDriver otherHand)
    {
        float x = Mathf.Abs(Vector3.Dot(InputHandler.inputDirectionNoY, Vector3.forward));
        float x2 = Vector3.Dot(rig.lastNormal, Vector3.up);
        float num = Extensions.Map(x2, 0f, 1f, 0.1f, 0.6f);
        float num2 = Extensions.Map(x, 0f, 1f, 0.5f, 1.25f) * num * Player.Instance.scale;
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
