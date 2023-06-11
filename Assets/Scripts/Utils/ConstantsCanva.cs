using UnityEngine;

namespace Utils
{
    public class ConstantsCanva
    {
        private static GameObject buttonsPanelObj = GameObject.FindWithTag("UI");
        public static RectTransform buttonsPanel = buttonsPanelObj.GetComponent<RectTransform>();
    }
}