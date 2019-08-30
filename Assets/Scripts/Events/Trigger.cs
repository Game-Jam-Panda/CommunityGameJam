using CGJ.System;
using UnityEngine;

namespace CGJ.Events
{
    [RequireComponent(typeof(BoxCollider))]
    public class Trigger : MonoBehaviour
    {
        [Header("Trigger settings")]
        [SerializeField] protected bool oneTimeTrigger = true;

        protected BoxCollider triggerCollider = null;
        void Awake()
        {
            triggerCollider = GetComponent<BoxCollider>();
            triggerCollider.enabled = true;
        }
    }
}