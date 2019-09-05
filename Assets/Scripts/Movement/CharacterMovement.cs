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
        const string ANIM_JUMPING = "jumping";
        const string ANIM_FALLING = "falling";

        [Header("Jump settings")]
        [SerializeField] float jumpForce = 6.0f;
        [SerializeField] GameObject jumpDustParticle = null;
        [SerializeField] AudioClip[] jumpSounds;
        [SerializeField] float gravityMultiplier = 3.0f;
        [SerializeField] float fallingSpeedLimit = 8f;
        [SerializeField] float fallingVelocityTreshhold = 8.61f;
        bool useGravity = false;
        bool isJumping = false;
        bool isFalling = false;

        [Header("Grounded settings")]
        [SerializeField] LayerMask groundedLayers;
        [Range(0.01f, 1.0f)] [SerializeField] float groundedHeightOffset = 0.05f;
        [SerializeField] float groundedRadiusOffset = 0.1f;
        [Range(0.05f, 1.0f)] [SerializeField] float groundedCheckDistance = 0.15f;
        [SerializeField] Transform groundedCenterChecksource = null;
        [SerializeField] Transform groundedBehindChecksource = null;
        [SerializeField] Transform groundedFrontChecksource = null;
        Vector3 groundedCenterPosition;
        Vector3 groundedBehindPosition;
        Vector3 groundedFrontPosition;
        bool groundedCenter = true;
        bool groundedBehind = true;
        bool groundedFront = true;
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
        BoxCollider col;
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
            col = GetComponent<BoxCollider>();
            characterHealth = GetComponent<HealthSystem>();
            anim = GetComponent<Animator>();
            cam = Camera.main;
        }
        void Start()
        {
            UpdateGroundedChecksourcesPositions();
        }

        void Update()
        {
            //Animator
            UpdateAnimator();
            
            //Input
            ProcessMovementInput();

            //Checks
            ProcessGroundedCheck();
            ProcessFallingCheck();

            if(controlsBlocked) { return; }
            if(frozen) {return; }
            
            //Mechanics
            ProcessJump();
        }
        void FixedUpdate()
        {
            if(useGravity) { ProcessFallingGravity(); }

            if(controlsBlocked) { isMoving = false; return; }
            if(frozen) { isMoving = false; return; }

            //Mechanics
            ProcessMovement();
        }

        void UpdateGroundedChecksourcesPositions()
        {
            groundedCenterChecksource.localPosition = new Vector3(0, groundedHeightOffset, 0);
            groundedBehindChecksource.localPosition = new Vector3(0, groundedHeightOffset, -(col.size.z/2 + groundedRadiusOffset));
            groundedFrontChecksource.localPosition = new Vector3(0, groundedHeightOffset, col.size.z/2 + groundedRadiusOffset);
        }

#region Animator

    void UpdateAnimator()
    {
        anim.SetFloat(ANIM_FORWARD, inputMagnitudeClamped);
        anim.SetBool(ANIM_MOVING, isMoving);
        anim.SetBool(ANIM_GROUNDED, groundedCenter);
        anim.SetBool(ANIM_FALLING, isFalling);
        anim.SetBool(ANIM_JUMPING, isJumping);
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

        //*** JUMP ***
        void ProcessJump()
        {
            //Jump mechanic
            if(grounded && Input.GetKeyDown(KeyCode.Space))
            {
                // Prevent from jumping multiple times
                if(rb.velocity.y > 0) { return; }
                
                // Initiate jump
                anim.SetTrigger(ANIM_JUMP_TRIGGER);
                Jump();
            }
        }
        void ProcessGroundedCheck()
        {
            // Grounded check
            groundedCenter = Physics.Raycast(groundedCenterChecksource.position, Vector3.down, groundedCheckDistance, groundedLayers);
            groundedBehind = Physics.Raycast(groundedBehindChecksource.position, Vector3.down, groundedCheckDistance, groundedLayers);
            groundedFront = Physics.Raycast(groundedFrontChecksource.position, Vector3.down, groundedCheckDistance, groundedLayers);
            // Being grounded means there's a least one "grounded-check Source" that's grounded
            grounded = groundedFront || groundedBehind || groundedCenter;
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

        //*** FALLING ***
        void ProcessFallingGravity()
        {
            //Gravity falling effect while in the air
            if(rb.velocity.y < -fallingSpeedLimit) { return; }
            rb.AddForce(Vector3.down * gravityMultiplier * 100 * Time.deltaTime, ForceMode.Acceleration);
        }
        void ProcessFallingCheck()
        {
            bool isPhysicallyJumping = rb.velocity.y > 0.0f;
            bool isPhysicallyFalling = rb.velocity.y < 0.0f;

            //Physically falling
            if(!grounded && isPhysicallyFalling)
            {
                isFalling = true;
            }
            else
            {
                isFalling = false;
            }

            //In the air
            if(!grounded && isPhysicallyJumping)
            {
                isJumping = true;
            }
            else
            {
                isJumping = false;
            }

            //Use gravity effect
            if(!grounded && rb.velocity.y < fallingVelocityTreshhold)
            {
                useGravity = true;
            }
            else
            {
                useGravity = false;
            }
        }
#endregion

#region Public Utils
        public Vector3 GetColliderCenterPosition()
        {
            float colHalfHeight = (col.size.y / 2.0f);
            return new Vector3(transform.position.x, colHalfHeight, transform.position.z);
        }
#endregion

#region GIZMOS

        // Debug Gizmos to see grounded distance
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            //Center grounded-check source
            Gizmos.DrawLine(groundedCenterChecksource.position,
                new Vector3(groundedCenterChecksource.position.x, groundedCenterChecksource.position.y - groundedCheckDistance, groundedCenterChecksource.position.z)
            );
            //Behind grounded-check source
            Gizmos.DrawLine(groundedBehindChecksource.position,
                new Vector3(groundedBehindChecksource.position.x, groundedBehindChecksource.position.y - groundedCheckDistance, groundedBehindChecksource.position.z)
            );
            //Front grounded-check source
            Gizmos.DrawLine(groundedFrontChecksource.position,
                new Vector3(groundedFrontChecksource.position.x, groundedFrontChecksource.position.y - groundedCheckDistance, groundedFrontChecksource.position.z)
            );
        }
#endregion
    }
}