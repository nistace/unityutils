using Utils.Extensions;

namespace Utils.StaticUtils {
	public static class MathUtils {
		public static float ToUnsignedDegAngle(float degAngle) => degAngle.PosMod(360);

		public static float ToSignedDegAngle(float degAngle) {
			var unsignedAngle = ToUnsignedDegAngle(degAngle);
			return unsignedAngle > 180 ? unsignedAngle - 360 : unsignedAngle;
		}
	}
}