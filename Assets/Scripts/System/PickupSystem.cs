using System;
using System.Collections;
using System.Collections.Generic;
using CGJ.Mechanics;
using CGJ.Pickups;
using UnityEngine;

namespace CGJ.System
{
    public class PickupSystem: MonoBehaviour
    {
        public void Pickup(GameObject instigator, PickupType pickupType, AudioClip pickupSound)
        {
            // Pickup a shield
            switch (pickupType)
            {
                case PickupType.Shield:
                    instigator.GetComponent<ShieldMechanic>().AddShield(1);
                    SystemManager.systems.soundManager.PlaySound(pickupSound);
                    break;

                default:
                    break;
            }
        }
    }
}