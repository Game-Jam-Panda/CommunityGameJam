using UnityEngine;

namespace CGJ.Traps
{
    [CreateAssetMenu(menuName = "CGJ/Create Trap/Laser Trap", fileName = "Trap_Laser")]
    public class LaserTrapConfig : TrapConfig
    {
        //[Header("Laser Trap Settings")]

        public override TrapBehaviour AttachAndGetTrapBehaviour(GameObject trapToAttachOn)
        {
            LaserTrapBehaviour trapBehaviour = trapToAttachOn.AddComponent<LaserTrapBehaviour>();
            return trapBehaviour;
        }
    }
}