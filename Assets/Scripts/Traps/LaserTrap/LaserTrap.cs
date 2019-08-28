using System.Collections;
using System.Collections.Generic;
using CGJ.Utils;
using UnityEngine;

namespace CGJ.Traps
{
    public class LaserTrap : Trap
    {
        [Header("Laser Trap settings")]
        [SerializeField] bool toggleOverTime = false;
        [Tooltip("Time to wait before triggering the trap after the moment it becomes possible.")]
        [SerializeField] float triggerCooldown = 2.0f;
        [Tooltip("Time to wait before arming the trap after the moment it becomes possible.")]
        [SerializeField] float armCooldown = 0.5f;
        float elapsedTriggerTime = 0.0f;
        float elapsedArmingTime = 0.0f;

        [Tooltip("Time to wait before initializing the trap behaviour")]
        [SerializeField] bool startsLate = false;
        [SerializeField] float lateStartSeconds = 0.0f;
        float elapsedLateStartTime = 0.0f;

        bool triggering = false;
        bool arming = false;

        BoxCollider col;
        MeshRenderer meshRenderer;

        void Awake()
        {
            col = GetComponent<BoxCollider>();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        void Start()
        {
            AttachBehaviourOnTrap();

            EnableLaser();              // Disable laser at the beginning
            StartTriggeringTrap();      // Start the trap
        }

        void Update()
        {
            if(!toggleOverTime) { return; }

            ProcessAutoTrigger();
        }

#region Trap processing

        void ProcessAutoTrigger()
        {
            if(startsLate && lateStartSeconds > 0.0f)
            {
                if(elapsedLateStartTime < lateStartSeconds)
                {
                    elapsedLateStartTime += Time.deltaTime;
                    return;
                }
            }

            if(triggering)
            {
                ProcessTrapTriggering();
                return;
            }
            if(arming)
            {
                ProcessTrapArming();
            }
        }

        private void ProcessTrapTriggering()
        {
            //Wait for trigger cooldown
            if (elapsedTriggerTime < triggerCooldown)
            {
                elapsedTriggerTime += Time.deltaTime;
                return;
            }

            //It's time to Trigger the trap
            EnableLaser();
            StartArmingTrap();
        }
        private void ProcessTrapArming()
        {
            //Wait for arming cooldown
            if(elapsedArmingTime < armCooldown)
            {
                elapsedArmingTime += Time.deltaTime;
                return;
            }
            
            //It's time to Arm the trap
            DisableLaser();
            StartTriggeringTrap();
        }
#endregion

#region Trap triggers
        public void EnableLaser ()
        {
            col.enabled = true;
            meshRenderer.enabled = true;
        }
        public void DisableLaser ()
        {
            col.enabled = false;
            meshRenderer.enabled = false;
        }

        public void StartTriggeringTrap()
        {
            elapsedArmingTime = 0.0f;
            arming = false;

            //Start triggering
            triggering = true;
            
        }
        public void StartArmingTrap()
        {
            elapsedTriggerTime = 0.0f;
            triggering = false;

            //Start arming
            arming = true;
        }
    }
#endregion
}

