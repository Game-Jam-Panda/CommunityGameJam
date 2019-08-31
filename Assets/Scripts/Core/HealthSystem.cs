using System;
using System.Collections;
using System.Collections.Generic;
using CGJ.Mechanics;
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
    
        [Header("Health")]
        [SerializeField] int maxHearts = 3;
        int currentHealth = 3;
        bool alive = true;

        //[Header("Shield")]
        ShieldMechanic shieldMechanic = null;
        int currentShieldsAmount = 0;

        [Header("Damage settings")]
        [SerializeField] AudioClip[] damageSFXArray;

        [Header("Death")]
        [SerializeField] float deathTime = 1.0f;
        
        //Animator
        const string ANIM_ALIVE = "alive";
        Animator anim = null;
        AudioSource audioSource = null;

        public event Action onHealthChange;
        public event Action onDeath;
    
        // Delegate Subscription
        void OnEnable(){
            onHealthChange += UpdateHealthUI;
        }
        void OnDisable(){
            onHealthChange -= UpdateHealthUI;
        }

        //Health
        public int GetMaxHearts() { return maxHearts; }
        public int GetCurrentHealth() { return currentHealth; }

        //Shield - Setters
        public void SetShieldValue(int shieldValue) { currentShieldsAmount = shieldValue; }

        void Start()
        {
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            shieldMechanic = GetComponent<ShieldMechanic>();

            // Setup health on start
            onHealthChange?.Invoke();
        }
    
        void Update()
        {
            UpdateAnimator();

            //Make sure the current health doesn't exceed max hearts
            if(currentHealth > maxHearts)
            {
               currentHealth = maxHearts;
            }
        }
    
    #region Animations
        void UpdateAnimator()
        {
            anim.SetBool(ANIM_ALIVE, alive);
        }
    #endregion

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
            if (!alive) { return; }

            //Play random damage sound
            var randomDamageSFX = damageSFXArray[UnityEngine.Random.Range(0, damageSFXArray.Length)];
            audioSource.PlayOneShot(randomDamageSFX);

            //New health stored value
            int newHealth;

            // Possibly Shield from damage
            if (shieldMechanic.GetCurrentShieldsAmount() >= 1)
            {
                shieldMechanic.RemoveShield(1);
                newHealth = RemoveHealth(damage - 1);
            }
            else
            {
                newHealth = RemoveHealth(damage);
            }

            // Die
            if (newHealth <= 0)
            {
                StartCoroutine(Die()); return;
            }
        }

        private int RemoveHealth(int damage)
        {
            // Remove health
            int newHealth = Mathf.Clamp(currentHealth - damage, 0, maxHearts);
            currentHealth = newHealth;
            onHealthChange?.Invoke();
            return newHealth;
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
    #endregion
    }
}