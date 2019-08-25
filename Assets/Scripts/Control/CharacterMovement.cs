using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Movement
{
    public class CharacterMovement : MonoBehaviour
    {
        [Header("Movement settings")]
        [SerializeField] bool lockVerticalMovement = true;
        [SerializeField] float moveSpeed = 5.0f;
        [SerializeField] float allowMovementTreshhold = 0.0f;

        //Input
        float InputX = 0.0f;
        float InputZ = 0.0f;
        float inputMagnitude;

        //Movement
        Vector3 movementDirection;
        Vector3 movement;

        void Update()
        {
            ProcessInput();
            ProcessMovement();
        }

        void ProcessInput()
        {
            // Left and right movement
            InputX = Input.GetAxis("Horizontal");
            
            // Depth movement
            if(lockVerticalMovement) { InputZ = 0.0f; return; }
            InputZ = Input.GetAxis("Vertical");
        }

        void ProcessMovement()
        {
            // Get input magnitude
            inputMagnitude = new Vector2(InputX, InputZ).sqrMagnitude;
            var inputMagnitudeClamped = Mathf.Clamp01(inputMagnitude);

            // Move if the magnitude has reached the allowed movement treshhold
            if (inputMagnitudeClamped > allowMovementTreshhold)
            {
                Move();
            }
        }

        void Move()
        {
            Vector3 charForward = transform.forward;
            Vector3 charRight = transform.right;

            charForward.y = 0.0f;
            charRight.y = 0.0f;
            charForward.Normalize();
            charRight.Normalize();
            
            //Direction to move
            movementDirection = charForward * InputZ + charRight * InputX;
            movement = movementDirection * moveSpeed * Time.deltaTime;
            
            //Physically move the character
            transform.Translate(movement);
        }
    }

}