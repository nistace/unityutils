namespace NiUtils.StaticUtils {
	public static class OutUtils {
		public static bool True<TE>(this TE e, out TE variable) {
			variable = e;
			return true;
		}

		public static bool False<TE>(this TE e, out TE variable) {
			variable = e;
			return false;
		}
	}
}