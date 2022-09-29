///-----------------------------------------------------------------
/// Author : Adrien Lemaire
/// Date : 06/09/2022 15:39
///-----------------------------------------------------------------

using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Com.Isartdigital.EasterIsland.EasterIsland.Gameplay
{
    [RequireComponent(typeof(Collider))]
	public class PopupKeyToPress : MonoBehaviour 
	{
        private Transform _camera = default;

        private Action action = default;

        #region Unity Methods
        private void Start()
        {
            action = VoidMode;
        }

        public void Init(Transform camera)
        {
            _camera = camera;
            action = GameLoopMode;
        }

        private void Update () {
            action();
		}
        #endregion

        private void GameLoopMode()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _camera.position, Vector3.up);
        }

        private void VoidMode(){ }
    }
}
