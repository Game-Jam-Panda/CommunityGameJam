using System;
using System.Collections;
using CGJ.System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CGJ.System
{
    public class CGJSceneLoadingSystem: MonoBehaviour
    {
        //IMPORTANT - Manually reference a fixed index of the loading screen
        const int LOADING_SCREEN_INDEX = 1;

        int currentScene = -1;
        int sceneToLoad = -1;

        //Scene index
        public int GetLoadingScreenIndex() { return LOADING_SCREEN_INDEX; }
        public int GetPreviousScene() { return currentScene; }
        public int GetSceneToLoad() { return sceneToLoad; }

        public int GetCurrentSceneIndex() 
        { return SceneManager.GetActiveScene().buildIndex; }

        public event Action onSceneLoad;
        public event Action onSceneLoaded;

    #region Scene loading
        public void LoadSceneByIndex(int sceneToLoad)
        {
            //Update scenes index references
            UpdateCurrentSceneValue();
            this.sceneToLoad = sceneToLoad;

            StartCoroutine(LoadScene());
        }

        public void LoadNextScene()
        {
            //Update scenes index references
            UpdateCurrentSceneValue();
            sceneToLoad = GetCurrentSceneIndex() + 1;

            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            onSceneLoad();

            //Loading screen
            SceneManager.LoadScene(SystemManager.systems.sceneLoadingSystem.GetLoadingScreenIndex());
            yield return new WaitForSeconds(0.2f);

            //Load desired scene during the loading screen
            SceneManager.LoadSceneAsync(SystemManager.systems.sceneLoadingSystem.GetSceneToLoad());
            UpdateCurrentSceneValue();
            onSceneLoaded();
        }

        private void UpdateCurrentSceneValue()
        {
            currentScene = GetCurrentSceneIndex();
        }
    #endregion
    }
}