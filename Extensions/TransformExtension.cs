using MSG;
using UnityEngine;

public static class TransformExtension {
	public static void ClampPosition(this Transform transform, Vector3 minPosition, Vector3 maxPosition) {
		var currentPosition = transform.position;
		if (currentPosition.x >= minPosition.x && currentPosition.x <= maxPosition.x && currentPosition.y >= minPosition.y && currentPosition.y <= maxPosition.y && currentPosition.z >= minPosition.z &&
			currentPosition.z <= maxPosition.z) return;
		transform.position = new Vector3(currentPosition.x.Clamp(minPosition.x, maxPosition.x), currentPosition.y.Clamp(minPosition.y, maxPosition.y),
			currentPosition.z.Clamp(minPosition.z, maxPosition.z));
	}

	public static void MoveUiOverWorldTransform(this Transform uiTransform, Transform worldTarget) => uiTransform.position = CameraMonoBehaviour.mainCamera.WorldToScreenPoint(worldTarget.position);
}