using UnityEngine;

namespace CGJ.Traps
{
    [CreateAssetMenu(menuName = "CGJ/Create Trap/Void Trap", fileName = "Trap_Void")]
    public class VoidTrapConfig : TrapConfig
    {
        [Header("Void Trap Settings")]
        [SerializeField] bool instantKill = true;
        
        //Trap settings
        public bool IsInstantKill() { return instantKill; }

        public override TrapBehaviour AttachAndGetTrapBehaviour(GameObject trapToAttachOn)
        {
            var trapBehaviour = trapToAttachOn.AddComponent<VoidTrapBehaviour>();
            return trapBehaviour;
        }
    }
}