using Com.Isartdigital.EasterIsland.EasterIsland.Gameplay;
using Com.DefaultCompany.EasterIsland.EasterIsland;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.DefaultCompany.EasterIsland
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField]
        AudioClip attackSound, impactSound;

        [SerializeField]
        float attackVolume, impactVolume = 10f;
        BoxCollider boxCollider;
        Animator animator;

        public static bool isUpdate = true;
        public bool isGrounded = false;

        // Start is called before the first frame update
        void Awake()
        {
            boxCollider = GetComponentInChildren<BoxCollider>(true);
            //boxCollider.enabled = false;
            animator = GetComponentInParent<Animator>(true);

            //playerMovement = transform.parent.parent.parent.GetComponent<PlayerMovement>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isUpdate)
            {
                //if (animator == null)
                    //Debug.Log("animator is null");

                if (Input.GetButtonDown("Attack") && isGrounded)
                {
                    Debug.Log("Attacking");
                    AudioSource.PlayClipAtPoint(attackSound, transform.position, attackVolume);
                    animator.SetTrigger("Attack");
                }
            }
        }

        public void OnAttackStart()
        {
            Debug.Log("Hitbox should activate now");
        }

        private void OnTriggerEnter(Collider other)
        {
            Human enemy;
            if (other.CompareTag("Enemy"))
            {
                if (!other.TryGetComponent<Human>(out enemy))
                    return;

                enemy.StartAnimDie();

                Debug.Log("Enemy " + enemy.name + " hit!");
            }
        }
    }
}
