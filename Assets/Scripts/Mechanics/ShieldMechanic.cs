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

        [Header("Shield Visual")]
        [SerializeField] GameObject shieldModel = null;

        //Shield values
        int shieldsAmount = 0;

        HealthSystem playerHealth;

        public int GetCurrentShieldsAmount() { return shieldsAmount; }

        void Awake()
        {
            playerHealth = GetComponent<HealthSystem>();
        }
        
        void Start()
        {
            if(shieldModel.activeSelf) { shieldModel.SetActive(false); }    //Safety-check to disable the model at the beginning

            UpdateAllShieldUI();
        }

        void Update()
        {
            HandleBarVisibility();
            ProcessShieldFunctionnality();

            if (Input.GetKeyDown(KeyCode.F)) { AddShield(1); }   //Simulate shield pickup //TODO REMOVE
        }

        void ProcessShieldFunctionnality()
        {
            // Reduce shield timer as long you you have a shield
            if (shieldsAmount > 0 && shieldRemainingTime > 0)
            {
                shieldRemainingTime -= Time.deltaTime;
                UpdateShieldTimerText();
                return;
            }

            if (shieldRemainingTime <= 0)
            {
                shieldRemainingTime = 0.0f;

                // Remove 1 shield after the timer expires
                RemoveShield(1);
            }
        }

        #region Shield UI

        void HandleBarVisibility()
        {
            if(shieldsAmount > 0)
            {
                ShowTimerBar();
                ShowShieldModel();
            }
            else
            {
                HideTimerBar();
                HideShieldModel();
            }
        }

        void UpdateAllShieldUI()
        {
            UpdateShieldsAmountText();
            UpdateShieldTimerText();
        }

        // Timer bar visiblity
        void HideTimerBar() { if(shieldTimer.activeSelf) { shieldTimer.SetActive(false); } }
        void HideShieldModel() { if(shieldModel.activeSelf) { shieldModel.SetActive(false); } }
        // Model visibility
        void ShowTimerBar() { if(!shieldTimer.activeSelf) { shieldTimer.SetActive(true); } }
        void ShowShieldModel() { if(!shieldModel.activeSelf) { shieldModel.SetActive(true); } }

        void UpdateShieldsAmountText()
        {
            //Shields Number
            shieldAmountText.text = String.Format("x{0}",shieldsAmount.ToString());
        }
        void UpdateShieldTimerText()
        {
            //Timer number
            shieldTimerText.text = String.Format("{0:1}", shieldRemainingTime.ToString());

            //TODO Fix
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
            bool hadNoShields = shieldsAmount < 1;

            shieldsAmount += amount;

            if(hadNoShields) { ResetShieldTimer(); }
            UpdateShieldValue();
        }
        public void RemoveShield(int amount)
        {
            //Don't remove if currently don't have any shield
            bool hadNoShields = shieldsAmount < 1;
            if(hadNoShields) { return; }

            shieldsAmount -= amount;
            ResetShieldTimer();
            UpdateShieldValue();
        }

        private void UpdateShieldValue()
        {
            //Update Health shield variable
            playerHealth.SetShieldValue(shieldsAmount);

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
