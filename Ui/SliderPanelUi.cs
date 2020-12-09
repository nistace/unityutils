﻿using System.Collections;
using UnityEngine;
using Utils.Coroutines;
using Utils.Types.Ui;

namespace Utils.Ui {
	public class SliderPanelUi : MonoBehaviourUi {
		public static int   defaultCloseIndex => 0;
		public static int   defaultOpenIndex  => 1;
		public static float defaultMoveTime   { get; set; }

		[SerializeField] protected RectTransformPosition[] _positions;

		private SingleCoroutine singleCoroutine { get; set; }

		private void Awake() {
			singleCoroutine = new SingleCoroutine(this);
		}

		public void MoveToDefaultOpen(float? time = null) => MoveTo(defaultOpenIndex, time);
		public void MoveToDefaultClose(float? time = null) => MoveTo(defaultCloseIndex, time);

		public void MoveTo(int state, float? time = null) {
			if (state >= _positions.Length) {
				Debug.Log("State " + state + " does not exist");
				return;
			}
			if (singleCoroutine == null || !gameObject.activeInHierarchy || (time ?? defaultMoveTime) <= 0) JumpTo(state);
			else singleCoroutine.Start(DoMoveTo(_positions[state], time ?? defaultMoveTime));
		}

		public void JumpTo(int state) {
			var position = _positions[state];
			transform.MoveAnchorsKeepPosition(position.anchorMin, position.anchorMax);
			transform.SetOffsets(_positions[state].offsetMin, _positions[state].offsetMax);
		}

		private IEnumerator DoMoveTo(RectTransformPosition position, float time) {
			transform.MoveAnchorsKeepPosition(position.anchorMin, position.anchorMax);
			if (time > 0) {
				var timeCoefficient = 1 / time;
				for (var timeProgress = 0f; timeProgress < 1; timeProgress += timeCoefficient * Time.deltaTime) {
					transform.LerpToOffsets(position.offsetMin, position.offsetMax, timeProgress);
					yield return null;
				}
			}
			transform.SetOffsets(position.offsetMin, position.offsetMax);
		}

#if UNITY_EDITOR
		private void SaveCurrent(int index) {
			if (_positions == null) _positions = new RectTransformPosition[] { };
			while (_positions.Length <= index) _positions = _positions.WithAppended(new RectTransformPosition());

			_positions[index].anchorMin = transform.anchorMin;
			_positions[index].anchorMax = transform.anchorMax;
			_positions[index].offsetMin = transform.offsetMin;
			_positions[index].offsetMax = transform.offsetMax;
		}

		[ContextMenu("Save current at 0 - Close")] private void SaveCurrent0() => SaveCurrent(0);
		[ContextMenu("Save current at 1 - Open")] private void SaveCurrent1() => SaveCurrent(1);
		[ContextMenu("Save current at 2")] private void SaveCurrent2() => SaveCurrent(2);
		[ContextMenu("Save current at 3")] private void SaveCurrent3() => SaveCurrent(3);
		[ContextMenu("Save current at 4")] private void SaveCurrent4() => SaveCurrent(4);
		[ContextMenu("Jump to 0 - Close")] private void JumpTo0() => JumpTo(0);
		[ContextMenu("Jump to 1 - Open")] private void JumpTo1() => JumpTo(1);
		[ContextMenu("Jump to 2")] private void JumpTo2() => JumpTo(2);
		[ContextMenu("Jump to 3")] private void JumpTo3() => JumpTo(3);
		[ContextMenu("Jump to 4")] private void JumpTo4() => JumpTo(4);
#endif
	}
}