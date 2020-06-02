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
}