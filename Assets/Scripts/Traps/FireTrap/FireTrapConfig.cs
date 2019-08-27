using UnityEngine;

namespace CGJ.Traps
{
    [CreateAssetMenu(menuName = "CGJ/Create Trap/Fire Trap", fileName = "Trap_Fire")]
    public class FireTrapConfig : TrapConfig
    {
        [Header("Fire Trap Settings")]
        [Tooltip("The time the fire will last for.")]
        [SerializeField] float fireDuration = 2.0f;

        public float GetFireDuration() { return fireDuration; }

        public override TrapBehaviour AttachAndGetTrapBehaviour(GameObject trap)
        {
            var trapBehaviour = trap.AddComponent<FireTrapBehaviour>();
            return trapBehaviour;
        }
    }
}