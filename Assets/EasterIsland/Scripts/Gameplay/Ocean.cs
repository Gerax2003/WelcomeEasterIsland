///-----------------------------------------------------------------
/// Author : Adrien Lemaire
/// Date : 06/09/2022 11:04
///-----------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace Com.Isartdigital.EasterIsland.EasterIsland.Gameplay {
    [DisallowMultipleComponent]
    public class Ocean : MonoBehaviour 
	{
        [SerializeField]
        AudioSource audioSource;

        bool hasInit = false;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PullDownOcean(float height, float timeToGoDown, float waitTime)
        {
            Debug.Log("Puuling ocean down");

            audioSource.PlayOneShot(audioSource.clip);

            StartCoroutine(Wait(PullDown(height, timeToGoDown), waitTime));
        }

        public void PullDownOcean(float height)
        {
            transform.position += Vector3.down * height;
        }

        private IEnumerator Wait(IEnumerator coroutine, float time)
        {
            yield return new WaitForSeconds(time);

            StartCoroutine(coroutine);
        }

        private IEnumerator PullDown(float height, float time)
        {
            float elapsedTime = 0;
            Vector3 startPosition = transform.position;
            Vector3 endPosition = transform.position + Vector3.down * height;

            while (elapsedTime < time)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    [Serializable]
    public class OceanParams
    {
        public float levelHeight = 0f;
        public float levelDownTime = 0f;
        public float delay = 0f;
    }
}
