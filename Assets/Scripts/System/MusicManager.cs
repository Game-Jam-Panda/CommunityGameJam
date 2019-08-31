using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.System
{
    public class MusicManager : MonoBehaviour
    {
        [Header("Volume")]
        [SerializeField] float maxVolume = 1.0f;
        [SerializeField] float fadeOutMinVolumeTreshold = 0.0f;

        [Header("Fading settings")]
        [SerializeField] float fadeInTime = 2.0f;
        [SerializeField] float fadeOutTime = 2.0f;

        AudioSource musicAudioSource = null;

        void Awake()
        {
            musicAudioSource = GetComponent<AudioSource>();
            
            //musicAudioSource.playOnAwake = true;
        }

        public IEnumerator TriggerMusic(AudioClip musicClip)
        {
            // Don't play triggered music if it's the same that's already playing
            if(musicClip == musicAudioSource.clip) { yield break; }

            //Fade Out current music
            if(musicAudioSource.clip != null)
            {
                var fadeOut = StartCoroutine(FadeOutMusic());
                yield return fadeOut;
            }

            //Set and Fade In music to play
            musicAudioSource.clip = musicClip;
            musicAudioSource.Play();

            var fadeIn = StartCoroutine(FadeInMusic());
            yield return fadeIn;
        }

        IEnumerator FadeInMusic()
        {
            while(musicAudioSource.volume < maxVolume)
            {
                musicAudioSource.volume += fadeInTime * Time.deltaTime;
                yield return null;
            }
        }
        IEnumerator FadeOutMusic()
        {
            while(musicAudioSource.volume > fadeOutMinVolumeTreshold)
            {
                musicAudioSource.volume -= fadeOutTime * Time.deltaTime;
                yield return null;
            }
        }
    }
}
