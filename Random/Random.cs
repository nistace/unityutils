namespace Utils.RandomUtils {
	public static class Random {
		private static IRandomSource source { get; set; } = DefaultUnityRandomSource.instance;

		public static void SetSource(IRandomSource source) => Random.source = source;

		public static float value => source.value;

		public static int Range(int min, int max) => source.Range(min, max);
		public static float Range(float min, float max) => source.Range(min, max);

		public static string ToDebugString() => source.ToDebugString();
	}
}