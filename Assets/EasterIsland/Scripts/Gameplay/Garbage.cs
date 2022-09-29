///-----------------------------------------------------------------
/// Author : Adrien Lemaire
/// Date : 07/09/2022 11:46
///-----------------------------------------------------------------

using Com.Isartdigital.EasterIsland.EasterIsland.Managers;
using System;
using System.Collections;
using UnityEngine;

namespace Com.Isartdigital.EasterIsland.EasterIsland.Gameplay
{
    [DisallowMultipleComponent, RequireComponent(typeof(Rigidbody), typeof(Collider))]
	public class Garbage : MonoBehaviour 
	{
        private Rigidbody _rigidbody = default;

        //Time
		private float timeBeforeDisappearing = 0f;
		private float timeToDisappear = 0f;
        private float elapsedTimeBefore = 0f;
        private float elapsedTime = 0f;

        //Movement
        private float gravity = 0f;
        private float acceleration = 0;
        private Vector3 velocity;

        private Action DoAction;
        private Action DoActionFixed;

        #region Unity Methods
        private void Awake()
        {
            DoAction = Void;
            DoActionFixed = FixedVoid;
        }

        public void Init(Vector3 velocity, float throwForce, float timeBeforeDisappearing, float timeToDisappear)
        {
            DoActionFixed = Fly;

            this.timeBeforeDisappearing =   timeBeforeDisappearing;
            this.timeToDisappear =          timeToDisappear;
            this.velocity =                 velocity * throwForce;

            _rigidbody = GetComponent<Rigidbody>();

            gravity = GameManager._gravity;
        }

        private void Update()
        {
            DoAction();
        }

        private void FixedUpdate()
        {
            DoActionFixed();
        }
        #endregion

        #region State Machine FixedUpdate
        private void FixedVoid(){}

        private void Fly()
        {
            acceleration -= gravity * Time.deltaTime;
            velocity.y += acceleration;
            transform.position += velocity;
        }
        #endregion

        #region State Machine Update
        private void Void() { }

        private void WaitDisappear()
        {
            if (elapsedTimeBefore >= timeBeforeDisappearing)
                DoAction = Disappear;

            elapsedTimeBefore += Time.deltaTime;
        }

        private void Disappear()
        {
            if (elapsedTime >= timeToDisappear)
                Destroy(gameObject);

            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsedTime / timeToDisappear);

            elapsedTime += Time.deltaTime;
        }
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GameManager.groundTag))
            {
                _rigidbody.useGravity = false;
                _rigidbody.isKinematic = true;

                DoAction = WaitDisappear;
                DoActionFixed = FixedVoid;

                transform.position = other.ClosestPointOnBounds(transform.position/* + Vector3.up * 0.5f*/);
            }
        }
    }
}
