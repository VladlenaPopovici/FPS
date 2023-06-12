using UnityEngine;

namespace Utils
{
    public static class ConstantsCanvas
    {
        private static readonly GameObject ButtonsPanelObj = GameObject.FindWithTag("UI");
        public static readonly RectTransform ButtonsPanel = ButtonsPanelObj.GetComponent<RectTransform>();
    }
}