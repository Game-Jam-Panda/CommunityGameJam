using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Traps
{
    public abstract class TrapConfig : ScriptableObject
    {
        [Header("General - Trap settings")]
        [SerializeField] TrapTypes trapType;
        [Header("General - Collision settings")]
        [SerializeField] int damage;
        [SerializeField] GameObject collisionParticle;
        [Header("General - Knockback settings")]
        [SerializeField] bool knockbackEnabled = false;
        [SerializeField] float hitKnockbackTime = 0.3f;
        [SerializeField] float knockbackForceX = 2.0f;
        [SerializeField] float knockbackForceY = 2.0f;

        //Trap settings
        public TrapTypes GetTrapType() { return trapType; }
        //Collision
        public int GetTrapDamage() { return damage; }
        public GameObject GetCollisionParticle() { return collisionParticle; }
        //Knockback
        public bool IsKockbackEnabled() { return knockbackEnabled; }
        public float GetHitKnockbackTime() { return hitKnockbackTime; }
        public float GetKnockbackForceX() { return knockbackForceX; }
        public float GetKnockbackForceY() { return knockbackForceY; }

        // Get Behaviour
        protected TrapBehaviour behaviour;
        public abstract TrapBehaviour AttachAndGetTrapBehaviour(GameObject trapToAttachOn);

        // Attach the trap behaviour to the trap object & Reference it as the behaviour of this config
        public void AttachTrapBehaviourOn(GameObject trapToAttachOn)
        {
            TrapBehaviour behaviourComponent = AttachAndGetTrapBehaviour(trapToAttachOn);
            behaviourComponent.SetConfig(this);     // Set reference of Config in Behaviour
            behaviour = behaviourComponent;         // Set reference of Behaviour in Config
        }


        //----Trigger calls----
        public void TriggerTrap()
        {}

        //For Spawning at the point of collision
        public void SpawnAtContact(GameObject objectToSpawn, Vector3 contactPoint, Vector3 contactNormal)
        {
            behaviour.TriggerCollisionBehaviour(contactPoint, contactNormal);
        }
    }

}
