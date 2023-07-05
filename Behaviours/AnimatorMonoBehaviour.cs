using System;
using System.Collections;
using UnityEngine;

namespace NiUtils.Behaviours {
	public abstract class AnimatorMonoBehaviour : MonoBehaviour {
		private   Animator sAnimator { get; set; }
		protected Animator animator  => sAnimator ? sAnimator : sAnimator = GetComponent<Animator>();

		private int  currentActionStep     { get; set; }
		public  bool currentActionComplete { get; private set; }

		public void OnCurrentActionNextStep() => currentActionStep++;

		protected IEnumerator DoPlayAction(int actionCompleteIndex, float timeToLive, params Action[] stepAction) {
			currentActionComplete = false;
			currentActionStep = 0;
			var lastPlayedIndex = -1;
			while (timeToLive > 0 && lastPlayedIndex < stepAction.Length - 1) {
				while (currentActionStep < lastPlayedIndex) yield return null;
				currentActionComplete = currentActionStep >= actionCompleteIndex;
				lastPlayedIndex++;
				stepAction[lastPlayedIndex]?.Invoke();
				timeToLive -= Time.deltaTime;
			}
			currentActionComplete = true;
		}
	}
}