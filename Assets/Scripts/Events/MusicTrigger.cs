using System.Collections;
using System.Collections.Generic;
using CGJ.System;
using UnityEngine;

namespace CGJ.Events
{
    public class MusicTrigger : Trigger
    {
        [Header("Music")]
        [SerializeField] AudioClip musicToTrigger = null;
        [SerializeField] float triggerDelay = 0.0f;

        private void OnTriggerEnter(Collider col)
        {
            if(col.tag == "Player")
            {
                if(musicToTrigger == null) { Debug.LogError(gameObject + " isn't referencing any music to trigger."); return;}

                // Trigger music
                SystemManager.systems.musicManager.StartCoroutine(SystemManager.systems.musicManager.TriggerMusic(musicToTrigger));


                //Disable this trigger's collider if enabled
                if(oneTimeTrigger)
                {
                    triggerCollider.enabled = false;
                }
            }
        }
    }
}
