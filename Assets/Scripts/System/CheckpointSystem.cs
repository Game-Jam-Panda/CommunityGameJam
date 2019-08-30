using System;
using System.Collections;
using System.Collections.Generic;
using CGJ.Core;
using CGJ.Events;
using CGJ.Movement;
using CGJ.Pickups;
using UnityEngine;

namespace CGJ.System
{
    public class CheckpointSystem: MonoBehaviour
    {
        [Header("Respawn")]
        [SerializeField] float respawnFreezeCooldown = 0.2f;

        List<Pickup> pickupsThatCanBeLoaded = new List<Pickup>();

        Checkpoint lastCheckpoint = null;

        GameObject player = null;
        Camera cam = null;

        public event Action onCheckpointUpdate;

        public void UpdateCheckpoint(Checkpoint newCheckpoint)
        {
            // Update reference of last checkpoint
            lastCheckpoint = newCheckpoint;

            // Save unused pickups so that they respawn later
            SaveUnusedPickups();

            onCheckpointUpdate?.Invoke();
        }

        void SaveUnusedPickups()
        {
            // Clear the saved pickup list
            pickupsThatCanBeLoaded.Clear();

            // Save all pickups that aren't picked up ahead of the new checkpoint
            var pickups = GameObject.FindObjectsOfType<Pickup>();
            foreach(Pickup pickup in pickups)
            {
                if(!pickup.WasTriggered())
                {
                    pickupsThatCanBeLoaded.Add(pickup);
                }
            }
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

            //TODO re-enable pickups
            RestoreSavedPickups();
            StartCoroutine(RestorePlayerControls(playerMovement));
        }

        void RestoreSavedPickups()
        {
            foreach(Pickup pickup in pickupsThatCanBeLoaded)
            {
                //if(pickup.enabled) { continue; }

                pickup.SetWasTriggered(false);
                pickup.gameObject.SetActive(true);
            }
        }

        IEnumerator RestorePlayerControls(CharacterMovement playerMovement)
        {
            // Unfreeze after cd
            yield return new WaitForSeconds(respawnFreezeCooldown);
            playerMovement.UnfreezeControls();
        }
    }
}