using UnityEngine;

namespace Ecs.Components
{
    public struct JumpComponent
    {
        public float JumpForce;
        public float Gravity;
        public Vector3 PlayerVelocity;
    }
}