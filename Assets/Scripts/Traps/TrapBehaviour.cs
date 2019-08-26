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

        // Process Trap Behaviour
        private void Update()
        {
           ProcessTrapBehaviour();
        }
        public abstract void ProcessTrapBehaviour();
        public abstract void TriggerCollisionBehaviour(ContactPoint contact);

        protected void SpawnParticleAtContactPoint(GameObject particlePrefab, ContactPoint contact)
        {
            Vector3 pos = contact.point;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Instantiate(particlePrefab, pos, rot);
        }
    }
}
