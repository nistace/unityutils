using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Extensions;

namespace Utils.Behaviours {
	public class RagdollSwitcher : MonoBehaviour {
		protected enum Status {
			Unchanged = 0,
			Ragdoll   = 1,
			Animated  = 2
		}

		[SerializeField] protected Status      _startStatus = Status.Animated;
		[SerializeField] protected Animator    _animator;
		[SerializeField] protected Rigidbody[] _ragdollRigidBodies;

		public IEnumerable<Rigidbody> ragdollRigidBodies => _ragdollRigidBodies;

		private void Start() {
			switch (_startStatus) {
				case Status.Ragdoll:
					SwitchToRagdoll();
					return;
				case Status.Animated:
					SwitchToAnimated();
					return;
				case Status.Unchanged:
				default: return;
			}
		}

		private void Reset() {
			_animator = GetComponentInChildren<Animator>();
			_ragdollRigidBodies = transform.Children().SelectMany(t => t.GetComponentsInChildren<Rigidbody>()).ToArray();
		}

		public void SwitchToRagdoll() {
			_animator.enabled = false;
			_ragdollRigidBodies.ForEach(t => t.isKinematic = false);
		}

		public void SwitchToAnimated() {
			_animator.enabled = true;
			_ragdollRigidBodies.ForEach(t => t.isKinematic = true);
		}
	}
}