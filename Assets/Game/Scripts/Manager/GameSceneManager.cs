using System;
using System.Collections;
using Game.Scripts.Container;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Manager
{
    public class GameSceneManager : ComponentContainerBehaviour
    {
        private const string AsyncRoutineKey = "AsyncSceneLoadingRoutineKey";

        private CoroutineManager _coroutineManager;
        public event Action SceneChangingDelegate;
        public event Action SceneChangedDelegate;

        public override void ContainerOnAwake()
        {
            _coroutineManager = MainContainer.GetContainerComponent(nameof(CoroutineManager)) as CoroutineManager;
        }

        public override void ContainerDoAfterAwake()
        {
        }

        public void LoadNextScene()
        {
            var activeScene = SceneManager.GetActiveScene().buildIndex;
            activeScene++;
            if (activeScene == SceneManager.sceneCountInBuildSettings)
                activeScene = 1;

            LoadScene(activeScene);
        }

        public void ReLoadScene()
        {
            var activeScene = SceneManager.GetActiveScene().buildIndex;
            LoadScene(activeScene);
        }

        private void LoadScene(int sceneIndex)
        {
            _coroutineManager.HStopCoroutine(AsyncRoutineKey);
            _coroutineManager.HStartCoroutine(AsyncRoutineKey, LoadYourAsyncScene(sceneIndex));
        }

        private IEnumerator LoadYourAsyncScene(int sceneIndex)
        {
            SceneChangingDelegate?.Invoke();

            var asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

            // Wait until the asynchronous scene fully loads
            yield return new WaitUntil(() => asyncLoad.isDone);

            SceneChangedDelegate?.Invoke();
            _coroutineManager.HStopCoroutine(AsyncRoutineKey);
        }
    }
}