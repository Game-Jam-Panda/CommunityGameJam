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
        [SerializeField] float jumpForce = 5.0f;
        [SerializeField] GameObject jumpDustParticle = null;
        [SerializeField] LayerMask groundedLayers;
        [SerializeField] Transform groundedCheckSource = null;
        [SerializeField] float groundedCheckDistance = 0.1f;
        [SerializeField] bool grounded = true;  //TODO Remove exposure
        
        //Input
        float InputX = 0.0f;
        float InputZ = 0.0f;
        float inputMagnitude;

        //Movement
        Vector3 movementDirection;
        Vector3 movement;
        bool zMovementFrozen = true;

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
                FreezeRigidbodyZMovement();
                InputZ = 0.0f;
            }
            else
            {
                AllowRigidbodyZMovement();
                InputZ = Input.GetAxis("Vertical");
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
            movement = movementDirection * moveSpeed;
            
            //Physically move the character
            transform.Translate(movement * Time.deltaTime);
        }

        private void FreezeRigidbodyZMovement()
        {
            if(!zMovementFrozen) { return; }
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            zMovementFrozen = true;
        }
        private void AllowRigidbodyZMovement()
        {
            if(zMovementFrozen) { return; }
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            zMovementFrozen = false;
        }
#endregion

#region Jump
        void ProcessJump()
        {
            // Grounded check
            grounded = Physics.Raycast(groundedCheckSource.position, Vector3.down, groundedCheckDistance, groundedLayers);

            if(grounded && Input.GetKeyDown(KeyCode.Space))
            {
                // Prevent from jumping multiple times
                if(rb.velocity.y > 0) { return; }
                
                Jump();
            }
        }

        void Jump()
        {
            // Spawn jump dust particle
            if (jumpDustParticle != null) { Instantiate(jumpDustParticle, transform.position, Quaternion.identity); }

            // Physically make the player jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            grounded = false;
        }
#endregion

#region GIZMOS

        // Debug Gizmos to see grounded distance
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundedCheckSource.position,
                new Vector3(groundedCheckSource.position.x, groundedCheckSource.position.y - groundedCheckDistance, groundedCheckSource.position.z)
            );
        }
#endregion
    }
}