namespace Utils.GameStates {
	public abstract class GameState {
		public static GameState currentState  { get; private set; }
		public static GameState previousState { get; private set; }

		protected abstract void Enable();
		protected abstract void Disable();

		public static void ChangeState(GameState newState, bool force = false) {
			if (!force && currentState == newState) return;
			currentState?.Disable();
			previousState = currentState;
			currentState = newState;
			currentState?.Enable();
		}

		public static void ChangeStateToPrevious(bool force = false) => ChangeState(previousState, force);
	}
}