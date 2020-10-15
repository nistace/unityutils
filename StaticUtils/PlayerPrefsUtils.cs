using System;
using UnityEngine;

public static class PlayerPrefsUtils {
	public static Color GetColor(string key, Color defaultValue = default) => new Color(PlayerPrefs.GetFloat($"{key}__r", defaultValue.r), PlayerPrefs.GetFloat($"{key}__g", defaultValue.g),
		PlayerPrefs.GetFloat($"{key}__b", defaultValue.b), PlayerPrefs.GetFloat($"{key}__a", defaultValue.a));

	public static void SetColor(string key, Color value) {
		PlayerPrefs.SetFloat($"{key}__r", value.r);
		PlayerPrefs.SetFloat($"{key}__g", value.g);
		PlayerPrefs.SetFloat($"{key}__b", value.b);
		PlayerPrefs.SetFloat($"{key}__a", value.a);
	}

	public static int GetOrSave(string key, int newValue) => GetOrSave(key, newValue, PlayerPrefs.GetInt, PlayerPrefs.SetInt);
	public static string GetOrSave(string key, string newValue) => GetOrSave(key, newValue, PlayerPrefs.GetString, PlayerPrefs.SetString);
	public static float GetOrSave(string key, float newValue) => GetOrSave(key, newValue, PlayerPrefs.GetFloat, PlayerPrefs.SetFloat);

	private static E GetOrSave<E>(string key, E newValue, Func<string, E, E> getFunc, Action<string, E> setFunc) {
		var value = getFunc(key, newValue);
		if (Equals(value, newValue)) setFunc(key, newValue);
		return value;
	}
}