using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Traps
{
    public abstract class TrapBehaviour : MonoBehaviour
    {
        // Set Config
        protected TrapConfig config;
        public void SetConfig(TrapConfig trapConfig) { config = trapConfig; }

        public abstract void TriggerCollisionBehaviour(Vector3 contactPoint, Vector3 contactNormal);

        protected void SpawnParticleAtContactPoint(GameObject particlePrefab, Vector3 contactPoint, Vector3 contactNormal)
        {
            Vector3 pos = contactPoint;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contactNormal);
            Instantiate(particlePrefab, pos, rot);
        }
    }
}
