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
        private EcsFilter<MainMenuButtonComponent> _buttonFilter;
        private EcsWorld _world;
        private MainMenuStaticData _staticData;
        private ScrollRect _mainMenuRect;

        public void Init()
        {
            var playButton = Object.Instantiate(_staticData.playButtonPrefab, ConstantsCanva.buttonsPanel);
            playButton.onClick.AddListener(StartGame);

            var optionButton = Object.Instantiate(_staticData.optionButtonPrefab, ConstantsCanva.buttonsPanel);
            optionButton.onClick.AddListener(OptionMenu);

            var quitButton = Object.Instantiate(_staticData.quitButtonPrefab, ConstantsCanva.buttonsPanel);
            quitButton.onClick.AddListener(QuitGame);
        }

        private void QuitGame()
        {
            Debug.Log("Quit Game");
            Application.Quit();
        }

        private void OptionMenu()
        {
            Debug.Log("Open option menu");
        }

        private void StartGame()
        {
            SceneManager.LoadScene("SampleScene");
            Debug.Log("startGame");
        }
    }
}