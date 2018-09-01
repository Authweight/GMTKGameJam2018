using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Interactive : MonoBehaviour
    {
        public float GroundDistance;
        protected bool isGrounded;
        protected virtual bool MoveWithConveyor => isGrounded;
        protected virtual float LaunchX => 4f;
        protected virtual float LaunchY => 30f;
        protected Rigidbody2D rb;
        private float lastLaunch = 0;
        private float launchDelay = .3f;
        private bool isLaunching = false;

        public void OnStart()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void OnUpdate()
        {
            isGrounded = IsGrounded();

            if (TimeKeeper.GetTime() - lastLaunch > launchDelay)
                isLaunching = false;
        }

        public bool IsGrounded()
        {
            return Physics2D.Raycast(transform.position, Vector2.down, GroundDistance, LayerMask.GetMask("Conveyor"));
        }

        public void OnFixedUpdate()
        {
            MoveAlongConveyor();
        }

        protected virtual void MoveAlongConveyor()
        {
            if (MoveWithConveyor && !isLaunching)
                rb.velocity = new Vector2(ConveyorSpeed.GetSpeed(), rb.velocity.y);
        }

        public virtual void SpringLaunch()
        {
            if (!isLaunching)
            {
                lastLaunch = TimeKeeper.GetTime();
                isLaunching = true;
                rb.velocity = new Vector2(LaunchX + rb.velocity.x, LaunchY);
            }
        }
    }
}
