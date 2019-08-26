using System.Collections;
using System.Collections.Generic;
using CGJ.Utils;
using UnityEngine;

namespace CGJ.Traps
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] TrapConfig trapConfig;

        public TrapConfig GetTrapConfig() { return trapConfig; }

        void Start()
        {
            trapConfig.AttachTrapBehaviourOn(gameObject);
        }
    }
}