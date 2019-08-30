using System.Collections;
using System.Collections.Generic;
using CGJ.Mechanics;
using CGJ.System;
using UnityEngine;

namespace CGJ.Pickups
{
    public class TemporaryShieldPickup : Pickup
    {
        void OnTriggerEnter(Collider col)
        {
            if(col.tag == "Player" && col.gameObject.GetComponent<ShieldMechanic>())
            {
                SystemManager.systems.pickupSystem.Pickup(col.gameObject, pickupConfig.GetPickupType(), pickupConfig.GetPickupSound());
                Destroy(gameObject);
            }
        }
    }
}