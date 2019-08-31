using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.System
{
    public class SoundManager : MonoBehaviour
    {
        AudioSource managerAudioSource = null;

        void Awake()
        {
            // Spawn generic SoundManager AudioSource
            if(managerAudioSource == null)
            { 
                managerAudioSource = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                managerAudioSource = GetComponent<AudioSource>();
            }
            
            managerAudioSource.playOnAwake = false;
        }

        public void PlaySound(AudioClip audioClip, float volume = 1.0f)
        {
            managerAudioSource.PlayOneShot(audioClip, volume);
        }

        public void PlayRandomSound(AudioClip[] sounds, float volume = 1.0f)
        {
            var randomSound = sounds[UnityEngine.Random.Range(0, sounds.Length)];

            managerAudioSource.PlayOneShot(randomSound, volume);
        }
    }
}
