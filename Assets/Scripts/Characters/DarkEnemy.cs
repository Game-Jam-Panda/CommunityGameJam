using System.Collections;
using UnityEngine;

namespace CGJ.Characters
{
    [RequireComponent(typeof(AudioSource))]
    public class DarkEnemy : MonoBehaviour
    {
        [Header("Consume")]
        [SerializeField] GameObject consumeEffect = null;
        [SerializeField] AudioClip consumeSound = null;
        [SerializeField] float secondsToDisappear = 1.0f;
        AudioSource audioSource = null;

        [Header("Laugh")]
        [SerializeField] AudioClip[] evilLaughSounds = null;
        [SerializeField] float minLaughTimer = 2.0f;
        [SerializeField] float maxLaughTimer = 10.0f;
        float laughTimer = 0.0f;
        float elapsedLaughTime = 0.0f;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            ChooseRandomLaughTimer();
        }

        void Update()
        {
            if(evilLaughSounds == null) { return; }

            ProcessRandomLaugh();
        }

    #region Consume

        public IEnumerator Consume()
        {
            //Spawn consume effect
            Instantiate(consumeEffect, transform.position, Quaternion.identity);

            //Play consume sound
            PlayConsumedSound();

            // Destroy the enemy after consume time
            yield return new WaitForSeconds(secondsToDisappear);
            Destroy(gameObject);
        }

        public IEnumerator ConsumeWithoutSound()
        {
            //Spawn consume effect
            Instantiate(consumeEffect, transform.position, Quaternion.identity);

            // Destroy the enemy after consume time
            yield return new WaitForSeconds(secondsToDisappear);
            Destroy(gameObject);
        }

        public void PlayConsumedSound()
        {
            audioSource.PlayOneShot(consumeSound);
        }
    #endregion

    #region Laugh

        public void LaughEvil()
        {
            if(evilLaughSounds.Length < 1) { return; }

            // Play a random laugh sound
            var randomEvilLaugh = evilLaughSounds[Random.Range(0, evilLaughSounds.Length - 1)];
            audioSource.PlayOneShot(randomEvilLaugh);

            // Find a new laugh time and wait for timer to laugh again
            ChooseRandomLaughTimer();
        }

        void ProcessRandomLaugh()
        {
            if(elapsedLaughTime < laughTimer)
            {
                elapsedLaughTime += Time.deltaTime;
                return;
            }

            LaughEvil();
        }

        void ChooseRandomLaughTimer()
        {
            laughTimer = Random.Range(minLaughTimer, maxLaughTimer);
        }
    #endregion
    }
}
