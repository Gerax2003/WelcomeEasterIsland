using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.DefaultCompany.EasterIsland.EasterIsland.Gameplay.Player {
    [RequireComponent(typeof(SphereCollider))]
    public class CharacterSpawn : MonoBehaviour
    {
        [SerializeField]
        CharacterSwap storedCharacter;

        [SerializeField]
        CharacterSwap player;

        Vector3 spawnPos;
        Quaternion spawnRot;

        float range;

        // Start is called before the first frame update
        void Start()
        {
            storedCharacter = GetComponentInChildren<CharacterSwap>();
            spawnPos = storedCharacter.transform.position;
            spawnRot = storedCharacter.transform.rotation;

            range = GetComponent<SphereCollider>().radius;
        }

        // Update is called once per frame
        void Update()
        {
            // TODO: Try moving this out of update (OnTriggerExit?)
            if (storedCharacter.transform.position == spawnPos && storedCharacter.transform.rotation == spawnRot)
                return;

            if (player != null && Vector3.Distance(player.transform.position, spawnPos) >= range)
            {
                storedCharacter.transform.position = spawnPos;
                storedCharacter.transform.rotation = spawnRot;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            CharacterSwap otherChar;

            Debug.Log(other.name + " entered");

            if (other.TryGetComponent<CharacterSwap>(out otherChar))
            {
                if (otherChar == storedCharacter)
                    return; 

                player = otherChar;
                player.charSwapEvent.AddListener(OnStatueSwap);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            CharacterSwap otherChar;

            Debug.Log(other.name + " exited");

            if (other.TryGetComponent<CharacterSwap>(out otherChar))
            {
                if (player != null && player.charSwapEvent.GetPersistentEventCount() > 0)
                    player.charSwapEvent.RemoveListener(OnStatueSwap);

                player = null;

                //if (storedCharacter.transform.position != spawnPos || storedCharacter.transform.rotation != spawnRot)
                //{
                //    storedCharacter.transform.position = spawnPos;
                //    storedCharacter.transform.rotation = spawnRot;
                //}
            }
        }

        void OnStatueSwap(GameObject oldPlayer, GameObject newPlayer)
        {
            if (newPlayer != storedCharacter.gameObject)
                return;

            storedCharacter = oldPlayer.GetComponent<CharacterSwap>();
            storedCharacter.charSwapEvent.RemoveListener(OnStatueSwap);
            player = newPlayer.GetComponent<CharacterSwap>();
            player.charSwapEvent.AddListener(OnStatueSwap);
        }
    }
}
