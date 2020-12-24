using System;
using System.Linq;
using UnityEngine;
using Utils.Extensions;

namespace Utils.Behaviours {
	public class Ragdoll : MonoBehaviour {
		[SerializeField] protected RagdollLimb[] _limbs;

		public void ResetPhysics() => _limbs.ForEach(t => t.ResetPhysics());

		public void AddForce(string hitLimbName, Vector3 force, ForceMode forceMode) {
			if (_limbs.Length == 0) return;
			var hitRigidbody = !string.IsNullOrEmpty(hitLimbName) && _limbs.TryFirst(t => t.canReceiveForce && t.name == hitLimbName, out var limbRigidbody)
				? limbRigidbody
				: _limbs.Random(t => t.canReceiveForce ? 1 : 0);
			hitRigidbody.AddForce(force, forceMode);
		}

		public void SetEnabled(bool enabled) => _limbs.ForEach(t => t.SetPhysicsEnabled(enabled));

		[Serializable]
		public class RagdollLimb {
			[SerializeField] protected string     _name;
			[SerializeField] protected Rigidbody  _rigidbody;
			[SerializeField] protected Transform  _transform;
			[SerializeField] protected Vector3    _initialLocalPosition;
			[SerializeField] protected Quaternion _initialLocalRotation;
			[SerializeField] protected Vector3    _initialLocalScale;
			[SerializeField] protected bool       _canReceiveForce = true;

			public string name            => _name;
			public bool   canReceiveForce => _canReceiveForce;

			public void ResetPhysics() {
				_rigidbody.velocity = Vector3.zero;
				_rigidbody.angularVelocity = Vector3.zero;
				_transform.localPosition = _initialLocalPosition;
				//_transform.localRotation = _initialLocalRotation;
				//_transform.localScale = _initialLocalScale;
			}

			public RagdollLimb() { }

			public RagdollLimb(Rigidbody rigidbody) {
				_rigidbody = rigidbody;
				_name = _rigidbody.name;
				_transform = _rigidbody.transform;
				_initialLocalPosition = _transform.localPosition;
				_initialLocalRotation = _transform.localRotation;
				_initialLocalScale = _transform.localScale;
			}

			public void SetPhysicsEnabled(bool enabled) => _rigidbody.isKinematic = !enabled;

			public void AddForce(Vector3 force, ForceMode forceMode) => _rigidbody.AddForce(force, forceMode);
		}

#if UNITY_EDITOR
		[ContextMenu("Load limbs")]
		private void LoadRigidBodies() {
			_limbs = transform.Children().SelectMany(t => t.GetComponentsInChildren<Rigidbody>()).Select(t => new RagdollLimb(t)).ToArray();
		}
#endif
	}
}