using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Utils.Extensions {
	public static class MonoBehaviourExtension {
		public static void Delay(this MonoBehaviour monoBehaviour, UnityAction callback, float seconds) => monoBehaviour.StartCoroutine(DoDelay(callback, seconds));

		private static IEnumerator DoDelay(UnityAction callback, float seconds) {
			for (var secondsPassed = 0f; secondsPassed < seconds; ++secondsPassed) yield return null;
			callback();
		}
	}
}