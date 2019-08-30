using System;
using UnityEngine;

namespace CGJ.System
{
    public class UIManager: MonoBehaviour
    {
        [Header("Notification settings")]
        [SerializeField] GameObject notificationOutlet = null;

        [Header("Checkpoint")]
        [SerializeField] GameObject checkpointNotification = null;
        
        //private void OnEnable() {
        //    //Checkpoint
        //    SystemManager.systems.checkpointSystem.onCheckpointUpdate += SpawnCheckpointNotification;
        //}
        //private void OnDisable(){
        //    //Checkpoint
        //    SystemManager.systems.checkpointSystem.onCheckpointUpdate -= SpawnCheckpointNotification;
        //}

    #region Checkpoint

        // Checkpoint reached UI notification
        public void SpawnCheckpointNotification()
        {
            Instantiate(checkpointNotification, notificationOutlet.transform.position, Quaternion.identity, notificationOutlet.transform);
        }
    #endregion
    }
}