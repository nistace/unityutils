using System;
using UnityEngine;

namespace Utils.Types.Ui {
	[Serializable]
	public class RectTransformPosition {
		[SerializeField] protected Vector2 _anchorMin;
		[SerializeField] protected Vector2 _anchorMax;
		[SerializeField] protected Vector2 _offsetMin;
		[SerializeField] protected Vector2 _offsetMax;

		public Vector2 anchorMin {
			get => _anchorMin;
			set => _anchorMin = value;
		}

		public Vector2 anchorMax {
			get => _anchorMax;
			set => _anchorMax = value;
		}

		public Vector2 offsetMin {
			get => _offsetMin;
			set => _offsetMin = value;
		}

		public Vector2 offsetMax {
			get => _offsetMax;
			set => _offsetMax = value;
		}

		public RectTransformPosition() { }

		public RectTransformPosition(RectTransform transform) {
			anchorMin = transform.anchorMin;
			anchorMax = transform.anchorMax;
			offsetMin = transform.offsetMin;
			offsetMax = transform.offsetMax;
		}

		public RectTransformPosition WithLockedAxes(RectTransform transform, bool horizontallyLocked, bool verticallyLocked) {
			if (!horizontallyLocked && !verticallyLocked) return this;
			return new RectTransformPosition {
				anchorMin = GetLockedPosition(anchorMin, transform.anchorMin, horizontallyLocked, verticallyLocked),
				anchorMax = GetLockedPosition(anchorMax, transform.anchorMax, horizontallyLocked, verticallyLocked),
				offsetMin = GetLockedPosition(offsetMin, transform.offsetMin, horizontallyLocked, verticallyLocked),
				offsetMax = GetLockedPosition(offsetMax, transform.offsetMax, horizontallyLocked, verticallyLocked)
			};
		}

		private static Vector2 GetLockedPosition(Vector2 unlockedPosition, Vector2 currentPosition, bool horizontallyLocked, bool verticallyLocked) {
			if (horizontallyLocked && verticallyLocked) return currentPosition;
			if (!horizontallyLocked && !verticallyLocked) return unlockedPosition;
			return new Vector2(horizontallyLocked ? currentPosition.x : unlockedPosition.x, verticallyLocked ? currentPosition.y : unlockedPosition.y);
		}
	}
}