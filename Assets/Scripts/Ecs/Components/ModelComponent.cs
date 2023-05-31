using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ecs
{
    [Serializable]
    public struct ModelComponent
    {
        public Transform modelTransform;
    }
}