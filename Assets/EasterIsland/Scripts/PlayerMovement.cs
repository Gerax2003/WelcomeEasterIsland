using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.DefaultCompany.EasterIsland.EasterIsland {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        AudioClip jumpClip;
        [SerializeField]
        float jumpVolume = 10f;

        [SerializeField]
        List<AudioClip> footstepsClips = new List<AudioClip>();
        [SerializeField, Tooltip("Footsteps per second")]
        float footstepFrequency = 1f;
        [SerializeField]
        float footstepVolume = 10f;

        float footstepTimer;

        [SerializeField]
        LayerMask groundLayers;

        [SerializeField]
        Transform groundCastStartPoint;
        [SerializeField]
        float groundCastRadius = 1f;

        PlayerAttack attackScript;
        PlayerAttackJumper attackJumperScript;

        public Rigidbody rb;
        CapsuleCollider col;

        Vector3 movement = Vector3.zero;

        public float speed = 8f;
        public float jumpForce = 10f;
        public bool OnGround = true;

        public static bool dontMove = false;

        public bool isAttacking = false;

        public Transform checkpoint;

        // Start is called before the first frame update
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<CapsuleCollider>();
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;

            groundLayers &= ~(1 << LayerMask.NameToLayer("Player"));

            checkpoint = transform;

            attackScript = GetComponentInChildren<PlayerAttack>(true);
            if (attackScript == null)
            { 
                Debug.Log("Attack script is null");
                return;
            }

            attackJumperScript = GetComponentInChildren<PlayerAttackJumper>(true);
            if (attackJumperScript == null)
            {
                Debug.Log("AttackJumper script is null");
                return;
            }

            footstepTimer = 1f / footstepFrequency;
        }

        private void Update()
        {
            if (!dontMove)
            {
                bool isGrounded = IsGrounded();
                if (attackScript != null)
                    attackScript.isGrounded = isGrounded;

                // Stop all movement if attacking
                if (isAttacking)
                {
                    movement = Vector3.zero;
                    return;
                }

                // Horizontal movement
                movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

                // Jump
                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    rb.AddForce(Vector3.up * jumpForce);
                    AudioSource.PlayClipAtPoint(jumpClip, transform.position, jumpVolume);
                    Debug.Log("Jump");
                    isGrounded = false;
                }

                // Sound
                if (isGrounded && movement != Vector3.zero)
                {
                    if (footstepTimer < 0f)
                    {
                        AudioSource.PlayClipAtPoint(footstepsClips[Random.Range(0, footstepsClips.Count)], transform.position, footstepVolume);
                        footstepTimer = 1f / footstepFrequency;
                    }
                    footstepTimer -= Time.deltaTime;
                }
            }
        }

        void FixedUpdate()
        {
            if (dontMove)
            {
                rb.velocity = new Vector3(0f, 0f, 0f);
                return;
            }

            if (!dontMove)
            {
                if (movement == Vector3.zero)
                {
                    if (!IsGrounded())
                        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                    else
                        rb.velocity = new Vector3(0f, 0f, 0f);

                    return;
                }

                if (isAttacking)
                    return;

                // Rotation
                Quaternion rot = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);
                Vector3 horizontalVelocity = rot * movement;
                transform.forward = horizontalVelocity.normalized;

                // Clamp speed
                horizontalVelocity *= speed;

                if (horizontalVelocity.magnitude > speed)
                    horizontalVelocity = horizontalVelocity.normalized * speed;

                rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
            }
        }

        public bool IsGrounded()
        {
            Collider[] hits = new Collider[1];
            return Physics.OverlapSphereNonAlloc(groundCastStartPoint.position, groundCastRadius, hits, groundLayers, QueryTriggerInteraction.Ignore) > 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(groundCastStartPoint.position, groundCastRadius);
        }
    }
}
