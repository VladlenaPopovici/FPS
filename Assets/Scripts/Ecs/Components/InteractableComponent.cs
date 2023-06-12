using System;
using UnityEngine;

namespace Ecs.Components
{
    [Serializable]
    public struct InteractableComponent
    {
        public Transform transform;
        public Collider collider;
        public InteractableType type;
        public bool isNear;
    }

    public enum InteractableType
    {
        Chest,
        Tree,
        Rock,
        Player,
        Enemy
    }
}