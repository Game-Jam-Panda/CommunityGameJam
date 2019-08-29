using System;
using System.Collections;
using System.Collections.Generic;
using CGJ.Core;
using UnityEngine;

namespace CGJ.System
{
    public class NarratorSystem: MonoBehaviour
    {
        [SerializeField] AudioSource narratorAudiosource;
        List<AudioClip> voicelinesInQueue;
        AudioClip lastVoiceline;
        float minDelayBetweenVoicelines = 0.3f;
        bool canPlayVoiceline = true;
    
        public event Action onVoicelineStart;
        public event Action onVoicelieEnd;

        void Update()
        {
            //No voicelines to play
            if(voicelinesInQueue.Count < 1) { return; }

            // At least one voiceline to play
            if(canPlayVoiceline && voicelinesInQueue.Count >= 1)
            {
                StartCoroutine(PlayVoicelinesInQueue());
            }
        }

        public void TriggerVoiceline(AudioClip voicelineClip)
        {
            //TODO Add clip to queue
            voicelinesInQueue.Add(voicelineClip);
        }

        IEnumerator PlayVoicelinesInQueue()
        {
            foreach(AudioClip voiceline in voicelinesInQueue)
            {
                //Can't play another voice line
                canPlayVoiceline = false;

                //Play the desired voiceline & Remove it from the queue
                narratorAudiosource.PlayOneShot(voiceline);
                voicelinesInQueue.Remove(voicelinesInQueue[0]);
                onVoicelineStart();
                
                //Wait for the voiceline length + delay between each voiceline
                yield return new WaitForSeconds(voiceline.length + minDelayBetweenVoicelines);
                //Able to play another voiceline again
                canPlayVoiceline = true;
            }
        }
    }
}