using System;
using System.Collections;
using System.Collections.Generic;
using CGJ.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CGJ.Mechanics
{
    public class ShieldMechanic : MonoBehaviour
    {
        [Header("Shield UI")]
        [SerializeField] GameObject shieldTimer = null;
        [SerializeField] Image shieldTimerBar = null;
        [SerializeField] Text shieldTimerText = null;
        [SerializeField] Text shieldAmountText = null;

        [Header("Shield Timer")]
        [SerializeField] float shieldTemporaryTime = 5.0f;
        float shieldRemainingTime = 0.0f;

        //Shield values
        int shieldAmount = 0;

        HealthSystem playerHealth;

        public int GetShieldAmount() { return shieldAmount; }

        void Awake()
        {
            playerHealth = GetComponent<HealthSystem>();
        }
        
        void Start()
        {
            UpdateAllShieldUI();
        }

        void Update()
        {
            // Bar visibility
            if(shieldAmount > 0)
            { ShowTimerBar(); }
            else
            { HideTimerBar(); }

            if(Input.GetKeyDown(KeyCode.F)){ AddShield(1); }   //Simulate shield pickup //TODO REMOVE

            // Reduce shield timer as long you you have a shield
            if(shieldAmount > 0 && shieldRemainingTime > 0)
            {
                shieldRemainingTime -= Time.deltaTime;
                UpdateShieldTimerText();
                return;
            }

            if(shieldRemainingTime <= 0)
            {
                // Remove 1 shield after the timer expires
                RemoveShield(1);
            }
        }

    #region Shield UI

        void UpdateAllShieldUI()
        {
            UpdateShieldsAmountText();
            UpdateShieldTimerText();
        }

        void HideTimerBar()
        {
            if(shieldTimer.activeSelf)
            {
                shieldTimer.SetActive(false);
            }
        }
        void ShowTimerBar()
        {
            if(!shieldTimer.activeSelf)
            {
                shieldTimer.SetActive(true);
            }
        }

        void UpdateShieldsAmountText()
        {
            //Shields Number
            shieldAmountText.text = String.Format("x{0}",shieldAmount.ToString());
        }
        void UpdateShieldTimerText()
        {
            //Timer number
            shieldTimerText.text = String.Format("{0:0}", shieldRemainingTime.ToString());

            //Scale the bar depending on timer percentage
            var shieldTimerScale = shieldTimerBar.transform.localScale;
            shieldTimerScale.x = GetTimerPercentage();
        }

        private float GetTimerPercentage()
        {
            return (shieldRemainingTime/shieldTemporaryTime) / 100;
        }
    #endregion

    #region Shield calls
        public void AddShield(int amount)
        {
            bool hadNoShields = shieldAmount < 1;

            shieldAmount += amount;

            if(hadNoShields) { ResetShieldTimer(); }
            UpdateShieldValue();
        }
        public void RemoveShield(int amount)
        {
            //Don't remove if currently don't have any shield
            bool hadNoShields = shieldAmount < 1;
            if(hadNoShields) { return; }

            shieldAmount -= amount;
            ResetShieldTimer();
            UpdateShieldValue();
        }

        private void UpdateShieldValue()
        {
            //Update Health shield variable
            playerHealth.SetShieldValue(shieldAmount);

            //Update shield UI
            UpdateAllShieldUI();
        }

        private void ResetShieldTimer()
        {
            shieldRemainingTime = shieldTemporaryTime;
        }
    #endregion
    }

}
