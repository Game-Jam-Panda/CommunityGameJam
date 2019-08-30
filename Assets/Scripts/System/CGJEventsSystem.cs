using System;
using System.Collections.Generic;
using CGJ.Characters;
using CGJ.Core;
using UnityEngine;

namespace CGJ.System
{
    public class CGJEventsSystem: MonoBehaviour
    {
        [SerializeField] GameObject checkpointNotificationPosition;

        //Enemies
        public void ConsumeDarkEnemy(DarkEnemy darkEnemy)
        {
            darkEnemy.Consume();
        }
        public void ConsumeDarkEnemies(List<DarkEnemy> darkEnemies)
        {
            foreach(DarkEnemy darkEnemy in darkEnemies)
            {
                darkEnemy.Consume();
            }
        }
        public void ConsumeAllDarkEnemies()
        {
            var darkEnemies = GameObject.FindObjectsOfType<DarkEnemy>();
            
            foreach(DarkEnemy darkEnemy in darkEnemies)
            {
                darkEnemy.ConsumeWithoutSound();
            }
        }
    }
}