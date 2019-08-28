using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Traps
{
    public class LaserTrapBehaviour : TrapBehaviour
    {
        public override void ProcessTrapBehaviour()
        {}

        public override void TriggerCollisionBehaviour(Vector3 contactPoint, Vector3 contactNormal)
        {
            Debug.Log("Triggered Laser Trap Collision");
            SpawnParticleAtContactPoint(config.GetCollisionParticle(), contactPoint, contactNormal);
        }
    }
}