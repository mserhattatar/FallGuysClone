using System;
using System.Collections;
using Game.Scripts.Container;
using Game.Scripts.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Manager
{
    public class GameSceneManager : ComponentContainerBehaviour
    {
        public event Action SceneChangingDelegate;
        public event Action SceneChangedDelegate;

        private LoadingCanvasController _loadingCanvasController;
        public override void ContainerDoAfterAwake()
        {
            _loadingCanvasController = MainContainer.GetContainerComponent(nameof(LoadingCanvasController)) as LoadingCanvasController;
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
            StopAllCoroutines();
            StartCoroutine(LoadYourAsyncScene(sceneIndex));
        }

        private IEnumerator LoadYourAsyncScene(int sceneIndex)
        {
            SceneChangingDelegate?.Invoke();
            _loadingCanvasController.Set(true);
            var asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

            
            // Wait until the asynchronous scene fully loads
            yield return new WaitUntil(() => asyncLoad.isDone);
            
            yield return new WaitForSeconds(1f);
            _loadingCanvasController.Set(false);
            
            SceneChangedDelegate?.Invoke();
            StopAllCoroutines();
        }
    }
}