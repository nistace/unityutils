using UnityEngine;

public class Follow : MonoBehaviour {
	[SerializeField] protected Transform _target;
	[SerializeField] protected Vector3   _offset;

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
		transform.position = _target.position + _offset;
	}
}