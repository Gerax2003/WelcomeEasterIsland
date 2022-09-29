using Com.Isartdigital.EasterIsland.EasterIsland.Gameplay;
using Com.DefaultCompany.EasterIsland.EasterIsland;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.DefaultCompany.EasterIsland
{
    public class PlayerAttackJumper : MonoBehaviour
    {
        [SerializeField]
        AudioClip impactSound;

        [SerializeField]
        float attackVolume, impactVolume = 1f;
        BoxCollider boxCollider;

        PlayerMovement player;

        public static bool isUpdate = true;
        public bool isGrounded = false;

        public float speedToAttack = -39f;
        bool isAttacking = false;

        // Start is called before the first frame update
        void Awake()
        {
            boxCollider = GetComponentInChildren<BoxCollider>(true);
            Debug.Log(boxCollider.name);
            player = GetComponentInParent<PlayerMovement>(true);
            if (player == null)
                Debug.Log("player is null");
        }

        // Update is called once per frame
        void Update()
        {
            if (!isUpdate)
                return;

            Debug.Log("Player.IsGrounded() = " + player.IsGrounded() + ", y vel = " + player.rb.velocity.y.ToString() + ", isAttacking = " + isAttacking);

            if (!player.IsGrounded() && player.rb.velocity.y <= speedToAttack)
            {
                isAttacking = true;
                boxCollider.enabled = true;
            }

            if (isAttacking && player.IsGrounded())
            {
                isAttacking = false;
                boxCollider.enabled = false;
                AudioSource.PlayClipAtPoint(impactSound, player.transform.position);
            }
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