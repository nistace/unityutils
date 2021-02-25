using System;
using System.Linq;
using UnityEngine;
using Utils.Extensions;

namespace Utils.Behaviours {
	public class Ragdoll : MonoBehaviour {
		[SerializeField] protected string        _defaultForcedLimb;
		[SerializeField] protected bool          _hitRandomLimbWhenNotFound = true;
		[SerializeField] protected bool          _impactColliders           = true;
		[SerializeField] protected RagdollLimb[] _limbs;

		public new bool enabled {
			get => base.enabled;
			set {
				base.enabled = value;
				_limbs.ForEach(t => t.SetPhysicsEnabled(value, _impactColliders));
			}
		}

		public void ResetPhysics() => _limbs.ForEach(t => t.ResetPhysics());

		public void AddForce(string hitLimbName, Vector3 force, ForceMode forceMode) {
			if (_limbs.Length == 0) return;
			if (force == Vector3.zero) return;
			RagdollLimb limbToHit = null;
			if (!string.IsNullOrEmpty(hitLimbName) && _limbs.TryFirst(t => t.canReceiveForce && t.name == hitLimbName, out var preciseLimb)) limbToHit = preciseLimb;
			else if (!string.IsNullOrEmpty(_defaultForcedLimb) && _limbs.TryFirst(t => t.canReceiveForce && t.name == _defaultForcedLimb, out var defaultLimb)) limbToHit = defaultLimb;
			else if (_hitRandomLimbWhenNotFound) limbToHit = _limbs.Random(t => t.canReceiveForce ? 1 : 0);
			limbToHit?.AddForce(force, forceMode);
		}

		[Obsolete] public void SetEnabled(bool enabled) => _limbs.ForEach(t => t.SetPhysicsEnabled(enabled, _impactColliders));

		[Serializable]
		public class RagdollLimb {
			[SerializeField] protected string    _name;
			[SerializeField] protected Rigidbody _rigidbody;
			[SerializeField] protected Collider  _collider;
			[SerializeField] protected Transform _transform;
			[SerializeField] protected Vector3   _initialLocalPosition;
			[SerializeField] protected bool      _canReceiveForce = true;

			public string name            => _name;
			public bool   canReceiveForce => _canReceiveForce;

			public void ResetPhysics() {
				_rigidbody.velocity = Vector3.zero;
				_rigidbody.angularVelocity = Vector3.zero;
				_transform.localPosition = _initialLocalPosition;
			}

			public RagdollLimb() { }

			public RagdollLimb(Rigidbody rigidbody) {
				_rigidbody = rigidbody;
				_collider = _rigidbody.GetComponent<Collider>();
				_name = _rigidbody.name;
				_transform = _rigidbody.transform;
				_initialLocalPosition = _transform.localPosition;
			}

			public void SetPhysicsEnabled(bool enabled, bool impactColliders) {
				_rigidbody.isKinematic = !enabled;
				if (impactColliders) _collider.enabled = enabled;
			}

			public void AddForce(Vector3 force, ForceMode forceMode) => _rigidbody.AddForce(force, forceMode);
		}

#if UNITY_EDITOR
		[ContextMenu("Load limbs")]
		private void LoadRigidBodies() {
			_limbs = transform.Children().SelectMany(t => t.GetComponentsInChildren<Rigidbody>()).Select(t => new RagdollLimb(t)).ToArray();
		}

		[ContextMenu("Enable")] private void Enable() => enabled = true;
		[ContextMenu("Disable")] private void Disable() => enabled = false;
#endif
	}
}