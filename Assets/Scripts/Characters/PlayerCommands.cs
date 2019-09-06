using System.Collections;
using System.Collections.Generic;
using CGJ.Core;
using UnityEngine;

namespace CGJ.Characters
{
    public class PlayerCommands : MonoBehaviour
    {
        HealthSystem playerHealth = null;
        
        void Awake()
        {
            playerHealth = GetComponent<HealthSystem>();
        }

        void Update()
        {
            ProcessFakeCommands();
        }

        void ProcessFakeCommands()
        {
            // Instant kill for trying to heal ourselves
            if(Input.GetKeyDown(KeyCode.H))
            {
                playerHealth.TakeDamage(playerHealth.GetCurrentHealth());
            }
        }
    }
}
