using System.Collections;
using System.Collections.Generic;
using CGJ.Characters;
using CGJ.System;
using UnityEngine;

namespace CGJ.Events
{
    public class ConsumeEnemyTrigger : Trigger
    {
        [Header("Consume settings")]
        [SerializeField] bool consumeAllEnemies = false;
        [SerializeField] List<DarkEnemy> enemiesToConsume;

        private void OnTriggerEnter(Collider col)
        {
            if(col.tag != "Player") { return; }

            if(consumeAllEnemies)
            {
                SystemManager.systems.eventsSystem.ConsumeAllDarkEnemies();
                return;
            }
            else
            {
                SystemManager.systems.eventsSystem.ConsumeDarkEnemies(enemiesToConsume);
            }
            
        }
    }
}