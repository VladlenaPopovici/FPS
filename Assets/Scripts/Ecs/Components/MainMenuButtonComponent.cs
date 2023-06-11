using UnityEngine.UI;

namespace Ecs
{
    public struct MainMenuButtonComponent
    {
        public Button Button;
        public ButtonType ButtonType;
        public bool IsClicked;
    }

    public enum ButtonType
    {
        StartButton,
        OptionButton,
        QuitButton
    }
}