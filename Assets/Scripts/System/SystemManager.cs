using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CGJ.System
{
    public class SystemManager : MonoBehaviour
    {
        public static SystemManager instance { get; protected set; }
        public static ISystems systems { get; protected set; }
    }
}