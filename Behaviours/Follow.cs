using UnityEngine;

public class Follow : MonoBehaviour {
	[SerializeField] protected Transform _target;
	[SerializeField] protected Vector3   _offset;
	[SerializeField] protected float     _smoothTime = 1;
	[SerializeField] protected Vector3   _velocity;

	public Transform target {
		get => _target;
		set => _target = value;
	}

	public Vector3 offset {
		get => _offset;
		set => _offset = value;
	}

	private void LateUpdate() {
		if (!_target) return;
		transform.position = Vector3.SmoothDamp(transform.position, _target.position + _offset, ref _velocity, _smoothTime);
	}
}