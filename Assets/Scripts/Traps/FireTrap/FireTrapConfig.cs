using UnityEngine;

namespace CGJ.Traps
{
    [CreateAssetMenu(menuName = "CGJ/Create Trap/Fire Trap", fileName = "Fire Trap")]
    public class FireTrapConfig : TrapConfig
    {
        [Header("Fire Trap Settings")]
        [Tooltip("The time the fire will last for.")]
        [SerializeField] float fireDuration = 0.5f;
        [SerializeField] int fireNumberOfHits = 2;

        public float GetFireDuration() { return fireDuration; }
        public int GetFireHitsNumber() { return fireNumberOfHits; }

        public override TrapBehaviour AttachAndGetTrapBehaviour(GameObject trap)
        {
            FireTrapBehaviour trapBehaviour = trap.AddComponent<FireTrapBehaviour>();
            return trapBehaviour;
        }
    }
}