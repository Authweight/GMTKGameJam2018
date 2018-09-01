using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class SpringManager
    {
        private DeployState deployState = DeployState.Dropping;
        
        private float extensionForceX = 1f;
        private float extensionForceY = 5f;

        private float? deathTime;
        private float deathDelay = 1.2f;

        private float maxHardDrop = 30f;

        internal void HandleDrop(Transform transform, Transform spawn, bool isGrounded)
        {
            if (deployState == DeployState.Dropping)
            {
                transform.position = new Vector3(spawn.position.x, transform.position.y, transform.position.z);
            }

            if (isGrounded)
            {
                deployState = DeployState.Finished;
            }
        }

        internal bool IsDropping()
        {
            return deployState == DeployState.Dropping;
        }

        internal void Deploy()
        {
            deployState = DeployState.Deploying;
        }

        internal bool IsDeploying()
        {
            return deployState == DeployState.Deploying;
        }

        public Vector2 GetVelocity(Vector2 velocity)
        {
            if (deployState == DeployState.Dropping)
            {
                return new Vector2(velocity.x, Mathf.Max(velocity.y, -maxHardDrop));
            }

            return velocity;
        }

        internal Vector2 Extend(Vector2 velocity)
        {
            deployState = DeployState.Finished;
            deathTime = TimeKeeper.GetTime() + deathDelay;
            return new Vector2(extensionForceX, extensionForceY);
        }

        public bool Destroy()
        {
            return deathTime.HasValue && TimeKeeper.GetTime() > deathTime;
        }

        public enum DeployState
        {
            Dropping,
            Deploying,
            Finished
        }
    }
}
