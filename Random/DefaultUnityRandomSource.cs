namespace Utils.RandomUtils {
	public class DefaultUnityRandomSource : IRandomSource {
		public static DefaultUnityRandomSource instance { get; } = new DefaultUnityRandomSource();

		public float value => UnityEngine.Random.value;
		public int Range(int min, int max) => UnityEngine.Random.Range(min, max);

		public float Range(float min, float max) => UnityEngine.Random.Range(min, max);
		public string ToDebugString() => "Unity random";
	}
}