using System;
using System.Collections;
using CGJ.Core;
using CGJ.Events;
using CGJ.Movement;
using UnityEngine;

namespace CGJ.System
{
    public class CheckpointSystem: MonoBehaviour
    {
        [Header("Respawn")]
        [SerializeField] float respawnFreezeCooldown = 0.2f;

        Checkpoint lastCheckpoint = null;

        GameObject player = null;
        Camera cam = null;

        public event Action onCheckpointUpdate;

        public void UpdateCheckpoint(Checkpoint newCheckpoint)
        {
            // Update reference of last checkpoint
            lastCheckpoint = newCheckpoint;

            onCheckpointUpdate?.Invoke();
        }

        public void RespawnToLastCheckpoint()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var cam = Camera.main;
            var playerMovement = player.GetComponent<CharacterMovement>();
            var playerHealth = player.GetComponent<HealthSystem>();

            //Freeze
            playerMovement.FreezeControls();
            
            //Update player position
            player.transform.position = lastCheckpoint.GetCheckpointRespawnPoint().position;
            player.transform.rotation = lastCheckpoint.GetCheckpointRespawnPoint().rotation;
            //Update camera position
            cam.transform.position = lastCheckpoint.GetCheckpointRespawnPoint().position;

            //Restore health
            playerHealth.RestoreHealth();

            StartCoroutine(RestorePlayerControls(playerMovement));
        }

        public IEnumerator RestorePlayerControls(CharacterMovement playerMovement)
        {
            // Unfreeze after cd
            yield return new WaitForSeconds(respawnFreezeCooldown);
            playerMovement.UnfreezeControls();
        }
    }
}