using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Utils
{
    public class AutoDestroy : MonoBehaviour
    {
        [SerializeField] float autoDestroyTimer = 5.0f;

        void Start()
        {
            StartCoroutine(DestroyAfterTime(autoDestroyTimer));
        }

        IEnumerator DestroyAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}

