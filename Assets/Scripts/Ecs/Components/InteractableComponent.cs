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
        public bool isInteracted;
    }
}