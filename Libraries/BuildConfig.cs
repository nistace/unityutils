using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Extensions;

namespace Utils.Libraries {
	public static class BuildConfig {
		private static IReadOnlyDictionary<string, string> map { get; set; }

		public static void Load() {
			var buildConfigTextAsset = Resources.Load<TextAsset>("buildConfig");

			if (!buildConfigTextAsset) {
				Debug.Log("No buildConfig.txt in resources. This file must be created.");
				return;
			}

			map = buildConfigTextAsset.Lines().Select(t => (trimmed: t.Trim(), indexOfEquals: t.Trim().IndexOf("=", StringComparison.Ordinal)))
				.Where(t => !string.IsNullOrEmpty(t.trimmed) && !t.trimmed.StartsWith("//") && t.indexOfEquals > 0)
				.ToDictionary(t => t.trimmed.Substring(0, t.indexOfEquals).Trim(), t => t.trimmed.Substring(t.indexOfEquals + 1).Trim());
		}

		public static string Get(string key, string defaultValue = default) {
			if (map == null) return defaultValue;
			if (!map.ContainsKey(key)) return defaultValue;
			return map[key];
		}
	}
}