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
            systems = new StandaloneSystems();

            systems.sceneLoadingSystem = gameObject.AddComponent<CGJSceneLoadingSystem>();
            systems.eventsSystem = gameObject.AddComponent<CGJEventsSystem>();
            systems.checkpointSystem = gameObject.AddComponent<CheckpointSystem>();
            systems.uiManager = gameObject.AddComponent<UIManager>();
            yield return null;
        }
    }

    public class StandaloneSystems : ISystems
    {
        public CGJSceneLoadingSystem sceneLoadingSystem { get; set; }
        public CGJEventsSystem eventsSystem { get; set; }
        public UIManager uiManager { get; set; }
        public CheckpointSystem checkpointSystem { get; set; }
    }
}