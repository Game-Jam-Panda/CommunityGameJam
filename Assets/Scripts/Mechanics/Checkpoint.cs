using System.Collections;
using System.Collections.Generic;
using CGJ.System;
using UnityEngine;

namespace CGJ.Core
{
    public class Checkpoint : MonoBehaviour
    {
        [Header("Checkpoint Update")]
        [SerializeField] Light lanternLight = null;
        [SerializeField] AudioClip checkpointUpdateSE = null;

        [Header("Respawn")]
        [SerializeField] Transform respawnPoint = null;

        public Light GetLanternLight() { return lanternLight; }
        public Transform GetCheckpointRespawnPoint() { return respawnPoint; }

        private void OnTriggerEnter(Collider col)
        {
            // When the player enters checkpoint's collider
            if(col.tag == "Player")
            {
                SystemManager.systems.checkpointSystem.UpdateCheckpoint(this);

                // Play checkpoint update Sound Effect
                if (checkpointUpdateSE != null) { SystemManager.systems.soundManager.PlaySound(checkpointUpdateSE); }

                //Turn on checkpoint Light
                if(lanternLight != null) { lanternLight.enabled = true; }

                //Disable this checkpoint's collider
                var checkpointCollider = GetComponent<BoxCollider>();
                checkpointCollider.enabled = false;
            }
        }
    }
}
