using System.Collections;
using UnityEngine;

namespace Utils.Coroutines {
	public class SingleCoroutine {
		private MonoBehaviour actor         { get; }
		private Coroutine     singleRoutine { get; set; }

		public SingleCoroutine(MonoBehaviour actor) {
			this.actor = actor;
		}

		public void Start(IEnumerator routine) {
			if (!actor) return;
			if (!actor.isActiveAndEnabled) return;
			Stop();
			singleRoutine = actor.StartCoroutine(routine);
		}

		public void Stop() {
			if (!actor) return;
			if (!actor.enabled) return;
			if (singleRoutine != null) actor.StopCoroutine(singleRoutine);
		}
	}
}