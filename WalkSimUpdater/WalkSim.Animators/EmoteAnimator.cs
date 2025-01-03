using System;
using UnityEngine;
using WalkSim.Plugin;
using WalkSim.Rigging;

namespace WalkSim.Animators
{
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
            rig.active = true;
            rig.useGravity = false;
            if (emote == Emote.Dance)
            {
                float num = Time.time - startTime;
                Vector3 val = new Vector3(0f, Mathf.Sin(num * 10f) * 0.1f, 0f);
                rig.targetPosition = startingPosition + val;
            }
            else
            {
                rig.targetPosition = startingPosition;
            }
        }

        private void AnimateHand(HandDriver hand)
        {
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
            if (Time.time - lastDanceSwitch > danceSwitchRate)
            {
                dance = UnityEngine.Random.Range(0, 4);
                lastDanceSwitch = Time.time;
            }
            HandPositionInfo result = default(HandPositionInfo);
            float num = hand.isLeft ? 0f : Mathf.PI;
            switch (dance)
            {
                case 0:
                    Vector3 lookAt0 = hand.isLeft ? (hand.targetPosition + head.right + head.forward) : (hand.targetPosition - head.right + head.forward);
                    Vector3 pos0 = new Vector3(hand.isLeft ? -0.2f : 0.2f, -0.3f, 0.2f);
                    pos0.y += Mathf.Sin(t * 10f + num) * 0.1f;
                    result.position = head.TransformPoint(pos0);
                    result.lookAt = lookAt0;
                    result.up = head.up;
                    result.grip = true;
                    result.trigger = true;
                    result.thumb = true;
                    result.used = true;
                    return result;
                case 1:
                    Vector3 lookAt1 = hand.targetPosition + head.up;
                    Vector3 pos1 = new Vector3(hand.isLeft ? -0.2f : 0.2f, -0.2f, 0.3f);
                    pos1.z += Mathf.Sin(t * 10f + num) * 0.1f;
                    result.position = head.TransformPoint(pos1);
                    result.lookAt = lookAt1;
                    result.up = hand.isLeft ? head.right : -head.right;
                    result.grip = false;
                    result.trigger = false;
                    result.thumb = false;
                    result.used = true;
                    return result;
                case 2:
                    Vector3 lookAt2 = hand.targetPosition + head.up;
                    Vector3 pos2 = new Vector3(hand.isLeft ? -0.2f : 0.2f, -0.3f, 0.2f);
                    pos2.y += Mathf.Sin(t * 10f + num) * 0.1f;
                    result.position = head.TransformPoint(pos2);
                    result.lookAt = lookAt2;
                    result.up = hand.isLeft ? head.right : -head.right;
                    result.grip = true;
                    result.trigger = false;
                    result.thumb = true;
                    result.used = true;
                    return result;
                case 3:
                    Vector3 lookAt3 = hand.targetPosition + head.up;
                    Vector3 pos3 = new Vector3(hand.isLeft ? -0.2f : 0.2f, -0.2f, 0.3f);
                    pos3.x += Mathf.Cos(t * 10f) * 0.1f;
                    pos3.z += Mathf.Sin(t * 10f) * 0.1f;
                    result.position = head.TransformPoint(pos3);
                    result.lookAt = lookAt3;
                    result.up = hand.isLeft ? head.right : -head.right;
                    result.grip = true;
                    result.trigger = true;
                    result.thumb = true;
                    result.used = true;
                    return result;
                default:
                    return result;
            }
        }

        private HandPositionInfo ThumbsDownPositioner(HandDriver hand, float _)
        {
            Vector3 lookAt = hand.isLeft ? (hand.targetPosition + head.right) : (hand.targetPosition - head.right);
            HandPositionInfo result = default(HandPositionInfo);
            result.position = head.TransformPoint(new Vector3(hand.isLeft ? -0.2f : 0.2f, 0f, 0.4f));
            result.lookAt = lookAt;
            result.up = -head.up;
            result.grip = true;
            result.trigger = true;
            result.used = true;
            return result;
        }

        private HandPositionInfo ThumbsUpPositioner(HandDriver hand, float _)
        {
            Vector3 lookAt = hand.isLeft ? (hand.targetPosition + head.right) : (hand.targetPosition - head.right);
            HandPositionInfo result = default(HandPositionInfo);
            result.position = head.TransformPoint(new Vector3(hand.isLeft ? -0.2f : 0.2f, 0f, 0.4f));
            result.lookAt = lookAt;
            result.up = head.up;
            result.grip = true;
            result.trigger = true;
            result.used = true;
            return result;
        }

        private HandPositionInfo ShrugPositioner(HandDriver hand, float _)
        {
            Vector3 lookAt = hand.isLeft ? (hand.targetPosition - head.right + head.forward) : (hand.targetPosition + head.right + head.forward);
            HandPositionInfo result = default(HandPositionInfo);
            result.position = body.TransformPoint(new Vector3(hand.isLeft ? -0.4f : 0.4f, 0f, 0.2f));
            result.lookAt = lookAt;
            result.up = -head.forward;
            result.used = true;
            return result;
        }

        private HandPositionInfo PointPositioner(HandDriver hand, float __)
        {
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
            Vector3 val = new Vector3(0.25f, 0f, 0.2f);
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
            Logging.Debug("===SETUP===");
            base.Start();
            startingPosition = body.position;
        }
    }
}
