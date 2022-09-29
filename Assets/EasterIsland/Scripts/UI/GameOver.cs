///-----------------------------------------------------------------
/// Author : Adrien Lemaire
/// Date : 06/09/2022 13:58
///-----------------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Isartdigital.EasterIsland.EasterIsland.UI
{
	[DisallowMultipleComponent]
	public class GameOver : BaseScreen 
	{
		[SerializeField] private Button retryButton = default;
		[SerializeField] private Button quitButton = default;
	
		public static event BaseScreenEventHandler OnQuitGameOver;
		public static event BaseScreenEventHandler OnRetry;

		#region Active screen
		public override void ActivateButtons()
		{
			retryButton.onClick.AddListener(Retry);
			quitButton.onClick.AddListener(Quit);
		}

		protected override void DeactivateButtons()
		{
			retryButton.onClick.RemoveListener(Retry);
			quitButton.onClick.RemoveListener(Quit);
		}
		#endregion

		private void Retry()
		{
			gameObject.SetActive(false);
			OnRetry?.Invoke(this);
		}

		private void Quit()
		{
			gameObject.SetActive(false);
			OnQuitGameOver?.Invoke(this);
		}
	}
}
