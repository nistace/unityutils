using System;
using UnityEngine;

namespace NiUtils.Hacking {
	public class SpeedHackDetector : MonoBehaviour {
		[SerializeField] protected int   _timeDiff;
		[SerializeField] protected int   _previousTime;
		[SerializeField] protected int   _realTime;
		[SerializeField] protected float _gameTime;
		[SerializeField] protected bool  _detected;
		[SerializeField] protected int   _threshold;

		public static void Instantiate(int threshold = 7) => new GameObject("SpeedHackDetector").AddComponent<SpeedHackDetector>()._threshold = threshold;

		private void Start() {
			_previousTime = DateTime.Now.Second;
			_gameTime = 0;
		}

		private void Update() {
			_gameTime += Time.deltaTime;
			if (_previousTime == DateTime.Now.Second) return;
			_realTime++;
			_previousTime = DateTime.Now.Second;
			_timeDiff = (int) _gameTime - _realTime;
			if (_timeDiff > _threshold) {
				if (_detected) return;
				_detected = true;
				Hacks.onHackDetected.Invoke();
			}
			else _detected = false;
		}
	}
}