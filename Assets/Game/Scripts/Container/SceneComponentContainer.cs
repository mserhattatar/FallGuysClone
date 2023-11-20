using Game.Scripts.Manager;
using UnityEngine;

namespace Game.Scripts.Container
{
    public class SceneComponentContainer : MonoBehaviour
    {
        [SerializeField] private ComponentContainerBehaviour[] containerBehavioursList;
        private GameSceneManager _gameSceneManager;
        private MainContainer _mainContainer;

        private void Awake()
        {
            _mainContainer = FindObjectOfType<MainContainer>();
            _gameSceneManager = _mainContainer.GetContainerComponent(nameof(GameSceneManager)) as GameSceneManager;
            _gameSceneManager!.SceneChangingDelegate += OnGameSceneChangingDelegate;
            _mainContainer.AddSceneComponentContainer(containerBehavioursList);
        }

        private void OnGameSceneChangingDelegate()
        {
            _gameSceneManager.SceneChangingDelegate -= OnGameSceneChangingDelegate;
            _mainContainer.RemoveSceneComponentContainer(containerBehavioursList);
        }
    }
}