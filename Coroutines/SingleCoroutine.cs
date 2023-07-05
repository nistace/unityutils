using System.Collections;
using UnityEngine;

namespace NiUtils.Coroutines {
	public class SingleCoroutine {
		private MonoBehaviour actor     { get; }
		private Coroutine     coroutine { get; set; }
		public  bool          running   { get; private set; }

		public SingleCoroutine(MonoBehaviour actor) {
			this.actor = actor;
		}

		public void Start(IEnumerator routine) {
			if (!actor) return;
			if (!actor.isActiveAndEnabled) return;
			if (running) Stop();
			coroutine = actor.StartCoroutine(RoutineWrapper(routine));
		}

		public void Stop() {
			if (!actor) return;
			if (!actor.enabled) return;
			if (!running) return;
			if (coroutine == null) return;
			actor.StopCoroutine(coroutine);
			running = false;
		}

		private IEnumerator RoutineWrapper(IEnumerator routine) {
			running = true;
			while (routine != null && routine.MoveNext()) yield return routine.Current;
			running = false;
		}
	}
}