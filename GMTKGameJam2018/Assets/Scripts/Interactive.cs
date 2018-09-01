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
        protected bool IsGrounded => CalculateGrounded();
        protected virtual bool MoveWithConveyor => IsGrounded;
        protected virtual float LaunchX => 4f;
        protected virtual float LaunchY => 30f;
        protected Rigidbody2D rb;
        private float lastLaunch = 0;
        private float launchDelay = .3f;
        private bool isLaunching = false;
        private bool hasLaunched = false;

        public Action onLaunch = () => { };
        public Action onLand = () => { };

        public void OnStart()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void OnUpdate()
        {                        
            if (TimeKeeper.GetTime() - lastLaunch > launchDelay)
                isLaunching = false;
        }

        public bool CalculateGrounded()
        {
            return Physics2D.Raycast(transform.position, Vector2.down, GroundDistance, LayerMask.GetMask("Conveyor"));
        }

        public void OnFixedUpdate()
        {
            if (!isLaunching && hasLaunched && IsGrounded)
            {
                onLand();
                hasLaunched = false;
            }

            MoveAlongConveyor();
        }

        protected virtual void MoveAlongConveyor()
        {
            if (MoveWithConveyor && !isLaunching)
                rb.velocity = new Vector2(ConveyorSpeed.GetSpeed(), rb.velocity.y);
        }

        public virtual void SpringLaunch(float? x = null, float? y= null)
        {
            if (!isLaunching)
            {
                lastLaunch = TimeKeeper.GetTime();
                isLaunching = true;
                hasLaunched = true;
                rb.velocity = new Vector2(x ?? LaunchX + rb.velocity.x, y ?? LaunchY);
                onLaunch();
            }
        }
    }
}
