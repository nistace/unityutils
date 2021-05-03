using System.Collections;
using UnityEngine;
using Utils.Coroutines;
using Utils.Extensions;
using Utils.Types.Ui;

namespace Utils.Ui {
	public class PanelTransformUi : MonoBehaviourUi {
		public static float defaultMoveTime { get; set; } = 1;

		[SerializeField] protected RectTransformPosition _openPosition;
		[SerializeField] protected RectTransformPosition _closePosition;
		[SerializeField] protected bool                  _lockHorizontal;
		[SerializeField] protected bool                  _lockVertical;

		public  RectTransformPosition openPosition    => _openPosition;
		public  RectTransformPosition closePosition   => _closePosition;
		private SingleCoroutine       singleCoroutine { get; set; }
		public  RectTransformPosition position        => new RectTransformPosition(transform);

		private void Awake() {
			singleCoroutine = new SingleCoroutine(this);
		}

		public void Open(float? time = null) => MoveTo(_openPosition, time);
		public void Close(float? time = null) => MoveTo(_closePosition, time);

		public void MoveTo(RectTransformPosition position, float? time = null) {
			if (!Application.isPlaying || singleCoroutine == null || !gameObject.activeInHierarchy || (time ?? defaultMoveTime) <= 0) JumpTo(position);
			else singleCoroutine.Start(DoMoveTo(position.WithLockedAxes(transform, _lockHorizontal, _lockVertical), time ?? defaultMoveTime));
		}

		public void JumpTo(RectTransformPosition position) {
			var lockedPosition = position.WithLockedAxes(transform, _lockHorizontal, _lockVertical);
			transform.MoveAnchorsKeepPosition(lockedPosition.anchorMin, lockedPosition.anchorMax);
			transform.SetOffsets(lockedPosition.offsetMin, lockedPosition.offsetMax);
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
	}
}