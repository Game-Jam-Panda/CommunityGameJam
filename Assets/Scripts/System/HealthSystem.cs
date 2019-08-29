using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CGJ.System
{
    public class HealthSystem : MonoBehaviour
    {
        [Header("Health UI")]
        [SerializeField] Image[] hearts;
        [SerializeField] Sprite fullHeart;
        [SerializeField] Sprite emptyHeart;
    
        //TODO Remove exposure
        [SerializeField] int maxHearts = 3;
        [SerializeField] int currentHealth = 1;

        [SerializeField] AudioClip[] damageSFXArray;
    
        public event Action onHealthChange;
    
        // Delegate Subscription
        void OnEnable()
        { onHealthChange += UpdateHealthUI; }
        void OnDisable()
        { onHealthChange -= UpdateHealthUI; }

        public int GetMaxHearts() { return maxHearts; }
        public int GetCurrentHealth() { return currentHealth; }

        void Start()
        {
            // Setup health on start
            onHealthChange();
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
        
        public void TakeDamage(int damage)
        {
            //Remove health
            int newHealth = Mathf.Clamp(currentHealth - damage, 0, maxHearts);
            currentHealth = newHealth;

            //Play random damage sound
            var randomDamageSFX = damageSFXArray[UnityEngine.Random.Range(0, damageSFXArray.Length)];
            GetComponent<AudioSource>().PlayOneShot(randomDamageSFX);

            onHealthChange();
        }
        public void Heal(int healAmount)
        {
            //Add health
            int newHealth = Mathf.Clamp(currentHealth + healAmount, currentHealth, maxHearts);
            currentHealth = newHealth;
            onHealthChange();
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
    }
}