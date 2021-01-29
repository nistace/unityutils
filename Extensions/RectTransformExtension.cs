using UnityEngine;

namespace Utils.Extensions {
	public static class RectTransformExtension {
		public static void MoveAnchorsKeepPosition(this RectTransform transform, Vector2 anchorMin, Vector2 anchorMax) {
			var parentDimensions = transform.parent.TryGetComponent<RectTransform>(out var parent) ? parent.rect.size : new Vector2(Screen.width, Screen.height);
			transform.offsetMin += (transform.anchorMin - anchorMin) * parentDimensions;
			transform.offsetMax += (transform.anchorMax - anchorMax) * parentDimensions;
			transform.anchorMin = anchorMin;
			transform.anchorMax = anchorMax;
		}

		public static void MoveOverWorldTransform(this RectTransform uiTransform, Transform worldTarget, Vector3? targetOffset = null, Vector2? uiOffset = null) =>
			uiTransform.MoveOverWorldPosition(worldTarget.position, targetOffset, uiOffset);

		public static void MoveOverWorldPosition(this RectTransform uiTransform, Vector3 worldPosition, Vector3? targetOffset = null, Vector2? uiOffset = null) {
			var uiPosition = CameraUtils.main.WorldToScreenPoint(worldPosition + (targetOffset ?? Vector3.zero));
			if (uiOffset != null) uiPosition += new Vector3(uiOffset.Value.x, uiOffset.Value.y, 0);
			uiTransform.position = uiPosition;
		}

		public static void SetOffsets(this RectTransform uiTransform, Vector2 offsetMin, Vector2 offsetMax) {
			uiTransform.offsetMin = offsetMin;
			uiTransform.offsetMax = offsetMax;
		}

		public static void LerpToOffsets(this RectTransform uiTransform, Vector2 offsetMin, Vector2 offsetMax, float lerp) {
			uiTransform.offsetMin = Vector2.Lerp(uiTransform.offsetMin, offsetMin, lerp);
			uiTransform.offsetMax = Vector2.Lerp(uiTransform.offsetMax, offsetMax, lerp);
		}

		public static RectTransform Positioned(this RectTransform transform, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax) {
			transform.anchorMin = anchorMin;
			transform.anchorMax = anchorMax;
			transform.offsetMin = offsetMin;
			transform.offsetMax = offsetMax;
			return transform;
		}
	}
}