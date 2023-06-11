using UnityEngine;
using UnityEngine.UI;

namespace Ecs.Data
{
    [CreateAssetMenu]
    public class MainMenuStaticData : ScriptableObject
    {
        public Button playButtonPrefab;
        public Button optionButtonPrefab;
        public Button quitButtonPrefab;
    }
}