using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Cameras
{
    public class CameraLookAt : MonoBehaviour
    {
        [SerializeField] GameObject target = null;

        void LateUpdate()
        {
            if(target == null) { return; }

            transform.LookAt(target.transform);
        }
    }
}
