using System;
using CGJ.Characters;
using CGJ.Core;
using UnityEngine;

namespace CGJ.System
{
    public class CGJEventsSystem: MonoBehaviour
    {
        [SerializeField] GameObject checkpointNotificationPosition;

        //Game
        public void QuitGame()
        {
            //SystemManager.systems.soundManager.PlaySound();
            
            
            Application.Quit();
        }

        //Enemies
        public void ConsumeDarkEnemy(DarkEnemy darkEnemy)
        {
            darkEnemy.Consume();
        }
        public void ConsumeAllDarkEnemies()
        {
            var darkEnemies = GameObject.FindObjectsOfType<DarkEnemy>();
            foreach(DarkEnemy darkEnemy in darkEnemies)
            {
                darkEnemy.Consume();
            }
        }
    }
}