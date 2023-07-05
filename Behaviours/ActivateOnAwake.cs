using UnityEngine;

namespace NiUtils.Behaviours {
	public class ActivateOnAwake : MonoBehaviour {
		[SerializeField] protected GameObject[] _objectsToActivate;

		private void Awake() {
			foreach (var objectToActive in _objectsToActivate) objectToActive.SetActive(true);
		}
	}
}