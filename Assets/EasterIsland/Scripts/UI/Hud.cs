///-----------------------------------------------------------------
/// Author : Adrien Lemaire
/// Date : 08/09/2022 16:33
///-----------------------------------------------------------------

using Com.Isartdigital.EasterIsland.EasterIsland.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.DefaultCompany.EasterIsland.EasterIsland.UI
{
	[DisallowMultipleComponent]
	public class Hud : BaseScreen 
	{
		[SerializeField] private TextMeshProUGUI humanCounter = default;
		[SerializeField] private TextMeshProUGUI mapKey = default;
		[SerializeField] private GameObject keyE = default;

        public void SetKeyE(bool value)
        {
            if (keyE) keyE.SetActive(value);
        }

        public override void ActivateButtons()
        {
            if (Input.GetJoystickNames().Length > 0)
                mapKey.text = "RT";
            else mapKey.text = "shift";

            base.ActivateButtons();
        }

        public void SetCounter(int nHuman)
        {
			humanCounter.text = nHuman.ToString();
        }
	}
}
