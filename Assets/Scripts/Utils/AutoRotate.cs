using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Utils
{
    public class AutoRotate : MonoBehaviour
    {
        [SerializeField] bool enableRotation = true;
        [SerializeField] float xRotationsPerMinute = 1f;
        [SerializeField] float yRotationsPerMinute = 1f;
        [SerializeField] float zRotationsPerMinute = 1f;

        void Update()
        {
            if(!enableRotation) { return; }

            float xDegreesPerFrame = Time.deltaTime / 60 * 360 * xRotationsPerMinute; // X Rotation
            transform.RotateAround(transform.position, transform.right, xDegreesPerFrame);

            float yDegreesPerFrame = Time.deltaTime / 60 * 360 * yRotationsPerMinute; // Y Rotation
            transform.RotateAround(transform.position, transform.up, yDegreesPerFrame);

            float zDegreesPerFrame = Time.deltaTime / 60 * 360 * zRotationsPerMinute; // Z Rotation
            transform.RotateAround(transform.position, transform.forward, zDegreesPerFrame);
        }
    }
}