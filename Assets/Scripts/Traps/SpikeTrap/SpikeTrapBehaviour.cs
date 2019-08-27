using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Traps
{
    public class SpikeTrapBehaviour : TrapBehaviour
    {
        public override void ProcessTrapBehaviour()
        {}

        public override void TriggerCollisionBehaviour(Vector3 contactPoint, Vector3 contactNormal)
        {
            Debug.Log("Triggered Spike Trap Collision");
            SpawnParticleAtContactPoint(config.GetCollisionParticle(), contactPoint, contactNormal);
        }
    }
}