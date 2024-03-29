﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Cameras
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] GameObject target = null;

        [Header("Camera Movement")]
        [SerializeField] float followSpeed = 10.0f;

        [Header("Camera Positioning")]
        [SerializeField] float ZOffset = 10.0f;
        [SerializeField] float heightOffset = 2.0f;

        float camPosX = 0.0f;
        float camPosY = 0.0f;
        float camPosZ = 0.0f;

        void Start()
        {
            if(target == null) { return; }
            
            // TP the camera to the starting position
            Vector3 startPosition = new Vector3(target.transform.position.x, target.transform.position.y + heightOffset, -ZOffset);
            transform.position = startPosition;
        }

        void FixedUpdate()
        {
            if(target == null) { return; }

            FollowTarget();
        }

        void FollowTarget()
        {
            Vector3 currentPosition = transform.position;
            Vector3 targetPos = target.transform.position;

            camPosX = targetPos.x;
            camPosY = targetPos.y + heightOffset;      // Camera height relative to the target
            camPosZ = -ZOffset;                   // Side-view offset

            Vector3 newPosition = new Vector3(camPosX, camPosY, camPosZ);

            // Move the camera towards the target
            transform.position = Vector3.Lerp(currentPosition, newPosition, followSpeed * Time.deltaTime);
        }
    }
}