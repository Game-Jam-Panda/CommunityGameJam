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

            //Add systems
            systems.sceneLoadingSystem = gameObject.AddComponent<CGJSceneLoadingSystem>();
            systems.eventsSystem = gameObject.AddComponent<CGJEventsSystem>();
            systems.checkpointSystem = gameObject.AddComponent<CheckpointSystem>();
            systems.soundManager = gameObject.AddComponent<SoundManager>();
            systems.uiManager = gameObject.AddComponent<UIManager>();

            //Get systems
            systems.narratorSystem = FindObjectOfType<NarratorSystem>();
            systems.soundManager = FindObjectOfType<SoundManager>();
            yield return null;
        }
    }

    public class StandaloneSystems : ISystems
    {
        public CGJSceneLoadingSystem sceneLoadingSystem { get; set; }
        public CGJEventsSystem eventsSystem { get; set; }
        public UIManager uiManager { get; set; }
        public CheckpointSystem checkpointSystem { get; set; }
        public SoundManager soundManager { get; set; }
        public NarratorSystem narratorSystem { get; set; }
    }
}