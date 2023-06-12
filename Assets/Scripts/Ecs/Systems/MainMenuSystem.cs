using Ecs.Data;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace Ecs.Systems
{
    public sealed class MainMenuSystem : IEcsInitSystem
    {
        private ScrollRect _mainMenuRect;
        private MainMenuStaticData _staticData;
        private EcsWorld _world;

        public void Init()
        {
            var playButton = Object.Instantiate(_staticData.playButtonPrefab, ConstantsCanvas.ButtonsPanel);
            playButton.onClick.AddListener(StartGame);

            var optionButton = Object.Instantiate(_staticData.optionButtonPrefab, ConstantsCanvas.ButtonsPanel);
            optionButton.onClick.AddListener(OptionMenu);

            var quitButton = Object.Instantiate(_staticData.quitButtonPrefab, ConstantsCanvas.ButtonsPanel);
            quitButton.onClick.AddListener(QuitGame);
        }

        private static void QuitGame()
        {
            Debug.Log("Quit Game");
            Application.Quit();
        }

        private static void OptionMenu()
        {
            Debug.Log("Open option menu");
        }

        private static void StartGame()
        {
            SceneManager.LoadScene("SampleScene");
            Debug.Log("startGame");
        }
    }
}