using UnityEngine;

namespace Utils.Behaviours {
	public class SinLocalScale : MonoBehaviour {
		[SerializeField] protected Vector3 _minScale;
		[SerializeField] protected Vector3 _maxScale;
		[SerializeField] protected float   _speed;

		private void Update() {
			transform.localScale = Vector3.Lerp(_minScale, _maxScale, (Mathf.Sin(Time.time * _speed) + 1) / 2);
		}
	}
}