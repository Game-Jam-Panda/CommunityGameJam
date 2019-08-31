using System.Collections;
using System.Collections.Generic;
using CGJ.System;
using UnityEngine;

namespace CGJ.Events
{
    public class Checkpoint : Trigger
    {
        [Header("Checkpoint Update")]
        [SerializeField] Light lanternLight = null;
        [SerializeField] AudioClip checkpointUpdateSE = null;
        [Range(0.01f, 1.0f)] [SerializeField] float checkpointUpdateVolume = 0.1f;

        [Header("Respawn")]
        [SerializeField] Transform respawnPoint = null;

        public Light GetLanternLight() { return lanternLight; }
        public Transform GetCheckpointRespawnPoint() { return respawnPoint; }

        private void OnTriggerEnter(Collider col)
        {
            if(col.tag == "Player")
            {
                //Update latest checkpoint to this one
                SystemManager.systems.checkpointSystem.UpdateCheckpoint(this);

                // Play checkpoint update Sound Effect
                if (checkpointUpdateSE != null) { SystemManager.systems.soundManager.PlaySound(checkpointUpdateSE, checkpointUpdateVolume); }
                // Turn on checkpoint Light
                if(lanternLight != null) { lanternLight.enabled = true; }


                //Disable this trigger's collider if enabled
                if(oneTimeTrigger)
                {
                    triggerCollider.enabled = false;
                }
            }
        }
    }
}
