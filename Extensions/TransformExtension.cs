using UnityEngine;

public static class TransformExtension {
	public static void ClampPosition(this Transform transform, Vector3 minPosition, Vector3 maxPosition) {
		var currentPosition = transform.position;
		if (currentPosition.x >= minPosition.x && currentPosition.x <= maxPosition.x && currentPosition.y >= minPosition.y && currentPosition.y <= maxPosition.y && currentPosition.z >= minPosition.z &&
			currentPosition.z <= maxPosition.z) return;
		transform.position = new Vector3(currentPosition.x.Clamp(minPosition.x, maxPosition.x), currentPosition.y.Clamp(minPosition.y, maxPosition.y),
			currentPosition.z.Clamp(minPosition.z, maxPosition.z));
	}

	public static void MoveUiOverWorldTransform(this Transform uiTransform, Transform worldTarget) => uiTransform.position = CameraUtils.main.WorldToScreenPoint(worldTarget.position);

	public static void SetRotationWithEuler(this Transform t, float? x = null, float? y = null, float? z = null) => t.rotation = t.rotation.WithEuler(x, y, z);
	public static void SetPositionWith(this Transform t, float? x = null, float? y = null, float? z = null) => t.position = t.position.With(x, y, z);
}