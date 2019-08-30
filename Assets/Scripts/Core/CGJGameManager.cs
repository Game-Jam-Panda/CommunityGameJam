using CGJ.System;
using UnityEngine;

namespace CGJ.Core
{
    public class CGJGameManager : MonoBehaviour
    {
        // Static singleton property
        public static CGJGameManager instance { get; private set; }
     
        void Awake()
        {
            // Destroy any other GameManager that's trying to newly load
            if(instance != null && instance != this)
            {
                Destroy(gameObject);
            }
    
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
    }
}