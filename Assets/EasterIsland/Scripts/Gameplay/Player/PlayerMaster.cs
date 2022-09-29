using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.DefaultCompany.EasterIsland.EasterIsland.Gameplay.Player {
    public class PlayerMaster : MonoBehaviour
    {
        [SerializeField]
        AudioClip swapSound;
        [SerializeField]
        float swapVolume = 10f;

        [SerializeField]
        GameObject basicMoai;
        [SerializeField]
        GameObject jumperMoai;
        [SerializeField]
        GameObject thinMoai;

        [SerializeField]
        bool jumperUnlocked = false;
        [SerializeField]
        bool thinUnlocked = false;

        int currentStatue = 0;
        public int moaiNumber = 3;

        string basicName, jumperName, thinName;
        [SerializeField]
        Cinemachine.CinemachineFreeLook cameraController;

        Transform checkpoint;

        PlayerAttack basicAttack, thinAttack;

        void Awake()
        {
            //cameraController = FindObjectOfType<Cinemachine.CinemachineFreeLook>();

            basicName = basicMoai.name;
            jumperName = jumperMoai.name;
            thinName = thinMoai.name;

            basicMoai.GetComponentInChildren<PlayerDeath>().deathEvent.AddListener(RespawnPlayer);
            jumperMoai.GetComponentInChildren<PlayerDeath>().deathEvent.AddListener(RespawnPlayer);
            thinMoai.GetComponentInChildren<PlayerDeath>().deathEvent.AddListener(RespawnPlayer);

            basicMoai.SetActive(true);
            jumperMoai.SetActive(false);
            thinMoai.SetActive(false);

            basicAttack = basicMoai.GetComponentInChildren<PlayerAttack>();
            thinAttack = thinMoai.GetComponentInChildren<PlayerAttack>();

            checkpoint = transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (!jumperUnlocked && !thinUnlocked)
                return;

            if (PlayerAttack.isUpdate)
            {
                if (Input.GetButtonDown("SwapStatue"))
                {
                    GameObject current = GetStatueByIndex(currentStatue);
                    GameObject next = GetStatueByIndex(currentStatue + 1);

                    // TODO: Play() with audio source comp
                    if (current != next)
                        AudioSource.PlayClipAtPoint(swapSound, current.transform.position, swapVolume);

                    Debug.Log("Swapping from " + current.name + " to " + next.name);

                    // Keep track of current position
                    Transform swapPosition = current.transform;

                    // Activate new statue and deactivate old one
                    current.SetActive(false);
                    next.SetActive(true);

                    // Set position from last swap pos to current position
                    next.transform.position = swapPosition.position + (Vector3.up * 0.2f); // +0.2 in Y to avoid falling in the ground
                    next.transform.rotation = swapPosition.rotation;

                    // Change camera target
                    cameraController.Follow = next.transform;
                    cameraController.LookAt = next.transform;

                    currentStatue = GetIndexByStatue(next);
                }
            }
        }

        private int GetIndexByStatue(GameObject statue)
        {
            if (statue.name == basicName)
                return 0;
            else if (statue.name == jumperName)
                return 1;
            else if (statue.name == thinName)
                return 2;
            else
                return 0;
        }

        private GameObject GetStatueByIndex(int statue)
        {
            if (statue > moaiNumber || statue < 0)
                statue = 0;

            switch (statue)
            {
                case 0:
                    return basicMoai;
                case 1:
                    if (jumperUnlocked)
                        return jumperMoai;
                    else 
                        return GetStatueByIndex(statue + 1);
                case 2:
                    if (thinUnlocked)
                        return thinMoai;
                    else
                        return GetStatueByIndex(statue + 1);
                default:
                    return basicMoai;
            }
        }

        public void UnlockThin(bool setUnlock)
        {
            thinUnlocked = setUnlock;
        }

        public void UnlockJumper(bool setUnlock)
        {
            jumperUnlocked = setUnlock;
        }

        public Vector3 GetPlayerPos()
        {
            GameObject statue = GetStatueByIndex(currentStatue);
            return statue.transform.position;
        }

        public void SetNewCheckpoint(Transform newCheckpoint)
        {
            Debug.Log("Setting new CP");
            checkpoint = newCheckpoint;
        }

        public void RespawnPlayer()
        {
            GameObject statue = GetStatueByIndex(currentStatue);
            statue.transform.position = checkpoint.position;
            statue.transform.rotation = checkpoint.rotation;
            Debug.Log("Respawned at " + checkpoint.position.ToString());
        }
    }
}
