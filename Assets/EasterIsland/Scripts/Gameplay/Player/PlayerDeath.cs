using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Com.DefaultCompany.EasterIsland
{
    [RequireComponent(typeof(BoxCollider))]
    public class PlayerDeath : MonoBehaviour
    {
        BoxCollider waterTrigger;

        public UnityEvent deathEvent;

        // Start is called before the first frame update
        void Awake()
        {
            waterTrigger = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                Debug.Log("Water over head level, firing death event");
                deathEvent.Invoke();
            }
        }
    }
}
