using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using Utils.Extensions;

namespace Utils.Libraries {
	public static class Localisation {
		public class LanguageChangedEvent : UnityEvent { }

		private static Regex                            pluralRegex       { get; } = new Regex("\\[PL(\\d+):([^\\]]+)\\]");
		private static Regex                            singularRegex     { get; } = new Regex("\\[SG(\\d+):([^\\]]+)\\]");
		private static Dictionary<string, string>       messages          { get; } = new Dictionary<string, string>();
		private static Dictionary<string, int>          multipleItemCount { get; } = new Dictionary<string, int>();
		private static Dictionary<string, HashSet<int>> uniquenessCheck   { get; } = new Dictionary<string, HashSet<int>>();
		public static  Language                         language          { get; private set; }
		public static  bool                             loaded            => language != null;
		public static  bool                             debugAllKeys      { get; set; }

		public static LanguageChangedEvent onLanguageChanged { get; } = new LanguageChangedEvent();

		public static void SetLanguage(Language language) {
			if (Localisation.language == language) return;
			Localisation.language = language;
			Reload();
		}

		private static void Load(TextAsset textAsset) {
			foreach (var line in textAsset.Lines()) {
				var cleanLine = line.Trim();
				if (line.Contains("//")) cleanLine = cleanLine.Substring(0, line.IndexOf("//", StringComparison.Ordinal)).Trim();
				if (string.IsNullOrEmpty(cleanLine)) continue;
				if (line.Contains("=")) {
					messages.Set(cleanLine.Substring(0, cleanLine.IndexOf("=", StringComparison.Ordinal)).CleanKey(), cleanLine.Substring(cleanLine.IndexOf("=", StringComparison.Ordinal) + 1).Trim());
				}
			}
			onLanguageChanged.Invoke();
		}

		public static void Reload() {
			messages.Clear();
			Resources.LoadAll<Language>("Localisation").Where(t => t.alwaysLoad && t != language).ForEach(t => Load(t.textAsset));
			if (Application.isEditor) MarkAllAsNotLocalised(language.iso);
			Load(language.textAsset);
		}

		private static void MarkAllAsNotLocalised(string isoCode) {
			messages.Keys.ToArray().ForEach(t => messages.Set(t, $"[{isoCode}] {messages[t]}"));
		}

		public static bool TryMap(string key, out string result, params object[] parameters) {
			result = MapOrNull(key, parameters);
			return result != null;
		}

		private static string MapOrNull(string key, params object[] parameters) {
			if (string.IsNullOrEmpty(key)) return string.Empty;
			var cleanKey = key.CleanKey();
			if (!messages.ContainsKey(cleanKey)) {
				return null;
			}
			try {
				var message = string.Format(messages[cleanKey], parameters);
				message = pluralRegex.Replace(message, t => GetFloatValueReplacement(t, v => Mathf.Abs(v) > 1));
				message = singularRegex.Replace(message, t => GetFloatValueReplacement(t, v => Mathf.Abs(v) <= 1));
				return message;
			}
			catch (FormatException) {
				Debug.LogWarning($"Format exception while mapping [{cleanKey}] with {parameters.Length} parameters");
				return $"[{cleanKey}]";
			}

			string GetFloatValueReplacement(Match match, Func<float, bool> check) {
				if (!int.TryParse(match.Groups[1].Value, out var paramIndex))
					Debug.LogWarning($"Localisation {cleanKey} have an error parsing the regex. {match.Groups[1].Value} cannot be turned into an index.");
				else if (parameters.Length <= paramIndex)
					Debug.LogWarning($"Localisation {cleanKey} requires a parameter at index {paramIndex} but not enough parameters were given.");
				else if (!float.TryParse($"{parameters[paramIndex]}", out var paramAsFloat))
					Debug.LogWarning($"Localisation {cleanKey} requires a number parameter at {paramIndex} but this parameter could not be parsed to a number.");
				else if (check(paramAsFloat)) return match.Groups[2].Value;
				return string.Empty;
			}
		}

		public static string Map(string key, params object[] parameters) {
			if (debugAllKeys) Debug.Log($"MappingRequested : key {key}, parameters [{parameters.Join(", ")}]");
			var text = MapOrNull(key, parameters);
			if (string.IsNullOrEmpty(text) && Application.isPlaying) Debug.LogWarning($"No value in localisation file for key {key.CleanKey()}");
			return text ?? "[" + key.CleanKey() + "]";
		}

		public static int Count(string keyRoot) => CountMultipleItem(keyRoot.CleanKey());

		private static int CountMultipleItem(string cleanKeyRoot) {
			if (!multipleItemCount.ContainsKey(cleanKeyRoot)) {
				var count = 0;
				while (messages.ContainsKey(cleanKeyRoot + "." + count)) count++;
				multipleItemCount.Add(cleanKeyRoot, count);
			}
			return multipleItemCount[cleanKeyRoot];
		}

		/// <summary>Given keyRoot the root of the key we are looking for, returns a random item like "keyRoot.#" where # is a number</summary>
		internal static string MapRandom(string keyRoot, bool unique = false, params object[] parameters) {
			var cleanKey = keyRoot.CleanKey();
			if (CountMultipleItem(cleanKey) <= 0) return Map(cleanKey + ".0", parameters);
			return Map(cleanKey + "." + GetRandomIndex(cleanKey, unique), parameters);
		}

		public static string[] MapMultiple(string keyRoot, params object[] parameters) {
			return CountMultipleItem(keyRoot.CleanKey()).CreateArray(t => Map(keyRoot + "." + t, parameters));
		}

		public static void ReinitializeUniqueness() {
			uniquenessCheck.Clear();
		}

		private static int GetRandomIndex(string keyRoot, bool unique) {
			var random = UnityEngine.Random.Range(0, CountMultipleItem(keyRoot));
			if (!unique) return random;
			if (!uniquenessCheck.ContainsKey(keyRoot)) uniquenessCheck.Add(keyRoot, new HashSet<int>());
			if (uniquenessCheck[keyRoot].Count >= CountMultipleItem(keyRoot)) {
				Debug.LogWarning("No more unique names for key" + keyRoot);
				return random;
			}
			while (uniquenessCheck[keyRoot].Contains(random)) random = UnityEngine.Random.Range(0, CountMultipleItem(keyRoot));
			uniquenessCheck[keyRoot].Add(random);
			return random;
		}
	}
}