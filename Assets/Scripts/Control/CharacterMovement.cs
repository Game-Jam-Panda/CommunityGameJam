using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Movement
{
    public class CharacterMovement : MonoBehaviour
    {
        [Header("Movement settings")]
        [SerializeField] bool lockVerticalMovement = true;
        [SerializeField] float moveSpeed = 6.0f;
        [SerializeField] float allowMovementTreshhold = 0.0f;

        [Header("Jump settings")]
        [SerializeField] LayerMask groundedLayers;
        [SerializeField] float jumpForce = 5.0f;
        [SerializeField] float groundedRadiusMultiplier = 0.2f;
        bool grounded = true;

        //Input
        float InputX = 0.0f;
        float InputZ = 0.0f;
        float inputMagnitude;

        //Movement
        Vector3 movementDirection;
        Vector3 movement;

        //References
        Rigidbody rb;
        CapsuleCollider col;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<CapsuleCollider>();
        }

        void Update()
        {
            ProcessMovementInput();
            ProcessJump();
        }
        void FixedUpdate()
        {
            ProcessMovement();
        }

#region Movement
        void ProcessMovementInput()
        {
            // Left and right movement
            InputX = Input.GetAxis("Horizontal");
            
            // Depth movement
            if(lockVerticalMovement)
            {
                InputZ = 0.0f;
                FreezeRigidbodyZMovement();
            }
            else
            {
                InputZ = Input.GetAxis("Vertical");
                AllowRigidbodyZMovement();
            }
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
        
        private void Move()
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

        private void FreezeRigidbodyZMovement()
        {
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
        private void AllowRigidbodyZMovement()
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
#endregion

#region Jump
        void ProcessJump()
        {
            grounded = IsGrounded();

            if(grounded && Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        private bool IsGrounded()
        {
            //TODO Remove magic 0.9f number
            return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.center.y, col.bounds.center.z), col.radius * groundedRadiusMultiplier, groundedLayers);
        }
#endregion
    }
}