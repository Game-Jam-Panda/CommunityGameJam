using System.Collections;
using System.Collections.Generic;
using CGJ.System;
using UnityEngine;

namespace CGJ.Core
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] GameObject checkpointLantern = null;

        public GameObject GetCheckpointLantern() { return checkpointLantern; }

        private void OnTriggerEnter(Collider col)
        {
            // When the player enters checkpoint's collision
            if(col.tag == "Player")
            {
                SystemManager.systems.checkpointSystem.UpdateCheckpoint(this);
            }
        }
    }
}
