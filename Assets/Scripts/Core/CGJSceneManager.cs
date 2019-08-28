using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CGJ.Core
{
    public class CGJSceneManager: MonoBehaviour
    {
        //IMPORTANT - Reference a fixed index of the loading screen
        const int LOADING_SCREEN_INDEX = 1;

        int previousScene = -1;
        int sceneToLoad = -1;

        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void LoadSceneByIndex(int sceneToLoad)
        {
            //Update scenes index references
            previousScene = GetCurrentSceneIndex();
            this.sceneToLoad = sceneToLoad;

            StartCoroutine(LoadScene());
        }

        public void LoadNextScene()
        {
            //Update scenes index references
            previousScene = GetCurrentSceneIndex();
            sceneToLoad = previousScene + 1;

            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            SceneManager.LoadScene(LOADING_SCREEN_INDEX);           //Loading screen
            yield return new WaitForSeconds(1);
            SceneManager.LoadSceneAsync(this.sceneToLoad);           //Load desired scene during the loading screen
            yield return null;
        }

        private int GetCurrentSceneIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }
    }
}