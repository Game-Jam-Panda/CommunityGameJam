using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGJ.Traps;
using CGJ.System;
using CGJ.Movement;

namespace CGJ.Characters
{
    public class PlayerCollisions : MonoBehaviour
    {
        [SerializeField] bool debug = false;    //TODO Remove variable

        HealthSystem playerHealth = null;
        CharacterMovement playerMovement = null;

        void Awake()
        {
            playerHealth = GetComponent<HealthSystem>();
            playerMovement = GetComponent<CharacterMovement>();
        }

        private void OnTriggerEnter(Collider col)
        {
            // Trap collisions
            if(col.gameObject.GetComponent<Trap>())
            {
                var colTrapComponent = col.GetComponent<Trap>();

                //direction towards the col
                var colliderWithoutZ = new Vector3(colTrapComponent.GetColliderCenterPosition().x, colTrapComponent.GetColliderCenterPosition().y, 0);
                var playerWithoutZ = new Vector3(playerMovement.GetColliderCenterPosition().x, playerMovement.GetColliderCenterPosition().y, 0);
                //Vector3 toCollider = (col.transform.position - playerMovement.GetColliderCenterPosition()).normalized;
                Vector3 toCollider = colliderWithoutZ - playerWithoutZ;

                RaycastHit hit;
                if (Physics.Raycast(playerMovement.GetColliderCenterPosition(), toCollider, out hit))
                {
                    if(debug) { Debug.DrawRay(playerMovement.GetColliderCenterPosition(), toCollider, Color.blue, 1.0f); }

                    Vector3 contactPoint = hit.point;
                    Vector3 contactPointNormal = hit.normal;

                    // Get trap infos
                    TrapConfig trapConfig = col.gameObject.GetComponent<Trap>().GetTrapConfig();
                    TrapTypes trapType = trapConfig.GetTrapType();

                    //***Knockback***// (WIP)
                    //Freeze playermovement for a time && Push the player on the opposite side
                    if(trapConfig.IsKockbackEnabled())
                    { playerMovement.Knockback(trapConfig.GetHitKnockbackTime(), trapConfig.GetKnockbackForceX(), trapConfig.GetKnockbackForceY(), contactPointNormal); }

                    //***Collision effect***//
                    SpawnTrapCollisionParticleByContact(contactPoint, contactPointNormal, trapConfig);

                    //***TRAPS DAMAGE LOGIC***// Depending on the trap, use a damage logic specific for that type of trap
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
                            playerHealth.TakeDamage(trapConfig.GetTrapDamage());
                            break;
                        #endregion
                        #region SPIKES
                        case TrapTypes.Spikes:
                            playerHealth.TakeDamage(trapConfig.GetTrapDamage());
                            break;
                        #endregion
                        default:
                            break;
                    }
            }
        }
        }

        void SpawnTrapCollisionParticleByContact(Vector3 contactPoint, Vector3 contactPointNormal, TrapConfig trapConfig)
        {
            trapConfig.SpawnAtContact(trapConfig.GetCollisionParticle(), contactPoint, contactPointNormal);
        }
    }
}
