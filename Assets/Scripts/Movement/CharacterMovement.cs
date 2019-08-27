using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Movement
{
    public class CharacterMovement : MonoBehaviour
    {
        [Header("Movement settings")]
        [SerializeField] bool lockVerticalMovement = true;  //TODO Remove exposure
        [SerializeField] bool controlsBlocked = false;      //TODO Remove exposure
        [SerializeField] bool blockMovement = false;
        [SerializeField] float moveSpeed = 6.0f;
        [SerializeField] float allowMovementTreshhold = 0.0f;
        [Header("Rotation settings")]
        [SerializeField] bool blockRotation = false;    //TODO Remove exposure
        [SerializeField] float rotationSpeed = 10.0f;

        [Header("Knockback settings")]
        [SerializeField] float yKnockbackForce = 0.0f;

        [Header("Jump settings")]
        [SerializeField] float jumpForce = 6.0f;
        [SerializeField] GameObject jumpDustParticle = null;
        [SerializeField] AudioClip[] jumpSounds;
        [SerializeField] bool isFalling = false;
        [SerializeField] float gravityMultiplier = 3.0f;
        [SerializeField] float fallingSpeedLimit = 8f;
        [SerializeField] float fallingVelocityTreshhold = 8.61f;

        [Header("Grounded settings")]
        [SerializeField] LayerMask groundedLayers;
        [SerializeField] Transform groundedCheckSource = null;
        [SerializeField] float groundedCheckDistance = 0.1f;
        [SerializeField] bool grounded = true;  //TODO Remove exposure
        
        //Input
        float InputX = 0.0f;
        float InputZ = 0.0f;
        float inputMagnitude;
        float inputMagnitudeClamped;

        //Movement
        Vector3 movementDirection;
        Vector3 movement;
        bool zMovementFrozen = true;

        //Rotation
        Vector3 camLookAtRotation;

        //References
        Rigidbody rb;
        CapsuleCollider col;
        Camera cam;

        public Rigidbody GetRigidbody() { return rb; }

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<CapsuleCollider>();
            cam = Camera.main;
        }

        void Update()
        {
            ProcessMovementInput();

            if(controlsBlocked) {return; }

            ProcessJump();
        }
        void FixedUpdate()
        {
            if(isFalling) { ProcessFalling(); }

            if(controlsBlocked) { return; }

            ProcessMovement();
        }
        
#region Knockback
        // Knockback
        public void Knockback(float freezeTime, float knockbackForceX, float knockbackForceY, Vector3 contactPointNormal)
        {
            //Freeze movement
            StartCoroutine(FreezeMovementControlsForTime(freezeTime));

            //Knockback direction
            var knockbackDirection = (contactPointNormal - transform.forward);  //Based on character forward direction
            var knockbackDirectionNormal = (contactPointNormal - Vector3.forward);
            knockbackDirection.z = 0.0f;
            knockbackDirection.Normalize(); //Make the knockback more predictable
            //Knockback movement
            var knockback = new Vector3 (knockbackDirection.x * knockbackForceX,
                                        knockbackForceY,
                                        0.0f);                                      //Do not consider the z value of the knockback

            //Physically knockback character
            rb.AddForce(knockback, ForceMode.Impulse);
        }
#endregion

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
            inputMagnitudeClamped = Mathf.Clamp01(inputMagnitude);

            // Move if the magnitude has reached the allowed movement treshhold
            if (inputMagnitudeClamped > allowMovementTreshhold)
            {
                RotateAndMove();
            }
        }
        
        private void RotateAndMove()
        {
            var worldForward = Vector3.forward;
            var worldRight = Vector3.right;
            worldForward.y = 0.0f;
            worldRight.y = 0.0f;
            worldForward.Normalize();
            worldRight.Normalize();

            //Direction to move
            movementDirection = worldForward * InputZ + worldRight * InputX;
            //Forward Movement vector (always facing forward with the camera taking care of rotating)
            movement = worldForward * inputMagnitudeClamped * moveSpeed;

            //Physically Rotate the character
            if(blockRotation == false)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDirection), rotationSpeed * Time.deltaTime);
            }
            //Physically Move the character
            if(blockMovement == false)
            {
                transform.Translate(movement * Time.deltaTime);
            }
        }

        private void FreezeRigidbodyZMovement()
        {
            if(!zMovementFrozen)
            {
                zMovementFrozen = true;
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }
        private void AllowRigidbodyZMovement()
        {
            if(zMovementFrozen)
            {
                zMovementFrozen = false;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }

        IEnumerator FreezeMovementControlsForTime(float time)
        {
            //Freeze
            rb.velocity.Set(0.0f, 0.0f, 0.0f);
            //Block physical movement
            controlsBlocked = true;

            //Reset after freeze time
            yield return new WaitForSeconds(time);
            controlsBlocked = false;
        }
#endregion

#region Jump mechanic
        void ProcessJump()
        {
            // Grounded check
            grounded = Physics.Raycast(groundedCheckSource.position, Vector3.down, groundedCheckDistance, groundedLayers);

            //Jump mechanic
            if(grounded && Input.GetKeyDown(KeyCode.Space))
            {
                // Prevent from jumping multiple times
                if(rb.velocity.y > 0) { return; }
                
                // Initiate jump
                Jump();
            }

            //Gravity falling effect while in the air
            if(!grounded && !Mathf.Approximately(rb.velocity.y, 0) && rb.velocity.y < fallingVelocityTreshhold)
            {
                isFalling = true;
            }
            else
            {
                isFalling = false;
            }
        }

        void ProcessFalling()
        {
            if(rb.velocity.y < -fallingSpeedLimit) { return; }
            rb.AddForce(Vector3.down * gravityMultiplier * 100 * Time.deltaTime, ForceMode.Acceleration);
        }

        void Jump()
        {
            grounded = false;
            
            // Physically make the character jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // Spawn jump dust particle
            if (jumpDustParticle != null) { Instantiate(jumpDustParticle, transform.position, Quaternion.identity); }
            
            //Jump Sound effect
            GetComponent<AudioSource>().clip = jumpSounds[UnityEngine.Random.Range(0, jumpSounds.Length)];
            GetComponent<AudioSource>().Play();
        }
#endregion

#region Public Utils
        public Vector3 GetColliderCenterPosition()
        {
            float colHalfHeight = (col.height / 2.0f);
            return new Vector3(transform.position.x, colHalfHeight, transform.position.z);
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