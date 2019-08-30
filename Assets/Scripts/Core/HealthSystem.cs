using System;
using System.Collections;
using System.Collections.Generic;
using CGJ.System;
using UnityEngine;
using UnityEngine.UI;

namespace CGJ.Core
{
    public class HealthSystem : MonoBehaviour
    {
        [Header("Health UI")]
        [SerializeField] Image[] hearts;
        [SerializeField] Sprite fullHeart;
        [SerializeField] Sprite emptyHeart;
    
        //TODO Remove exposure
        [Header("Health")]
        [SerializeField] int maxHearts = 3;
        int currentHealth = 3;
        
        bool alive = true;

        //[Header("Shield")]
        int shieldedHealth = 0;

        [Header("Damage settings")]
        [SerializeField] AudioClip[] damageSFXArray;

        [Header("Death")]
        [SerializeField] float deathTime = 1.0f;
        
        AudioSource audioSource = null;

        public event Action onHealthChange;
        public event Action onDeath;
    
        // Delegate Subscription
        void OnEnable()
        { onHealthChange += UpdateHealthUI; }
        void OnDisable()
        { onHealthChange -= UpdateHealthUI; }

        //Health
        public int GetMaxHearts() { return maxHearts; }
        public int GetCurrentHealth() { return currentHealth; }

        //Shield - Setters
        public void SetShieldValue(int shieldValue) { shieldedHealth = shieldValue; }

        void Start()
        {
            audioSource = GetComponent<AudioSource>();

            // Setup health on start
            onHealthChange?.Invoke();
        }
    
        void Update()
        {
            //Make sure the current health doesn't exceed max hearts
            if(currentHealth > maxHearts)
            {
               currentHealth = maxHearts;
            }

            //TODO REMOVE AFTER FINISHING TESTS 
            DamageTesting();
        }
    
    #region Health UI
        void UpdateHealthUI()
        {
            for(int i = 0; i < hearts.Length; i++)
            {
                // Update Hearts sprites
                if(i < currentHealth)
                { hearts[i].sprite = fullHeart; }
                else
                { hearts[i].sprite = emptyHeart; }
    
                // Update total Hearts number
                if(i < maxHearts)
                { hearts[i].enabled = true; }
                else
                { hearts[i].enabled = false; }
            }
        }
    #endregion
    
    #region Health Calls
    
        public void TakeDamage(int damage)
        {
            if(!alive) { return; }

            // Possibly Shield from damage
            if(shieldedHealth >= 1) 
            {
                shieldedHealth -= 1;
                return;
            }
            
            int newHealth = Mathf.Clamp(currentHealth - damage, 0, maxHearts);

            //Play random damage sound
            var randomDamageSFX = damageSFXArray[UnityEngine.Random.Range(0, damageSFXArray.Length)];
            audioSource.PlayOneShot(randomDamageSFX);

            // Remove health
            currentHealth = newHealth;
            onHealthChange?.Invoke();

            // Die
            if(newHealth <= 0)
            {
                StartCoroutine(Die()); return;
            }
        }
        public void Heal(int healAmount)
        {
            if(!alive) { return; }

            //Add health
            int newHealth = Mathf.Clamp(currentHealth + healAmount, currentHealth, maxHearts);
            currentHealth = newHealth;
            onHealthChange();
        }
        public void RestoreHealth()
        {
            currentHealth = maxHearts;
            alive = true;
            onHealthChange();
        }

        IEnumerator Die()
        {
            alive = false;
            
            onDeath();
            
            //Death sound
            // audioSource.PlayOneShot(deathSound);

            // Wait for death time and respawn to last checkpoint
            yield return new WaitForSeconds(deathTime);
            SystemManager.systems.checkpointSystem.RespawnToLastCheckpoint();
        }

        //TODO REMOVE AFTER FINISHING TESTS
        void DamageTesting()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                TakeDamage(1);
            }
            if(Input.GetKeyDown(KeyCode.E))
            {
                Heal(1);
            }
            if(Input.GetKeyDown(KeyCode.T))
            {
                TakeDamage(maxHearts);
            }
        }
    #endregion
    }
}