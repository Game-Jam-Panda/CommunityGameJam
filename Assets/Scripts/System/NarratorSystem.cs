using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.System
{
    public class NarratorSystem: MonoBehaviour
    {
        [SerializeField] float minDelayBetweenVoicelines = 0.3f;

        AudioSource narratorAudiosource = null;
        List<AudioClip> voicelinesInQueue = new List<AudioClip>();
        
        bool canPlayVoiceline = true;

        void Awake()
        {
            // Reference Narrator AudioSource
            if(narratorAudiosource == null)
            { 
                narratorAudiosource = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                narratorAudiosource = GetComponent<AudioSource>();
            }

            narratorAudiosource.playOnAwake = false;
        }

        void Update()
        {
            if(voicelinesInQueue.Count < 1) { return; }

            // At least one voiceline to play
            if(canPlayVoiceline && voicelinesInQueue.Count >= 1)
            {
                StartCoroutine(PlayVoicelinesInQueue());
            }
        }

        public void TriggerVoiceline(AudioClip voicelineClip)
        {
            //Add clip to queue
            voicelinesInQueue.Add(voicelineClip);
        }

        IEnumerator PlayVoicelinesInQueue()
        {
            //Can't play another voice line
            canPlayVoiceline = false;

            //Play the desired voiceline & Remove it from the queue
            var firstVoicelineInQueue = voicelinesInQueue[0];
            narratorAudiosource.PlayOneShot(firstVoicelineInQueue);
            voicelinesInQueue.Remove(firstVoicelineInQueue);

            //Wait for the voiceline length + delay between each voiceline
            yield return new WaitForSeconds(firstVoicelineInQueue.length);
            if(voicelinesInQueue.Count > 1) { yield return new WaitForSeconds(minDelayBetweenVoicelines); } //Additional delay
            
            //Able to play another voiceline again
            canPlayVoiceline = true;
        }
    }
}