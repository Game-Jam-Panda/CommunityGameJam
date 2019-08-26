using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Traps
{
    public abstract class TrapConfig : ScriptableObject
    {
        [Header("General Trap Settings")]
        [SerializeField] TrapTypes trapType;
        [SerializeField] int damage;
        [SerializeField] GameObject collisionParticle;

        public TrapTypes GetTrapType() { return trapType; }
        public int GetTrapDamage() { return damage; }
        public GameObject GetCollisionParticle() { return collisionParticle; }

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
        public void SpawnContact(GameObject objectToSpawn, ContactPoint contactPoint)
        {
            behaviour.TriggerCollisionBehaviour(contactPoint);
        }
    }

}
