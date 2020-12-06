using System;
using System.Collections;
using UnityEngine;

namespace Utils.Ui {
	[RequireComponent(typeof(RectTransform))]
	public class MonoBehaviourUi : MonoBehaviour {
		private    RectTransform rt        { get; set; }
		public new RectTransform transform => rt ? rt : rt = GetComponent<RectTransform>();

		private Coroutine singleRoutine { get; set; }

		protected Coroutine StartSingleCoroutine(IEnumerator routine) {
			StopSingleCoroutine();
			singleRoutine = StartCoroutine(routine);
			return singleRoutine;
		}

		protected void StopSingleCoroutine() {
			if (singleRoutine != null) StopCoroutine(singleRoutine);
		}
	}
}