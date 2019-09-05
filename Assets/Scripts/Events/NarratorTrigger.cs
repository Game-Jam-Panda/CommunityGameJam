using System.Collections;
using System.Collections.Generic;
using CGJ.System;
using UnityEngine;

namespace CGJ.Events
{
    public class NarratorTrigger : Trigger
    {
        [Header("Voiceline")]
        [SerializeField] AudioClip voicelineToTrigger = null;

        private void OnTriggerEnter(Collider col)
        {
            if(col.tag == "Player")
            {
                //Disable this trigger's collider if one-time trigger
                if(oneTimeTrigger)
                {
                    triggerCollider.enabled = false;
                }


                if(voicelineToTrigger == null) { Debug.LogError(gameObject + " isn't referencing any voiceline to trigger."); return;}

                // Add voiceline to current voicelines queue
                SystemManager.systems.narratorSystem.TriggerVoiceline(voicelineToTrigger);
            }
        }
    }
}
