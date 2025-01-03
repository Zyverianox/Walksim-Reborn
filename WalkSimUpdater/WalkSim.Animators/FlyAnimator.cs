using GorillaLocomotion;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using WalkSim.Plugin;
using WalkSim.Rigging;
using WalkSim.Tools;

namespace WalkSim.Animators
{
    public class FlyAnimator : AnimatorBase
    {
        private float speed = 1f;
        private float minSpeed = 0f;
        private float maxSpeed = 5f;
        private int layersBackup;
        private bool noClipActive = false;

        private void Awake()
        {
            layersBackup = Player.Instance.locomotionEnabledLayers;
        }

        public override void Animate()
        {
            AnimateBody();
            AnimateHands();
        }

        private void Update()
        {
            speed += Mouse.current.scroll.ReadValue().y / 1000f;
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

            if (Keyboard.current.nKey.wasPressedThisFrame)
            {
                noClipActive = !noClipActive;
                Player.Instance.locomotionEnabledLayers = noClipActive ? 536870912 : layersBackup;
                Player.Instance.headCollider.isTrigger = noClipActive;
                Player.Instance.bodyCollider.isTrigger = noClipActive;
            }
        }

        private void AnimateBody()
        {
            rig.active = true;
            rig.useGravity = false;
            rig.targetPosition = body.TransformPoint(InputHandler.inputDirection * speed);
        }

        private void AnimateHands()
        {
            leftHand.followRate = rightHand.followRate = Extensions.Map(speed, minSpeed, maxSpeed, 0f, 1f);
            leftHand.targetPosition = leftHand.DefaultPosition;
            rightHand.targetPosition = rightHand.DefaultPosition;
            leftHand.lookAt = leftHand.targetPosition + body.forward;
            rightHand.lookAt = rightHand.targetPosition + body.forward;
            leftHand.up = body.right;
            rightHand.up = -body.right;
        }

        public override void Cleanup()
        {
            base.Cleanup();
            leftHand.followRate = rightHand.followRate = 0.1f;
            Player.Instance.locomotionEnabledLayers = layersBackup;
        }

        public override void Setup()
        {
            HeadDriver.Instance.LockCursor = true;
            HeadDriver.Instance.turn = true;
        }
    }
}
