using System.Collections;
using UnityEngine;

namespace Utils.Coroutines {
	public class SingleCoroutine {
		private MonoBehaviour actor         { get; }
		private Coroutine     singleRoutine { get; set; }

		public SingleCoroutine(MonoBehaviour actor) {
			this.actor = actor;
		}

		public Coroutine Start(IEnumerator routine) {
			Stop();
			singleRoutine = actor.StartCoroutine(routine);
			return singleRoutine;
		}

		public void Stop() {
			if (singleRoutine != null) actor.StopCoroutine(singleRoutine);
		}
	}
}