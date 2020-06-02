using UnityEngine;

public static class RectTransformExtension {
	public static void MoveAnchorsKeepPosition(this RectTransform transform, Vector2 anchorMin, Vector2 anchorMax) {
		var parentDimensions = transform.parent.TryGetComponent<RectTransform>(out var parent) ? parent.rect.size : new Vector2(Screen.width, Screen.height);
		transform.offsetMin += (transform.anchorMin - anchorMin) * parentDimensions;
		transform.offsetMax += (transform.anchorMax - anchorMax) * parentDimensions;
		transform.anchorMin = anchorMin;
		transform.anchorMax = anchorMax;
	}
}