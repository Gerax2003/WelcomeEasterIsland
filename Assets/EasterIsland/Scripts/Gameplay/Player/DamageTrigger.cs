using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.DefaultCompany.EasterIsland.EasterIsland.Gameplay.Player {
    [RequireComponent(typeof(Collider))]
    public class DamageTrigger : MonoBehaviour
    {
        Collider col;
        public float damage = 1f;

        // Start is called before the first frame update
        void Start()
        {
            col = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerLife playerLife;

            if (other.TryGetComponent<PlayerLife>(out playerLife))
                playerLife.TakeDamage(damage);
        }
    }
}
