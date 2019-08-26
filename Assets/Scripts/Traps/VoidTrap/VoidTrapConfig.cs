using CGJ.Utils;
using UnityEngine;

namespace CGJ.Traps
{
    [CreateAssetMenu(menuName = "CGJ/Create Trap/Void Trap", fileName = "Void Trap")]
    public class VoidTrapConfig : TrapConfig
    {
        [Header("Void Trap Settings")]
        [SerializeField] bool instantKill = true;
        
        //Trap settings
        public bool IsInstantKill() { return instantKill; }

        public override TrapBehaviour AttachAndGetTrapBehaviour(GameObject trapToAttachOn)
        {
            VoidTrapBehaviour trapBehaviour = trapToAttachOn.AddComponent<VoidTrapBehaviour>();
            return trapBehaviour;
        }
    }
}