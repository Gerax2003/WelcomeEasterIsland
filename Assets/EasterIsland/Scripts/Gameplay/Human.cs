///-----------------------------------------------------------------
/// Author : Adrien Lemaire
/// Date : 06/09/2022 11:18
///-----------------------------------------------------------------

using Com.Isartdigital.EasterIsland.Common.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Isartdigital.EasterIsland.EasterIsland.Gameplay
{

    public delegate void HumanEventHandler(Human sender);

    [DisallowMultipleComponent]
    public class Human : MonoBehaviour 
	{
		[SerializeField] private ParticleSystem particleDeath = default;

        [SerializeField] private List<Garbage> wasteListPrefab = new List<Garbage>();
        private List<Garbage> randomWasteList = new List<Garbage>();

        private List<Garbage> wasteList = new List<Garbage>();

        [Header("Throws")]
        [SerializeField] private float timeBetweenThrows = 0;
        [SerializeField] private float timeBeforeGarbageDisappearing = 0f;
        [SerializeField] private float timeGarbageToDisappear = 0f;
        [SerializeField, Range(0, 1)] private float throwUpForce = 0f;
        [SerializeField] private float throwForce = 0;
        private float elapsedTime = 0f;

        public bool isDead = false;

        public static event HumanEventHandler OnDeath;

        public void GameLoop()
        {
            if (elapsedTime >= timeBetweenThrows)
            {
                ThrowTrash();
                elapsedTime = 0;
            }

            elapsedTime += Time.deltaTime;
        }

        public void ThrowTrash()
        {
            if (randomWasteList.Count == 0)
                randomWasteList = wasteListPrefab.CopyToList();

            int index = UnityEngine.Random.Range(0, randomWasteList.Count);

            Vector2 randomInCircle = UnityEngine.Random.insideUnitCircle;
            Vector3 randomDirection = new Vector3(randomInCircle.x, throwUpForce, randomInCircle.y).normalized;
            Quaternion rotation = UnityEngine.Random.rotation;

            Garbage garbage = Instantiate(randomWasteList[index], transform.position + Vector3.up * 2, rotation).GetComponent<Garbage>();
            garbage.Init(randomDirection, throwForce, timeBeforeGarbageDisappearing, timeGarbageToDisappear);

            randomWasteList.RemoveAt(index);

            wasteList.Add(garbage);
        }

        public void StartAnimDie()
        {
            if (isDead) return;

            isDead = true;

            OnDeath?.Invoke(this);

            GetComponent<Animator>().SetTrigger("Die");
        }

        public void EndAnimDie()
        {
            Instantiate(particleDeath, transform.position, Quaternion.identity);

            for (int i = wasteList.Count - 1; i >= 0; i--)
            {
                if (wasteList[i])
                {
                    Instantiate(particleDeath, wasteList[i].transform.position, Quaternion.identity);
                    Destroy(wasteList[i].gameObject);
                }

                wasteList.RemoveAt(i);
            }

            gameObject.SetActive(false);
        }
    }
}
