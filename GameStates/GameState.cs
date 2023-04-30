using System.Collections;
using UnityEngine;
using Utils.Coroutines;

namespace Utils.GameStates {
	public abstract class GameState {
		public static  GameState currentState  { get; private set; }
		public static  GameState previousState { get; private set; }
		private static Coroutine stateRoutine  { get; set; }

		protected bool enabled { get; private set; }

		protected abstract void Enable();
		protected abstract void Disable();
		protected abstract IEnumerator Continue();
		protected abstract void SetListenersEnabled(bool enabled);

		public static void ChangeState(GameState newState, bool force = false) {
			if (!force && currentState == newState) return;
			if (currentState != null) {
				currentState.enabled = false;
				currentState.SetListenersEnabled(false);
				if (stateRoutine != null) CoroutineRunner.Stop(stateRoutine);
				stateRoutine = null;
				currentState.Disable();
			}
			previousState = currentState;
			currentState = newState;
			if (currentState != null) {
				currentState.Enable();
				currentState.enabled = true;
				currentState.SetListenersEnabled(true);
				stateRoutine = CoroutineRunner.Run(currentState.Continue());
			}
		}

		public static void ChangeStateToPrevious(bool force = false) => ChangeState(previousState, force);
	}
}