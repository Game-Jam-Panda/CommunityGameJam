using System.Collections;
using System.Collections.Generic;
using CGJ.Utils;
using UnityEngine;

namespace CGJ.Traps
{
    public class FireTrap : Trap
    {
        [Header("Fire Trap settings")]
        [SerializeField] bool enableFire = true;
        [SerializeField] GameObject firePrefab = null;
        [SerializeField] Vector3 fireSpawnPosition;
        [Tooltip("Time to wait before triggering the trap after the moment it becomes possible.")]
        [SerializeField] float cooldownBetweenFires = 2.0f;
        float elapsedCooldownTime = 0.0f;

        void Start()
        {
            AttachBehaviourOnTrap();
        }

        void Update()
        {
            if(!enableFire) { return; }
            ProcessAutoTrigger();
        }

#region Trap processing

        private void ProcessAutoTrigger()
        {
            //Wait for trigger cooldown
            if (elapsedCooldownTime < cooldownBetweenFires)
            {
                elapsedCooldownTime += Time.deltaTime;
                return;
            }

            //It's time to Trigger the trap
            SpawnFire();
        }

        void SpawnFire()
        {
            Instantiate(firePrefab, fireSpawnPosition, Quaternion.identity);
            elapsedCooldownTime = 0.0f;
        }
#endregion
    }
}

