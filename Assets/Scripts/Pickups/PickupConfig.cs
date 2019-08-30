using UnityEngine;

namespace CGJ.Pickups
{
    [CreateAssetMenu(menuName = "CGJ/Pickups/Create Pickup", fileName = "Pickup")]
    public class PickupConfig : ScriptableObject
    {
        [SerializeField] PickupType pickupType;
        [SerializeField] AudioClip pickupSound;

        public PickupType GetPickupType() { return pickupType; }
        public AudioClip GetPickupSound() { return pickupSound; }
    }
}