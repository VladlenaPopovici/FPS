using UnityEngine;

namespace Utils
{
    public class Constants
    {
        public const float MinAngle = 0;
        public const float MaxAngle = 360;
        
        private static GameObject buttonsPanelObj = GameObject.FindWithTag("UI");
        public static RectTransform buttonsPanel = buttonsPanelObj.GetComponent<RectTransform>();
    }
}