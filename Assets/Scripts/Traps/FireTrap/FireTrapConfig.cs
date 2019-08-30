using UnityEngine;

namespace CGJ.Traps
{
    [CreateAssetMenu(menuName = "CGJ/Create Trap/Fire Trap", fileName = "Trap_Fire")]
    public class FireTrapConfig : TrapConfig
    {
        //[Header("Fire Trap Settings")]
        
        public override TrapBehaviour AttachAndGetTrapBehaviour(GameObject trap)
        {
            var trapBehaviour = trap.AddComponent<FireTrapBehaviour>();
            return trapBehaviour;
        }
    }
}