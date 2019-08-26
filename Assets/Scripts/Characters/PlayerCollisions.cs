using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGJ.Traps;
using CGJ.System;

namespace CGJ.Characters
{
    public class PlayerCollisions : MonoBehaviour
    {
        HealthSystem playerHealth = null;

        void Awake()
        {
            playerHealth = GetComponent<HealthSystem>();
        }

        private void OnCollisionEnter(Collision col)
        {
            // Trap collisions
            if(col.gameObject.GetComponent<Trap>())
            {
                TrapConfig trapConfig = col.gameObject.GetComponent<Trap>().GetTrapConfig();
                TrapTypes trapType = trapConfig.GetTrapType();
                Debug.Log("Collided with trap of type: " + trapType.ToString());

                //-Collision effect-//
                SpawnTrapCollisionParticleByContact(col, trapConfig);

                //--TRAPS DAMAGE LOGIC--// Depending on the trap, use a damage logic specific for that type of trap
                switch (trapType)
                {
                    #region VOID
                    case TrapTypes.Void:

                        // Instant kill or one-time damage
                        if((trapConfig as VoidTrapConfig).IsInstantKill())
                        {
                            playerHealth.TakeDamage(playerHealth.GetCurrentHealth());
                        }
                        else
                        {
                            playerHealth.TakeDamage(trapConfig.GetTrapDamage());
                        }
                        
                        break;
                    #endregion

                    #region FIRE
                    case TrapTypes.Fire:

                        break;
                    #endregion

                    #region SPIKES
                    case TrapTypes.Spikes:

                        break;
                    #endregion
                    default:
                        break;
                }
            }
        }

        void SpawnTrapCollisionParticleByContact(Collision col, TrapConfig trapConfig)
        {
            ContactPoint contact = col.contacts[0];
            
            trapConfig.SpawnContact(trapConfig.GetCollisionParticle(), contact);
        }
    }
}
