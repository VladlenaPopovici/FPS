using System;
using UnityEngine;

namespace Ecs.Components
{
    [Serializable]
    public struct MovableComponent
    {
        public CharacterController characterController;
        public float speed;
    }
}