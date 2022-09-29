///-----------------------------------------------------------------
/// Author : Adrien Lemaire
/// Date : 07/09/2022 09:38
///-----------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace Com.Isartdigital.EasterIsland.EasterIsland.UI
{
	public delegate void BaseScreenEventHandler(BaseScreen sender);
	public abstract class BaseScreen : MonoBehaviour 
	{
		public static readonly string active = "IsActive";
		public static readonly string startIn = "StartIn";

		[SerializeField] protected float timeAnimEnd = 0f;

		#region active
		public virtual void StartActivate(Action actionAfterDeactivate = null)
		{
			GetComponent<Animator>().SetTrigger(active);

			StartCoroutine(WaitAnimEnd(EndActivate, actionAfterDeactivate, timeAnimEnd));

			ActivateButtons();
		}

		public virtual void StartDeactivate(Action actionAfterDeactivate = null)
		{
			GetComponent<Animator>().ResetTrigger(active);

			StartCoroutine(WaitAnimEnd(EndDeactivate, actionAfterDeactivate, timeAnimEnd));

			DeactivateButtons();
		}

		public virtual void ActivateButtons() { }


		//These methods are called when the animation has finished
		protected virtual void EndActivate(Action actionAfterActivate)
		{
			if (actionAfterActivate != null)
				actionAfterActivate();
		}

		protected virtual void EndDeactivate(Action actionAfterActivate)
		{
			if (actionAfterActivate != null)
				actionAfterActivate();
		}

		protected virtual void DeactivateButtons() { }
		#endregion

		#region Coroutine
		protected IEnumerator WaitAnimEnd(Action<Action> action, Action actionAfterDeactivate, float timeAnimEnd)
		{
			yield return new WaitForSeconds(timeAnimEnd);

			action(actionAfterDeactivate);
		}
		#endregion
	}
}
