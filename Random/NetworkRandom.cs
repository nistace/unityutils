using Photon.Pun;
using UnityEngine;

namespace Utils.RandomUtils {
	public static class NetworkRandom {
		private static int[] values         { get; } = new int[12];
		private static int   nextValueIndex { get; set; }
		private static int   previousValue  { get; set; }
		private static int   step           { get; set; }

		public static void Set(int seed, int startingStep = 0) {
			for (var i = 0; i < 12; ++i) values[i] = ((seed << (i + 1)) + (int) Mathf.Pow(i + 2, 2)).PosMod(1000);
			previousValue = seed.PosMod(1000);
			nextValueIndex = 0;
			for (var i = 0; i < startingStep; ++i) GetNextValue();
		}

		public static float GetNextValue() {
			if (PhotonNetwork.OfflineMode) return Random.value;
			step++;
			previousValue += values[nextValueIndex];
			previousValue %= 1000;
			nextValueIndex = (nextValueIndex + 1) % values.Length;
			return previousValue / 1000f;
		}

		public static int Range(int min, int max) => (min + (int) ((max - min) * GetNextValue())).Clamp(min, max - 1);
		public static float Range(float min, float max) => (min + (max - min) * GetNextValue()).Clamp(min, max);
		public static string ToDebugString() => $"Network random, previous value ({previousValue}), step ({step})";
	}
}