using System;
using System.Collections;
using CGJ.Core;
using UnityEngine;

namespace CGJ.System
{
    public class NarratorSystem: MonoBehaviour
    {
        [SerializeField] AudioSource narratorAudiosource;
        AudioClip[] voicelinesInQueue;
        AudioClip lastVoiceline;
        float minDelayBetweenVoicelines = 0.3f;
        bool canPlayVoiceline = true;
    
        public event Action onVoicelineStart;
        public event Action onVoicelieEnd;

        void Update()
        {
            //No voicelines to play
            if(voicelinesInQueue.Length < 1) { return; }

            // At least one voiceline to play
            if(canPlayVoiceline && voicelinesInQueue.Length >= 1)
            {
                canPlayVoiceline = false;
                StartCoroutine(PlayVoicelinesInQueue());
            }
        }

        IEnumerator PlayVoicelinesInQueue()
        {
            foreach(AudioClip voiceline in voicelinesInQueue)
            {
                narratorAudiosource.PlayOneShot(voiceline);
                yield return new WaitForSeconds(voiceline.length);
                canPlayVoiceline = true;
            }
        }

        public void TriggerVoiceline(AudioClip voicelineClip)
        {
            //TODO Add clip to queue
            //voicelinesInQueue.Add(voicelineClip);
            //Play voiceline
            //lastVoiceline = voicelineToPlay;
            onVoicelineStart();
        }

        public void PlayVoiceline(AudioClip voicelineToPlay)
        {
            //TODO
        }
    }
}