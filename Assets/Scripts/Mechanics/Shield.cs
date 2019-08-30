using System;
using System.Collections;
using System.Collections.Generic;
using CGJ.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CGJ.Mechanics
{
    public class Shield : MonoBehaviour
    {
        [Header("Shield UI")]
        [SerializeField] Image shieldTimerBar = null;
        [SerializeField] Text shieldTimerText = null;
        [SerializeField] Text shieldAmountText = null;

        [Header("Shield Timer")]
        [SerializeField] float shieldTemporaryTime = 5.0f;
        float shieldRemainingTime = 0.0f;

        //Shield values
        int shieldAmount = 0;

        HealthSystem playerHealth;

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
            if(Input.GetKeyDown(KeyCode.F)){ AddShield(1); }   //Simulate shield pickup //TODO REMOVE

            // Reduce shield timer timer as long you you have a shield
            if(shieldAmount > 0 && shieldRemainingTime > 0)
            {
                shieldRemainingTime -= Time.deltaTime;
                UpdateUITimerText();
                return;
            }

            // Remove 1 shield after the timer expires
            RemoveShield(1);
        }

    #region Shield UI

        void UpdateAllShieldUI()
        {
            UpdateUIText();
            UpdateUITimerText();
        }

        void UpdateUIText()
        {
            //Shields Number
            shieldAmountText.text = shieldAmount.ToString();
        }
        void UpdateUITimerText()
        {
            shieldTimerText.text = String.Format("{0:1}", shieldRemainingTime.ToString());
            UpdateUIBar();
        }
        void UpdateUIBar()
        {
            shieldTimerBar.fillAmount = GetTimerPercentage();
        }

        private float GetTimerPercentage()
        {
            return  (shieldRemainingTime/shieldTemporaryTime) / 100;
        }
    #endregion

    #region Shield calls
        public void AddShield(int amount)
        {
            shieldAmount += amount;
            UpdateShieldValue();
        }
        public void RemoveShield(int amount)
        {
            shieldAmount -= amount;
            UpdateShieldValue();
        }

        private void UpdateShieldValue()
        {
            //Update Health shield variable
            playerHealth.SetShieldValue(shieldAmount);

            //Reset Shield timer
            shieldRemainingTime = shieldTemporaryTime;
            
            //Update shield UI
            UpdateAllShieldUI();
        }
    #endregion
    }

}
