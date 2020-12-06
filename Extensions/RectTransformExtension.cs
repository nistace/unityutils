﻿using UnityEngine;

public static class RectTransformExtension {
	public static void MoveAnchorsKeepPosition(this RectTransform transform, Vector2 anchorMin, Vector2 anchorMax) {
		var parentDimensions = transform.parent.TryGetComponent<RectTransform>(out var parent) ? parent.rect.size : new Vector2(Screen.width, Screen.height);
		transform.offsetMin += (transform.anchorMin - anchorMin) * parentDimensions;
		transform.offsetMax += (transform.anchorMax - anchorMax) * parentDimensions;
		transform.anchorMin = anchorMin;
		transform.anchorMax = anchorMax;
	}

	public static void MoveOverWorldTransform(this RectTransform uiTransform, Transform worldTarget, Vector3? targetOffset = null, Vector2? uiOffset = null) {
		var position = worldTarget.position;
		if (targetOffset != null) position += targetOffset.Value;
		var uiPosition = CameraUtils.main.WorldToScreenPoint(position);
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
}