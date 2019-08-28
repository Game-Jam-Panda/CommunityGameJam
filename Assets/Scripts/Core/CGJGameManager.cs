using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Core
{
    public class CGJGameManager : MonoBehaviour
    {
        CGJSceneManager sceneManager;

        void Awake()
        {
            //Always carry the game manager in every scene
            DontDestroyOnLoad(this);

            //Get references to managers attached to the game manager
            sceneManager = GetComponent<CGJSceneManager>();
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
            
            Application.Quit();
        }
    }

}