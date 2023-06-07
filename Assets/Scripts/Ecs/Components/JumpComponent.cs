using UnityEngine;

namespace Ecs
{
    public struct JumpComponent
    {
        public float jumpForce;
        public float gravity;
        public Vector3 playerVelocity;
    }
}