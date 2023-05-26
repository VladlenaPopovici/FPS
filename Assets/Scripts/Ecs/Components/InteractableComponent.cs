using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ecs
{
    [Serializable]
    public struct InteractableComponent
    {
        public Transform transform;
        public Collider collider;
        public InteractableType type;
    }

    public enum InteractableType
    {
        Chest,
    }
}