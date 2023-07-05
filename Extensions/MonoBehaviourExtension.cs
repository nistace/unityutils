using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace NiUtils.Extensions {
	public static class MonoBehaviourExtension {
		public static void Delay(this MonoBehaviour monoBehaviour, UnityAction callback, float seconds) => monoBehaviour.StartCoroutine(DoDelay(callback, seconds));

		private static IEnumerator DoDelay(UnityAction callback, float seconds) {
			for (var secondsPassed = 0f; secondsPassed < seconds; ++secondsPassed) yield return null;
			callback();
		}

		public static bool TryStartCoroutine(this MonoBehaviour behaviour, IEnumerator routine, out Coroutine coroutine) {
			coroutine = null;
			if (!behaviour.gameObject.activeInHierarchy) return false;
			coroutine = behaviour.StartCoroutine(routine);
			return true;
		}

		public static T Enabled<T>(this T thisMb, bool enabled) where T : MonoBehaviour {
			thisMb.enabled = enabled;
			return thisMb;
		}
	}
}