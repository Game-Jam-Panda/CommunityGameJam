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
                //Disable this trigger's collider if one-time trigger
                if(oneTimeTrigger)
                {
                    triggerCollider.enabled = false;
                }


                // Trigger music
                if(musicToTrigger == null) { Debug.LogError(gameObject + " isn't referencing any music to trigger."); return;}
                SystemManager.systems.musicManager.StartCoroutine(SystemManager.systems.musicManager.TriggerMusic(musicToTrigger));
            }
        }
    }
}
