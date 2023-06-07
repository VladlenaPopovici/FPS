using UnityEngine;
using UnityEngine.UI;

namespace Ecs
{
    public struct ButtonHoldComponent
    {
        public float holdDuration;
        public float holdTimer;
        public bool isButtonHeld;
        public bool isButtonReleased;
    }
}