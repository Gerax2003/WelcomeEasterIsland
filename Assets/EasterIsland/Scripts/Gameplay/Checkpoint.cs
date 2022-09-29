using Com.DefaultCompany.EasterIsland.EasterIsland.Gameplay.Player;
using Com.DefaultCompany.EasterIsland.EasterIsland;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.DefaultCompany.EasterIsland
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField]
        Transform spawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            PlayerMovement statue;
            if (other.TryGetComponent<PlayerMovement>(out statue))
            {
                PlayerMaster player = statue.GetComponentInParent<PlayerMaster>();
                Debug.Log("Player " + statue.name + " has entered " + name);
                player.SetNewCheckpoint(spawnPoint);
            }
        }
    }
}
