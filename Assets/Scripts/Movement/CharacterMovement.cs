using System.Collections;
using CGJ.Core;
using CGJ.System;
using UnityEngine;

namespace CGJ.Movement
{
    public class CharacterMovement : MonoBehaviour
    {
        [Header("Movement settings")]
        [SerializeField] bool lockVerticalMovement = true;
        [SerializeField] float moveSpeed = 6.0f;
        [SerializeField] float allowMovementTreshhold = 0.0f;
        [SerializeField] float footstepsVolume = 0.2f;
        [SerializeField] AudioClip[] footstepSounds;
        bool isMoving;
        bool blockMovement = false;
        bool controlsBlocked = false;
        bool zMovementFrozen = true;
        bool frozen = false;

        [Header("Rotation settings")]
        [SerializeField] float rotationSpeed = 10.0f;
        bool blockRotation = false;

        // Animator
        Animator anim;
        const string ANIM_FORWARD = "forward";
        const string ANIM_MOVING = "moving";
        const string ANIM_GROUNDED = "grounded";
        const string ANIM_JUMP_TRIGGER = "jump";
        const string ANIM_FALLING = "falling";

        [Header("Jump settings")]
        [SerializeField] float jumpForce = 6.0f;
        [SerializeField] GameObject jumpDustParticle = null;
        [SerializeField] AudioClip[] jumpSounds;
        [SerializeField] float gravityMultiplier = 3.0f;
        [SerializeField] float fallingSpeedLimit = 8f;
        [SerializeField] float fallingVelocityTreshhold = 8.61f;
        bool isFalling = false;

        [Header("Grounded settings")]
        [SerializeField] LayerMask groundedLayers;
        [SerializeField] Transform groundedCheckSource = null;
        [SerializeField] float groundedCheckDistance = 0.1f;
        bool grounded = true;

        // [Header("Landing settings")]
        // [SerializeField] AudioClip[] landSounds;
        
        //Input
        float InputX = 0.0f;
        float InputZ = 0.0f;
        float inputMagnitude;
        float inputMagnitudeClamped;

        //Movement values
        Vector3 movementDirection;
        Vector3 movement;

        //Rotation
        Vector3 camLookAtRotation;

        //References
        Rigidbody rb;
        CapsuleCollider col;
        Camera cam;
        HealthSystem characterHealth;

        public Rigidbody GetRigidbody() { return rb; }
        public bool ControlsBlocked() { return controlsBlocked; }

        //Public Setters
        public void FreezeControls() { controlsBlocked = true; }
        public void UnfreezeControls() { controlsBlocked = false; }

        // Delegate Subscription
        void OnEnable()
        { characterHealth.onDeath += FreezeControls; }
        void OnDisable()
        { characterHealth.onDeath -= FreezeControls; }

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<CapsuleCollider>();
            characterHealth = GetComponent<HealthSystem>();
            anim = GetComponent<Animator>();
            cam = Camera.main;
        }

        void Update()
        {
            UpdateAnimator();
            
            ProcessMovementInput();

            if(controlsBlocked) { return; }

            if(frozen) {return; }
            ProcessJump();
        }
        void FixedUpdate()
        {
            if(isFalling) { ProcessFalling(); }

            if(controlsBlocked) { return; }
            if(frozen) { return; }

            ProcessMovement();
        }

#region Animator

    void UpdateAnimator()
    {
        anim.SetFloat(ANIM_FORWARD, inputMagnitudeClamped);
        anim.SetBool(ANIM_MOVING, isMoving);
        anim.SetBool(ANIM_GROUNDED, grounded);
        anim.SetBool(ANIM_FALLING, isFalling);
    }
#endregion

#region Knockback
        // Knockback
        public void Knockback(float freezeTime, float knockbackForceX, float knockbackForceY, Vector3 contactPointNormal)
        {
            //Freeze movement
            StartCoroutine(FreezeForTime(freezeTime));

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

        //Animation events
        public void FootL()
        {
            if(footstepSounds != null)
            {
                SystemManager.systems.soundManager.PlayRandomSound(footstepSounds, footstepsVolume);
            }
        }
        public void FootR()
        {
            FootL();
        }

        void ProcessMovementInput()
        {
            // Left and right movement
            InputX = Input.GetAxis("Horizontal");
             
            // Depth movement
            if(lockVerticalMovement)
            {
                if(InputZ != 0.0f) { InputZ = 0.0f; }
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
                isMoving = true;
                RotateAndMove();
            }
            else
            {
                isMoving = false;
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

        IEnumerator FreezeForTime(float time)
        {
            //Freeze
            rb.velocity.Set(0.0f, 0.0f, 0.0f);
            //Block physical movement
            frozen = true;

            //Reset after freeze time
            yield return new WaitForSeconds(time);
            frozen = false;
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
                anim.SetTrigger(ANIM_JUMP_TRIGGER);
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
            var randomJumpSound = jumpSounds[UnityEngine.Random.Range(0, jumpSounds.Length)];
            GetComponent<AudioSource>().PlayOneShot(randomJumpSound);
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