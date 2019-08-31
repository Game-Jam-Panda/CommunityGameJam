using System;
using System.Collections.Generic;
using CGJ.Characters;
using CGJ.Core;
using UnityEngine;

namespace CGJ.System
{
    public class CGJEventsSystem: MonoBehaviour
    {
        //Enemies
        public void ConsumeDarkEnemy(DarkEnemy darkEnemy)
        {
            darkEnemy.StartCoroutine(darkEnemy.Consume());
        }
        public void ConsumeDarkEnemies(List<DarkEnemy> darkEnemies)
        {
            foreach(DarkEnemy darkEnemy in darkEnemies)
            {
                darkEnemy.StartCoroutine(darkEnemy.Consume());
            }
        }
        public void ConsumeAllDarkEnemies()
        {
            var darkEnemies = GameObject.FindObjectsOfType<DarkEnemy>();
            
            foreach(DarkEnemy darkEnemy in darkEnemies)
            {
                darkEnemy.StartCoroutine(darkEnemy.ConsumeWithoutSound());
            }
        }
    }
}