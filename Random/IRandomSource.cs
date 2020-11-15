namespace Utils.RandomUtils {
	public interface IRandomSource {
		float value { get; }

		/// <returns>A random value between min (included) and max (excluded)</returns>
		int Range(int min, int max);

		/// <returns>A random value between min (included) and max (included)</returns>
		float Range(float min, float max);

		string ToDebugString();
	}
}