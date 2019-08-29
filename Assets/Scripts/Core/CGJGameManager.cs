using CGJ.System;
using UnityEngine;

namespace CGJ.Core
{
    public class CGJGameManager : MonoBehaviour
    {
        void Awake()
        {
            //Always carry the game manager in every scene
            DontDestroyOnLoad(this);
        }
    }
}