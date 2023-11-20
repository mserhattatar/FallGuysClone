using Game.Scripts.Container;
using Game.Scripts.Controller;
using UnityEngine;

namespace Game.Scripts.Manager
{
    public class GameManager : ComponentContainerBehaviour
    {
        private GameSceneManager _gameSceneManager;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_EDITOR_LINUX || UNITY_WEBGL
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }
#endif

        public override void ContainerOnAwake()
        {
            _gameSceneManager = MainContainer.GetContainerComponent(nameof(GameSceneManager)) as GameSceneManager;
        }

        public override void ContainerDoAfterAwake()
        {
            _gameSceneManager.LoadNextScene();

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_EDITOR_LINUX || UNITY_WEBGL
            Screen.SetResolution(2960, 1440, true);
#endif
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            FinishLineController.FinisLineAction += GameEnded;
        }

        protected override void UnRegisterEvents()
        {
            base.UnRegisterEvents();
            FinishLineController.FinisLineAction -= GameEnded;
        }

        private void GameEnded(bool isSuccessful)
        {
            //TODO: add fail and success result canvas

            if (isSuccessful)
                _gameSceneManager.LoadNextScene();
            else
                _gameSceneManager.ReLoadScene();
        }
    }
}