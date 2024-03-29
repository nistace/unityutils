﻿using UnityEngine;

namespace NiUtils.Behaviours {
	public class Rotator : MonoBehaviour {
		[SerializeField] protected Vector3 _eulerPerSecond;

		private void Update() {
			transform.Rotate(_eulerPerSecond * Time.deltaTime);
		}
	}
}