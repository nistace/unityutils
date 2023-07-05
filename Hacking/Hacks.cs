using UnityEngine.Events;

namespace NiUtils.Hacking {
	public static class Hacks {
		public static UnityEvent onHackDetected { get; } = new UnityEvent();
	}
}