using System.Collections;
using System.Collections.Generic;
using CGJ.System;
using UnityEngine;

namespace CGJ.Events
{
    public class SoundTrigger : Trigger
    {
        [Header("Sound")]
        [SerializeField] AudioClip soundToTrigger = null;
        [SerializeField] float triggerDelay = 0.0f;

        private void OnTriggerEnter(Collider col)
        {
            if(col.tag == "Player")
            {
                if(soundToTrigger == null) { Debug.LogError(gameObject + " isn't referencing any sound to trigger."); return;}
    
                // Trigger sound, possibly after a delay
                if(triggerDelay > 0.0f)
                {
                    StartCoroutine(TriggerSoundAfterDelay());
                }
                else
                {
                    TriggerSound();
                }


                //Disable this trigger's collider if enabled
                if(oneTimeTrigger)
                {
                    triggerCollider.enabled = false;
                }
            }
        }

        void TriggerSound()
        {
            SystemManager.systems.soundManager.PlaySound(soundToTrigger);
        }
        IEnumerator TriggerSoundAfterDelay()
        {
            //Wait for delay
            yield return new WaitForSeconds(triggerDelay);

            // Play sound effect
            SystemManager.systems.soundManager.PlaySound(soundToTrigger);
        }
    }
}
