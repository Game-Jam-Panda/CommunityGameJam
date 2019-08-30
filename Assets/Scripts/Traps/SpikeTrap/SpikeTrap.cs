using System.Collections;
using System.Collections.Generic;
using CGJ.Utils;
using UnityEngine;

namespace CGJ.Traps
{
    public class SpikeTrap : Trap
    {
        [Header("Spike Trap settings")]
        [SerializeField] bool alwaysActive = false;
        [Tooltip("Time to wait before triggering the trap after the moment it becomes possible.")]
        [SerializeField] float triggerCooldown = 2.0f;
        [Tooltip("Time to wait before arming the trap after the moment it becomes possible.")]
        [SerializeField] float armCooldown = 0.5f;
        float elapsedTriggerTime = 0.0f;
        float elapsedArmingTime = 0.0f;
        bool ableToTriggerTrap = true;
        bool ableToArmTrap = false;

        //Animation Triggering
        bool open = false;
        Animator anim;
        BoxCollider col;
        
        const string OPEN_TRIGGER = "Open";
        const string CLOSE_TRIGGER = "Close";

        void Awake()
        {
            anim = GetComponent<Animator>();
            col = GetComponent<BoxCollider>();
        }

        void Start()
        {
            AttachBehaviourOnTrap();

            //When trap is always Always active
            if(alwaysActive)
            {
                InstantTriggerOnce();
                return;
            }

            //When trap toggles
            DisableCollider();     // Disable trap collider in the beginning for safety
            AbleToTriggerTrap();   //Trap needs to able to trigger at the beginning to start the animation chain
        }

        void Update()
        {
            if(alwaysActive) { return; }

            ProcessAutoTrigger();
        }

#region Trap processing

        //--- Always active mode ---
        void InstantTriggerOnce()
        {
            CloseSpikes();
        }

        //--- Auto trigger mode ---
        void ProcessAutoTrigger()
        {
            if(ableToTriggerTrap)
            {
                ProcessTrapTriggering();
                return;
            }
            if(ableToArmTrap)
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
            CloseSpikes();
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
            OpenSpikes();
        }

    #region Trap Toggles
        private void CloseSpikes()
        {
            SetAnimTrigger(CLOSE_TRIGGER);
        }
        private void OpenSpikes()
        {
            SetAnimTrigger(OPEN_TRIGGER);
        }
    #endregion
#endregion

#region Animator parameters
        //Trigger a parameter in the animator
        void SetAnimTrigger(string triggerName)
        {
            anim.SetTrigger(triggerName);
        }
#endregion

#region Animation events

        // Animation events
        public void EnableCollider ()
        { col.enabled = true; }
        public void DisableCollider ()
        { col.enabled = false; }

        //----TRIGERRING TRAP----
        public void UnableToTriggerTrap()   //Start of triggering
        {
            //Disable Triggering
            ableToTriggerTrap = false;
            elapsedTriggerTime = 0.0f;
        }
        public void AbleToTriggerTrap()     //End of arming
        {
            //Enable Triggering
            ableToTriggerTrap = true;
        }

        //----ARMING TRAP----
        public void UnableToArmTrap()   //Start of arming
        {
            //Disable Arming
            ableToArmTrap = false;
            elapsedArmingTime = 0.0f;
        }
        public void AbleToArmTrap()     //End of triggering
        {
            //Enable Arming
            ableToArmTrap = true;
        }
#endregion
        
    }
}