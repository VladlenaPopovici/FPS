﻿using UnityEngine;

namespace Utils
{
    public static class Constants
    {
        public const float MinAngle = 0;
        public const float MaxAngle = 360;

        private static readonly GameObject ButtonsPanelObj = GameObject.FindWithTag("UI");
        public static readonly RectTransform ButtonsPanel = ButtonsPanelObj.GetComponent<RectTransform>();
    }
}