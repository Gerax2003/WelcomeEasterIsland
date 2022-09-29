using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Com.DefaultCompany.EasterIsland.EasterIsland.Gameplay.Player {
    [System.Serializable]
    public class LifeChangeEvent : UnityEvent<float>
    {
    }

    public class PlayerLife : MonoBehaviour
    {
        LifeChangeEvent lifeChangeEvent;
        UnityEvent deathEvent;

        [SerializeField] private PlayerSettings playerSettings = default;
        
        float life;

        // Start is called before the first frame update
        void Start()
        {
            if (lifeChangeEvent == null)
                lifeChangeEvent = new LifeChangeEvent();

            if (deathEvent == null)
                deathEvent = new UnityEvent();

            lifeChangeEvent.AddListener(PrintLife);
            deathEvent.AddListener(PrintDeath);
        }

        public void TakeDamage(float damage)
        {
            if (damage == 0)
                return;

            life -= damage;

            if (life > playerSettings.life)
                life = playerSettings.life;

            if (life < 0)
            {
                life = 0;
                deathEvent.Invoke();
            }

            lifeChangeEvent.Invoke(life);
        }

        void PrintLife(float toPrint)
        {
            Debug.Log("Life = " + toPrint.ToString());
        }

        void PrintDeath()
        {
            Debug.Log("Death");
        }
    }
}
