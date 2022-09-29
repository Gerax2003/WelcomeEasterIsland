using Com.DefaultCompany.EasterIsland.EasterIsland.Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.DefaultCompany.EasterIsland
{
    public delegate void StatueUnlockEventHandler(StatueUnlock sender);

    public class StatueUnlock : MonoBehaviour
    {
        [SerializeField, Tooltip("Player detection range (cyan sphere in scene)")]
        float range = 12.5f;

        [SerializeField]
        bool unlocksJumper, unlocksThin = false;

        PlayerMaster player;

        public static event StatueUnlockEventHandler OnGetCloseToStatue;
        public static event StatueUnlockEventHandler OnGetFarFromStatue;

        private bool dontCallTwoTimes = false;

        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<PlayerMaster>();
        }

        // Update is called once per frame
        void Update()
        {
            if (PlayerAttack.isUpdate)
            {
                if (Vector3.Distance(player.GetPlayerPos(), transform.position) <= range)
                {
                    if (dontCallTwoTimes)
                    {
                        dontCallTwoTimes = false;
                        OnGetCloseToStatue?.Invoke(this);
                    }

                    if (Input.GetButtonDown("SwapStatue") && Vector3.Distance(player.GetPlayerPos(), transform.position) <= range)
                    {
                        Debug.Log("Unlocking corresponding statue (jumper = " + unlocksJumper + ", thin = " + unlocksThin + ")");

                        if (unlocksJumper)
                            player.UnlockJumper(true);

                        if (unlocksThin)
                            player.UnlockThin(true);
                    }
                }
                else
                {
                    if (!dontCallTwoTimes)
                    {
                        dontCallTwoTimes = true;
                        OnGetFarFromStatue?.Invoke(this);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
