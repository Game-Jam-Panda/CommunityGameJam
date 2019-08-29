using System.Collections;
using System.Collections.Generic;
using CGJ.Utils;
using UnityEngine;

namespace CGJ.Traps
{
    public class FireTrap : Trap
    {
        [Header("Fire Trap settings")]
        
        [SerializeField] bool toogleFireOverTime = false;
        [SerializeField] GameObject firePrefab = null;
        [SerializeField] Vector3 fireSpawnPosition;
        [Tooltip("Time to wait before triggering the trap after the moment it becomes possible.")]
        [SerializeField] float cooldownBetweenFires = 2.0f;
        float elapsedCooldownTime = 0.0f;
        bool fireEnabled = true;

        void Start()
        {
            AttachBehaviourOnTrap();

            // Make sure the Fire is enabled in the beginning
            if(fireEnabled == false) { ToggleFire(); }
        }

        void Update()
        {
            // Toggle Fire over time if enabled
            if(toogleFireOverTime)
            {
                ProcessAutoTrigger();
            }
        }

#region Trap processing

        
        void ProcessAutoTrigger()
        {
            //Wait for trigger cooldown
            if (elapsedCooldownTime < cooldownBetweenFires)
            {
                elapsedCooldownTime += Time.deltaTime;
                return;
            }

            //It's time to Trigger the trap
            ToggleFire();
        }

        private void ToggleFire()
        {
            fireEnabled = !fireEnabled;
            elapsedCooldownTime = 0.0f;
        }
#endregion
    }
}

