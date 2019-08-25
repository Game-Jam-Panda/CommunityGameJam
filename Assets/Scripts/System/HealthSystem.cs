using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGJ.System;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour, ISystems
{
    [Header("Health UI")]
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    //TODO Remove exposure
    [SerializeField] int maxHearts = 3;
    [SerializeField] int currentHealth = 1;

    void Update()
    {
        //Make sure the current health doesn't exceed max hearts
        if(currentHealth > maxHearts)
        {
           currentHealth = maxHearts;
        }

        UpdateHealthUI();
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

            // Update Hearts number
            if(i < maxHearts)
            { hearts[i].enabled = true; }
            else
            { hearts[i].enabled = false; }
        }
    }
}
