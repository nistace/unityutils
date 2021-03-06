using UnityEngine.Events;

namespace Utils.Hacking {
	public static class Hacks {
		public static UnityEvent onHackDetected { get; } = new UnityEvent();
	}
}