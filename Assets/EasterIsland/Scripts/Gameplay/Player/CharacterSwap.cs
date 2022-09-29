using Com.Isartdigital.EasterIsland.EasterIsland.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Com.DefaultCompany.EasterIsland.EasterIsland.Gameplay.Player {
    [System.Serializable]
    public class CharSwapEvent : UnityEvent<GameObject, GameObject>
    {
    }

    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterSwap : MonoBehaviour
    {
        public static readonly string playerTag = "Player";
        public static readonly string swapStatueControlName = "SwapStatue";

        [SerializeField] private GameObject popupKeyToPressPrefab = default;
        [HideInInspector] public GameObject popupKeyToPress = default;

        public CharSwapEvent charSwapEvent;

        PlayerMovement movementScript;
        Rigidbody rb;

        [SerializeField]
        Cinemachine.CinemachineFreeLook cameraController;

        [SerializeField]
        List<GameObject> otherCharacters;

        // Using awake so this script can get a reference to all the other scripts before they are deactivated
        void Awake()
        {
            if (charSwapEvent == null)
                charSwapEvent = new CharSwapEvent();

            charSwapEvent.AddListener(PrintSwap);

            movementScript = GetComponent<PlayerMovement>();
            cameraController = FindObjectOfType<Cinemachine.CinemachineFreeLook>();
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        public void Update()
        {
            if (PlayerAttack.isUpdate)
            {
                if (Input.GetButtonDown(swapStatueControlName))
                {
                    if (otherCharacters.Count <= 0)
                        return;

                    CharacterSwap otherSwap = otherCharacters[0].GetComponent<CharacterSwap>();

                    if (otherSwap == null)
                        return;

                    // Activate player only scripts on other character
                    otherSwap.enabled = true;
                    otherSwap.movementScript.enabled = true;
                    otherSwap.rb.isKinematic = false;

                    // Change camera target
                    cameraController.Follow = otherSwap.transform;
                    cameraController.LookAt = otherSwap.transform;

                    // Deactivate player only scripts on self
                    movementScript.enabled = false;
                    enabled = false;
                    rb.isKinematic = true;

                    // Fire event to update other elements (Ex: UI listeners). Fire before deactivation in case a component is needed by a sub.
                    charSwapEvent.Invoke(gameObject, otherSwap.gameObject);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == playerTag)
            {
                otherCharacters.Add(other.gameObject);
                InstantiatePopupKeyToPress(other.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == playerTag)
                otherCharacters.Remove(other.gameObject);
        }

        void PrintSwap(GameObject self, GameObject other)
        {
            Debug.Log("Swapping from " + self.name + " to " + other.name);
        }

        public void InstantiatePopupKeyToPress(Transform statue)
        {
            popupKeyToPress = Instantiate(popupKeyToPressPrefab, statue.position + Vector3.up * statue.lossyScale.y / 2, Quaternion.identity, statue.transform);
            popupKeyToPress.GetComponent<PopupKeyToPress>().Init(Camera.main.transform);
        }
    }
}
