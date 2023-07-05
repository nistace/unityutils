using System;
using UnityEngine;

namespace NiUtils.Ui {
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

		public static bool operator ==(RectTransformPosition first, RectTransformPosition second) {
			if (ReferenceEquals(first, null)) return ReferenceEquals(second, null);
			return first.Equals(second);
		}

		public static bool operator !=(RectTransformPosition first, RectTransformPosition second) => !(first == second);

		private bool Equals(RectTransformPosition other) =>
			_anchorMin.Equals(other._anchorMin) && _anchorMax.Equals(other._anchorMax) && _offsetMin.Equals(other._offsetMin) && _offsetMax.Equals(other._offsetMax);

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((RectTransformPosition)obj);
		}

		public override int GetHashCode() {
			unchecked {
				var hashCode = _anchorMin.GetHashCode();
				hashCode = (hashCode << 8) ^ _anchorMax.GetHashCode();
				hashCode = (hashCode << 8) ^ _offsetMin.GetHashCode();
				hashCode = (hashCode << 8) ^ _offsetMax.GetHashCode();
				return hashCode;
			}
		}
	}
}