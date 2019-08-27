using UnityEngine;

namespace CGJ.Traps
{
    [CreateAssetMenu(menuName = "CGJ/Create Trap/Spike Trap", fileName = "Trap_Spike")]
    public class SpikeTrapConfig : TrapConfig
    {
        //[Header("Spike Trap Settings")]

        public override TrapBehaviour AttachAndGetTrapBehaviour(GameObject trapToAttachOn)
        {
            SpikeTrapBehaviour trapBehaviour = trapToAttachOn.AddComponent<SpikeTrapBehaviour>();
            return trapBehaviour;
        }
    }
}