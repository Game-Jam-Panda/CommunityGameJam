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

        IEnumerator InitSystem()
        {
            // init stuff in ISystems interface
            systems.sceneLoadingSystem = gameObject.AddComponent<CGJSceneLoadingSystem>();
            systems.eventsSystem = gameObject.AddComponent<CGJEventsSystem>();
            systems.uiManager = gameObject.AddComponent<UIManager>();
            systems.checkpointSystem = gameObject.AddComponent<CheckpointSystem>();
            yield return null;
        }
    }
}