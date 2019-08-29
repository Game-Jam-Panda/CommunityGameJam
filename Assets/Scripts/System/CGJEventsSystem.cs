using System;
using CGJ.Core;
using UnityEngine;

namespace CGJ.System
{
    public class CGJEventsSystem: MonoBehaviour
    {
        [SerializeField] GameObject checkpointNotificationPosition;

        public event Action onButtonClicked;

        private void OnEnable() {
            //Checkpoint
            //SystemManager.systems.checkpointSystem.onCheckpointUpdate += ActivateCheckpointLantern;
        }
        private void OnDisable(){
            //Checkpoint
            //SystemManager.systems.checkpointSystem.onCheckpointUpdate -= ActivateCheckpointLantern;
        }

        public void QuitGame()
        {
            onButtonClicked();
            
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
            
            Application.Quit();
        }

        public void ActivateCheckpointLantern(Checkpoint checkpoint)
        {
            var checkpointLantern = checkpoint.GetCheckpointLantern();
            checkpointLantern.SetActive(true);
        }
    }
}