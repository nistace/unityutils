using UnityEngine;

public static class QuaternionExtension {
	public static Quaternion WithEuler(this Quaternion q, float? x = null, float? y = null, float? z = null) {
		var euler = q.eulerAngles;
		return Quaternion.Euler(x ?? euler.x, y ?? euler.y, z ?? euler.z);
	}
}