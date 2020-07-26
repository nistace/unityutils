using UnityEngine;

namespace Utils {
	public static class PlayerPreferences {
		private const int trueIntValue  = 1;
		private const int falseIntValue = 0;

		public static void Set(string key, string value) => PlayerPrefs.SetString(key.CleanKey(), value);
		public static void Set(string key, float value) => PlayerPrefs.SetFloat(key.CleanKey(), value);
		public static void Set(string key, int value) => PlayerPrefs.SetFloat(key.CleanKey(), value);
		public static void Set(string key, bool value) => PlayerPrefs.SetInt(key.CleanKey(), value ? trueIntValue : falseIntValue);

		public static string String(string key, string defaultValue = default) => PlayerPrefs.GetString(key.CleanKey(), defaultValue);
		public static int Int(string key, int defaultValue = default) => PlayerPrefs.GetInt(key.CleanKey(), defaultValue);
		public static float Float(string key, float defaultValue = default) => PlayerPrefs.GetFloat(key.CleanKey(), defaultValue);
		public static bool Bool(string key, bool defaultValue = default) => PlayerPrefs.GetInt(key.CleanKey(), defaultValue ? trueIntValue : falseIntValue) == trueIntValue;

		public static void Save() => PlayerPrefs.Save();
	}
}