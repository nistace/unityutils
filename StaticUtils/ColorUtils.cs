using UnityEngine;
using UnityEngine.Events;

namespace NiUtils.StaticUtils {
	public static class ColorUtils {
		public class Event : UnityEvent<Color> { }

		public static Color Random(float? r = null, float? g = null, float? b = null, float? a = null) =>
			new Color(r ?? UnityEngine.Random.value, g ?? UnityEngine.Random.value, b ?? UnityEngine.Random.value, a ?? UnityEngine.Random.value);
	}
}