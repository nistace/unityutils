using System.Collections;
using UnityEngine;
using Utils.Extensions;

namespace Utils.Coroutines {
	public class CoroutineRunner : MonoBehaviour {
		private static CoroutineRunner mInstance { get; set; }

		private static CoroutineRunner instance {
			get {
				if (mInstance) return mInstance;
				mInstance = new GameObject("CoroutineRunner").GetOrAddComponent<CoroutineRunner>();
				DontDestroyOnLoad(mInstance.gameObject);
				return mInstance;
			}
		}

		public static Coroutine Run(IEnumerator routine) => instance.StartCoroutine(routine);
		public static void Stop(Coroutine routine) => instance.StopCoroutine(routine);
	}
}