using System;
using UnityEngine;

public static class VectorExtension {
	public static Vector2 Inside(this Vector3 v3, Rect rect) => ((Vector2) v3).Inside(rect);

	public static Vector2 Inside(this Vector2 v2, Rect rect) => new Vector2(v2.x.Clamp(rect.xMin, rect.xMax), v2.y.Clamp(rect.yMin, rect.yMax));
	public static Vector2 With(this Vector2 v2, float? x = null, float? y = null) => new Vector2(x ?? v2.x, y ?? v2.y);

	public static Vector3 With(this Vector3 v3, float? x = null, float? y = null, float? z = null) => new Vector3(x ?? v3.x, y ?? v3.y, z ?? v3.z);

	public static Vector3 With(this Vector3 v3, Func<float, float> funcX = null, Func<float, float> funcY = null, Func<float, float> funcZ = null) =>
		new Vector3((funcX ?? (t => t))(v3.x), (funcY ?? (t => t))(v3.y), (funcZ ?? (t => t))(v3.z));

	public static Vector3 Rotate(this Vector3 v3, float? aroundXAxis = null, float? aroundYAxis = null, float? aroundZAxis = null) =>
		Quaternion.Euler(aroundXAxis ?? 0, aroundYAxis ?? 0, aroundZAxis ?? 0) * v3;

	public static Vector3 Rotate(this Vector3 v3, Vector3 aroundAllAxes) => v3.Rotate(aroundAllAxes.x, aroundAllAxes.y, aroundAllAxes.z);

	public static Vector2 Rotate(this Vector2 v2, float amplitude) => new Vector3(v2.x, 0, v2.y).Rotate(aroundYAxis: amplitude).OnGround();

	public static Vector3 Lerp(this Vector3[] v3Path, float lerp) {
		if (v3Path.Length == 0) throw new ArgumentException("Length of path is 0. Path should have at least one position");
		if (v3Path.Length < 2) return v3Path[0];
		if (lerp <= 0) return v3Path[0];
		if (lerp >= 1) return v3Path.Last();
		var lerpPerNode = 1f / (v3Path.Length - 1);
		var nodeIndex = (lerp / lerpPerNode).Floor();
		return Vector3.Lerp(v3Path[nodeIndex], v3Path[nodeIndex + 1], (lerp % lerpPerNode) / lerpPerNode);
	}

	public static Vector2 OnGround(this Vector3 v3) => new Vector2(v3.x, v3.z);

	public static Vector3 FromGround(this Vector2 v2, float? height = null) => new Vector3(v2.x, height ?? 0, v2.y);

	public static Vector2 Clamp(this Vector2 v2, Vector2 min, Vector2 max) => new Vector2(Mathf.Clamp(v2.x, min.x, max.x), Mathf.Clamp(v2.y, min.y, max.y));
}