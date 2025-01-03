using UnityEngine;
using WalkSim.Plugin;
using WalkSim.Rigging;

namespace WalkSim.Animators
{
    public abstract class AnimatorBase : MonoBehaviour
    {
        protected Transform body;
        protected Transform head;
        protected Rigidbody rigidbody;
        protected HandDriver leftHand;
        protected HandDriver rightHand;
        protected Rig rig;

        protected virtual void Start()
        {
            Logging.Debug("==START==");
            rig = Rig.Instance;
            body = rig.body;
            head = rig.head;
            rigidbody = rig.rigidbody;
            leftHand = rig.leftHand;
            rightHand = rig.rightHand;
        }

        /// <summary>
        /// Set up the animator.
        /// </summary>
        public abstract void Setup();

        /// <summary>
        /// Clean up the animator and reset the rig.
        /// </summary>
        public virtual void Cleanup()
        {
            this.enabled = false;
            rig.active = false;
            rig.useGravity = true;
            rig.headDriver.turn = true;
            leftHand.Reset();
            rightHand.Reset();
        }

        /// <summary>
        /// Animate the rig.
        /// </summary>
        public abstract void Animate();
    }
}
