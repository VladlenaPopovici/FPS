using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ecs
{
    [Serializable]
    public struct MovableComponent
    {
        public CharacterController characterController;
        public float speed;
    }
}