using System.Collections;
using UnityEngine;

namespace CGJ.System
{
    public class StandaloneSystem : SystemManager
    {
        public GameObject playerInterface;

        private void Start()
        {
            instance = this;
            StartCoroutine(InitSystem());
        }

        private IEnumerator InitSystem()
        {
            // init stuff in ISystems interface
            yield return null;
        }
    }
}