using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGJ.Traps;
using CGJ.Movement;
using CGJ.Core;

namespace CGJ.Characters
{
    public class PlayerCollisions : MonoBehaviour
    {
        [SerializeField] bool debug = false;    //TODO Remove variable
        [SerializeField] GameObject playerHitParticle = null;

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

                    // Contact point info
                    Vector3 contactPoint = hit.point;
                    Vector3 contactPointNormal = hit.normal;
                    // Get trap infos
                    TrapConfig trapConfig = col.gameObject.GetComponent<Trap>().GetTrapConfig();
                    TrapTypes trapType = trapConfig.GetTrapType();

                    //*** Damage ***//
                    // Instant kill or trap damage
                    if(trapConfig.IsInstantKill())
                    { playerHealth.TakeDamage(playerHealth.GetCurrentHealth()); }
                    else
                    { playerHealth.TakeDamage(trapConfig.GetTrapDamage()); }

                    //*** Knockback ***//
                    //Freeze player for a time && Push him on the opposite side
                    if(trapConfig.IsKockbackEnabled())
                    { playerMovement.Knockback(trapConfig.GetHitKnockbackTime(), trapConfig.GetKnockbackForceX(), trapConfig.GetKnockbackForceY(), contactPointNormal); }

                    //*** Collision effect ***//
                    SpawnTrapCollisionParticleByContact(contactPoint, contactPointNormal, trapConfig);
                    SpawnPlayerHitParticle(trapConfig);

                    //***TRAPS SPECIFIC LOGIC***// Call trap-specific code
                    switch (trapType)
                    {
                        #region VOID
                        case TrapTypes.Void:
                            
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

                        #region LASER
                        case TrapTypes.Laser:

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

        void SpawnPlayerHitParticle(TrapConfig trapConfig)
        {
            var hitParticle = trapConfig.GetTrapHitEffect();
            if(hitParticle == null) { return; }

            Instantiate(hitParticle, transform.position, Quaternion.identity);
        }
    }
}
