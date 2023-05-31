using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ecs
{
    
    [Serializable]
    public struct DirectionComponent
    {
        public Vector3 direction;
    }
}