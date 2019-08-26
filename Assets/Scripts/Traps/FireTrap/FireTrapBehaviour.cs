using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Traps
{
    public class FireTrapBehaviour : TrapBehaviour
    {
        public override void ProcessTrapBehaviour()
        {
            Debug.Log("Processing Fire Trap Behaviour");
        }

        public override void TriggerCollisionBehaviour(ContactPoint contact)
        {
            Debug.Log("Triggered Fire Trap Collision");
            SpawnParticleAtContactPoint(config.GetCollisionParticle(), contact);
        }
    }
}