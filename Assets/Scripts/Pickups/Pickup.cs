using UnityEngine;

namespace CGJ.Pickups
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] protected PickupConfig pickupConfig;
        protected bool triggered = false;

        public bool WasTriggered() { return triggered; }
        public void SetWasTriggered(bool state) { triggered = state; }
    }
}