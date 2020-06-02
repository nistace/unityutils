using UnityEngine.Events;

public static class UnityEventExtension {
	#region NoArgs

	public static void AddListenerOnce(this UnityEvent thisEvent, UnityAction listener) {
		thisEvent.RemoveListener(listener);
		thisEvent.AddListener(listener);
	}

	public static void ChoseListener(this UnityEvent thisEvent, bool activateFirst, UnityAction first, UnityAction second) {
		thisEvent.SetListenerActive(first, activateFirst);
		thisEvent.SetListenerActive(second, !activateFirst);
	}

	public static void RemoveListeners(this UnityEvent thisEvent, params UnityAction[] listeners) {
		foreach (var t in listeners) {
			thisEvent.RemoveListener(t);
		}
	}

	public static void SetListenerActive(this UnityEvent thisEvent, UnityAction listener, bool active) {
		if (active) thisEvent.AddListenerOnce(listener);
		else thisEvent.RemoveListener(listener);
	}

	#endregion

	#region OneArg

	public static void AddListenerOnce<E>(this UnityEvent<E> thisEvent, UnityAction<E> listener) {
		thisEvent.RemoveListener(listener);
		thisEvent.AddListener(listener);
	}

	public static void RemoveListeners<E>(this UnityEvent<E> thisEvent, params UnityAction<E>[] listeners) {
		foreach (var t in listeners) {
			thisEvent.RemoveListener(t);
		}
	}

	public static void ChoseListener<E>(this UnityEvent<E> thisEvent, bool activateFirst, UnityAction<E> first, UnityAction<E> second) {
		thisEvent.SetListenerActive(first, activateFirst);
		thisEvent.SetListenerActive(second, !activateFirst);
	}

	public static void SetListenerActive<E>(this UnityEvent<E> thisEvent, UnityAction<E> listener, bool active) {
		if (active) thisEvent.AddListenerOnce(listener);
		else thisEvent.RemoveListener(listener);
	}

	#endregion

	#region TwoArgs

	public static void AddListenerOnce<E, F>(this UnityEvent<E, F> thisEvent, UnityAction<E, F> listener) {
		thisEvent.RemoveListener(listener);
		thisEvent.AddListener(listener);
	}

	public static void RemoveListeners<E, F>(this UnityEvent<E, F> thisEvent, params UnityAction<E, F>[] listeners) {
		foreach (var t in listeners) {
			thisEvent.RemoveListener(t);
		}
	}

	public static void ChoseListener<E, F>(this UnityEvent<E, F> thisEvent, bool activateFirst, UnityAction<E, F> first, UnityAction<E, F> second) {
		thisEvent.SetListenerActive(first, activateFirst);
		thisEvent.SetListenerActive(second, !activateFirst);
	}

	public static void SetListenerActive<E, F>(this UnityEvent<E, F> thisEvent, UnityAction<E, F> listener, bool active) {
		if (active) thisEvent.AddListenerOnce(listener);
		else thisEvent.RemoveListener(listener);
	}

	#endregion

	#region ThreeArgs

	public static void AddListenerOnce<E, F, G>(this UnityEvent<E, F, G> thisEvent, UnityAction<E, F, G> listener) {
		thisEvent.RemoveListener(listener);
		thisEvent.AddListener(listener);
	}

	public static void RemoveListeners<E, F, G>(this UnityEvent<E, F, G> thisEvent, params UnityAction<E, F, G>[] listeners) {
		foreach (var t in listeners) {
			thisEvent.RemoveListener(t);
		}
	}

	public static void ChoseListener<E, F, G>(this UnityEvent<E, F, G> thisEvent, bool activateFirst, UnityAction<E, F, G> first, UnityAction<E, F, G> second) {
		thisEvent.SetListenerActive(first, activateFirst);
		thisEvent.SetListenerActive(second, !activateFirst);
	}

	public static void SetListenerActive<E, F, G>(this UnityEvent<E, F, G> thisEvent, UnityAction<E, F, G> listener, bool active) {
		if (active) thisEvent.AddListenerOnce(listener);
		else thisEvent.RemoveListener(listener);
	}

	#endregion

	#region FourArgs

	public static void AddListenerOnce<E, F, G, H>(this UnityEvent<E, F, G, H> thisEvent, UnityAction<E, F, G, H> listener) {
		thisEvent.RemoveListener(listener);
		thisEvent.AddListener(listener);
	}

	public static void RemoveListeners<E, F, G, H>(this UnityEvent<E, F, G, H> thisEvent, params UnityAction<E, F, G, H>[] listeners) {
		foreach (var t in listeners) {
			thisEvent.RemoveListener(t);
		}
	}

	public static void ChoseListener<E, F, G, H>(this UnityEvent<E, F, G, H> thisEvent, bool activateFirst, UnityAction<E, F, G, H> first, UnityAction<E, F, G, H> second) {
		thisEvent.SetListenerActive(first, activateFirst);
		thisEvent.SetListenerActive(second, !activateFirst);
	}

	public static void SetListenerActive<E, F, G, H>(this UnityEvent<E, F, G, H> thisEvent, UnityAction<E, F, G, H> listener, bool active) {
		if (active) thisEvent.AddListenerOnce(listener);
		else thisEvent.RemoveListener(listener);
	}

	#endregion
}