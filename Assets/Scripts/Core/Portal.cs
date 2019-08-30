using System.Collections;
using System.Collections.Generic;
using CGJ.Events;
using CGJ.System;
using UnityEngine;

namespace CGJ.Core
{
    public class Portal : Trigger
    {
        [SerializeField] int destinationSceneIndex = 3;

        private void OnTriggerEnter(Collider col)
        {
            if (col.tag != "Player") { return; }

            // Load destination scene on trigger
            SystemManager.systems.sceneLoadingSystem.LoadSceneByIndex(destinationSceneIndex);
        }
    }

}