using UnityEngine;

namespace Utils.Ui {
	public class PingPongUi : MonoBehaviourUi {
		[SerializeField] protected Vector2 _first;
		[SerializeField] protected Vector2 _second;
		[SerializeField] protected float   _speed = 1;

		private void Update() => transform.localPosition = Vector2.Lerp(_first, _second, (Mathf.Sin(Time.time * _speed) + 1) / 2);
	}
}