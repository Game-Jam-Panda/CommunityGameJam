using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Traps
{
    public class VoidTrapBehaviour : TrapBehaviour
    {
        public override void TriggerCollisionBehaviour(Vector3 contactPoint, Vector3 contactNormal)
        {
            Debug.Log("Triggered Void Trap Collision");
            SpawnParticleAtContactPoint(config.GetCollisionParticle(), contactPoint, contactNormal);
        }
    }
}