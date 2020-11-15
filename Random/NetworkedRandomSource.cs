using UnityEngine;

namespace Utils.RandomUtils {
	public class NetworkedRandomSource : IRandomSource {
		private int[] values         { get; }
		private int   nextValueIndex { get; set; }
		private int   previousValue  { get; set; }
		public  float value          => GetNextValue();
		private int   step           { get; set; }

		public NetworkedRandomSource(int seed, int startingStep = 0) {
			values = new int[12];
			for (var i = 0; i < 12; ++i) {
				values[i] = ((seed << (i + 1)) + (int) Mathf.Pow(i + 2, 2)).PosMod(1000);
			}
			previousValue = seed.PosMod(1000);
			nextValueIndex = 0;
			for (var i = 0; i < startingStep; ++i) GetNextValue();
		}

		private float GetNextValue() {
			step++;
			previousValue += values[nextValueIndex];
			previousValue %= 1000;
			nextValueIndex = (nextValueIndex + 1) % values.Length;
			return previousValue / 1000f;
		}

		public int Range(int min, int max) => (min + (int) ((max - min) * value)).Clamp(min, max - 1);
		public float Range(float min, float max) => (min + (max - min) * value).Clamp(min, max);
		public string ToDebugString() => $"Network random, previous value ({previousValue}), step ({step})";
	}
}