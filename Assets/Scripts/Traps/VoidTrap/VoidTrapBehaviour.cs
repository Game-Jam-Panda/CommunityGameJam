using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Traps
{
    public class VoidTrapBehaviour : TrapBehaviour
    {
        public override void ProcessTrapBehaviour()
        {
            Debug.Log("Processing Void Trap Behaviour");
        }

        public override void TriggerCollisionBehaviour(ContactPoint contact)
        {
            Debug.Log("Triggered Void Trap Collision");
            SpawnParticleAtContactPoint(config.GetCollisionParticle(), contact);
        }
    }
}