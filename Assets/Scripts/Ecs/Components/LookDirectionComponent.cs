using System;
using UnityEngine;

namespace Ecs
{
    [Serializable]
    public struct LookDirectionComponent
    {
        public Transform cameraTransform;
        public Vector2 direction;
        [Range(0, 2)] public float mouseSensitivity;
    }
}