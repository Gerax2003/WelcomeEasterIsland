///-----------------------------------------------------------------
/// Author : Adrien Lemaire
/// Date : 06/09/2022 12:17
///-----------------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Isartdigital.EasterIsland.EasterIsland.UI
{
    [DisallowMultipleComponent]
    public class TitleCard : BaseScreen 
	{
		[Header("GameObjects")]
		[SerializeField] private Button buttonPlay = default;
		[SerializeField] private Button buttonExit = default;

        public static event BaseScreenEventHandler OnPlay;

        #region Active screen
        public override void ActivateButtons()
        {
            buttonPlay.onClick.AddListener(Play);
            buttonExit.onClick.AddListener(Exit);
        }

        protected override void DeactivateButtons()
        {
            buttonPlay.onClick.RemoveListener(Play);
            buttonExit.onClick.RemoveListener(Exit);
        }
        #endregion

        private void Play()
        {
            StartDeactivate();
            OnPlay?.Invoke(this);
        }

        private void Exit()
        {
            Application.Quit();
        }
    }
}
