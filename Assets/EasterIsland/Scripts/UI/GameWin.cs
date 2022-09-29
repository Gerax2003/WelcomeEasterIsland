///-----------------------------------------------------------------
/// Author : Adrien Lemaire
/// Date : 06/09/2022 14:14
///-----------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Isartdigital.EasterIsland.EasterIsland.UI
{
	[DisallowMultipleComponent]
	public class GameWin : BaseScreen 
	{
		[SerializeField] private Button nextButton = default;

		public static event BaseScreenEventHandler OnNext;

		#region Active screen
		public override void ActivateButtons()
		{
			nextButton.onClick.AddListener(Next);
		}

		protected override void DeactivateButtons()
		{
			nextButton.onClick.RemoveListener(Next);
		}

        public override void StartActivate(Action actionAfterDeactivate = null)
        {
            base.StartActivate(actionAfterDeactivate);
			nextButton.Select();
        }
        #endregion

        private void Next()
		{
			gameObject.SetActive(false);
			OnNext?.Invoke(this);
		}
	}
}
