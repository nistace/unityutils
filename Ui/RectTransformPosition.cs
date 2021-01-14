using System;
using UnityEngine;

namespace Utils.Types.Ui {
	[Serializable]
	public class RectTransformPosition {
		[SerializeField] protected Vector2 _anchorMin;
		[SerializeField] protected Vector2 _anchorMax;
		[SerializeField] protected Vector2 _offsetMin;
		[SerializeField] protected Vector2 _offsetMax;

		public Vector2 anchorMin => _anchorMin;
		public Vector2 anchorMax => _anchorMax;
		public Vector2 offsetMin => _offsetMin;
		public Vector2 offsetMax => _offsetMax;

		public RectTransformPosition() { }

		public RectTransformPosition(RectTransform transform) : this(transform.anchorMin, transform.anchorMax, transform.offsetMin, transform.offsetMax) { }

		public RectTransformPosition(Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax) {
			_anchorMin = anchorMin;
			_anchorMax = anchorMax;
			_offsetMin = offsetMin;
			_offsetMax = offsetMax;
		}

		public RectTransformPosition WithLockedAxes(RectTransform transform, bool horizontallyLocked, bool verticallyLocked) {
			if (!horizontallyLocked && !verticallyLocked) return this;
			return new RectTransformPosition {
				_anchorMin = GetLockedPosition(anchorMin, transform.anchorMin, horizontallyLocked, verticallyLocked),
				_anchorMax = GetLockedPosition(anchorMax, transform.anchorMax, horizontallyLocked, verticallyLocked),
				_offsetMin = GetLockedPosition(offsetMin, transform.offsetMin, horizontallyLocked, verticallyLocked),
				_offsetMax = GetLockedPosition(offsetMax, transform.offsetMax, horizontallyLocked, verticallyLocked)
			};
		}

		private static Vector2 GetLockedPosition(Vector2 unlockedPosition, Vector2 currentPosition, bool horizontallyLocked, bool verticallyLocked) {
			if (horizontallyLocked && verticallyLocked) return currentPosition;
			if (!horizontallyLocked && !verticallyLocked) return unlockedPosition;
			return new Vector2(horizontallyLocked ? currentPosition.x : unlockedPosition.x, verticallyLocked ? currentPosition.y : unlockedPosition.y);
		}
	}
}