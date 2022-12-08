namespace Utils.StaticUtils {
	public static class OutUtils {
		public static bool True<TE>(this TE e, out TE variable) {
			variable = e;
			return true;
		}
	}
}