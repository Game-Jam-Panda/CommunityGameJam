using System.Collections;
using System.Collections.Generic;
using CGJ.Utils;
using UnityEngine;

namespace CGJ.Traps
{
    public class Trap : MonoBehaviour
    {
        [Header("Trap - General settings")]
        [SerializeField] protected TrapConfig trapConfig;
        [SerializeField] protected Transform colliderCenter = null;

        public TrapConfig GetTrapConfig() { return trapConfig; }
        public Vector3 GetColliderCenterPosition()
        {
            if(colliderCenter) 
            { return colliderCenter.position; }
            else
            { return transform.position; }
        }

        void Start()
        {
            AttachBehaviourOnTrap();
        }

        protected void AttachBehaviourOnTrap()
        {
            trapConfig.AttachTrapBehaviourOn(gameObject);
        }
    }
}