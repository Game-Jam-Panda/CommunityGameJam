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
        [SerializeField] bool blockMovement = false;
        [SerializeField] float moveSpeed = 6.0f;
        [SerializeField] float allowMovementTreshhold = 0.0f;
        [Header("Rotation settings")]
        [SerializeField] bool blockRotation = false;    //TODO Remove exposure
        [SerializeField] float rotationSpeed = 10.0f;

        [Header("Jump settings")]
        [SerializeField] float jumpForce = 5.0f;
        [SerializeField] GameObject jumpDustParticle = null;
        [SerializeField] AudioClip[] jumpSounds;

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
            ProcessJump();
        }
        void FixedUpdate()
        {
            ProcessMovement();
        }
        
#region Knockback
        // Knockback
        public void Knockback(float freezeTime, float knockbackForce, Vector3 contactPointNormal)
        {
            //Freeze movement
            StartCoroutine(FreezeMovementForTime(freezeTime));

            //Knockback
            var knockbackDirection = (contactPointNormal - transform.forward);
            var knockback = knockbackDirection * knockbackForce;
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

            //Physically Rotate the player
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
            if(zMovementFrozen) { return; }
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            zMovementFrozen = true;
        }
        private void AllowRigidbodyZMovement()
        {
            if(!zMovementFrozen) { return; }
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            zMovementFrozen = false;
        }

        IEnumerator FreezeMovementForTime(float time)
        {
            //Freeze
            rb.velocity.Set(0.0f, 0.0f, 0.0f);
            //Block physical movement
            blockMovement = true;
            //Reset after freeze time
            yield return new WaitForSeconds(time);
            blockMovement = false;
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
            GetComponent<AudioSource>().clip = jumpSounds[UnityEngine.Random.Range(0, jumpSounds.Length)];
            GetComponent<AudioSource>().Play();
            grounded = false;
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