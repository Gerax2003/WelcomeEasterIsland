///-----------------------------------------------------------------
/// Author : Adrien Lemaire
/// Date : 07/09/2022 10:22
///-----------------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Isartdigital.EasterIsland.EasterIsland.UI
{
    [DisallowMultipleComponent]
    public class Pause : BaseScreen 
	{
        [Header("GameObjects")]
        [SerializeField] private Button buttonResume = default;
        [SerializeField] private Button buttonQuit = default;

        public static event BaseScreenEventHandler OnResume;
        public static event BaseScreenEventHandler OnQuit;

        #region Active screen
        public override void ActivateButtons()
        {
            buttonResume.onClick.AddListener(Resume);
            buttonQuit.onClick.AddListener(Quit);
        }

        protected override void DeactivateButtons()
        {
            buttonResume.onClick.RemoveListener(Resume);
            buttonQuit.onClick.RemoveListener(Quit);
        }
        #endregion

        private void Resume()
        {
            StartDeactivate();
            OnResume?.Invoke(this);
        }

        private void Quit()
        {
            StartDeactivate();
            OnQuit?.Invoke(this);
        }
    }
}
