using System;
using System.Collections.Generic;
using NiUtils.StaticUtils;

namespace NiUtils.Libraries {
	public static class Constants {
		private static ConstantLibrary           library          { get; set; }
		public static  bool                      loaded           => library != null;
		private static Dictionary<string, int>   parsedToInts     { get; } = new Dictionary<string, int>();
		private static Dictionary<string, float> parsedToFloats   { get; } = new Dictionary<string, float>();
		private static Dictionary<string, bool>  parsedToBooleans { get; } = new Dictionary<string, bool>();

		public static void LoadLibrary(ConstantLibrary libraryToLoad) {
			library = libraryToLoad;
			if (library) library.Load();
		}

		public static int Int(string name, int defaultValue = 0) => Get(name, parsedToInts, t => Parse.TryInt(t, out var value) ? value : defaultValue);
		public static float Float(string name, float defaultValue = 0) => Get(name, parsedToFloats, t => Parse.TryFloat(t, out var value) ? value : defaultValue);
		public static bool Bool(string name, bool defaultValue = false) => Get(name, parsedToBooleans, t => Parse.TryBool(t, out var value) ? value : defaultValue);
		public static string String(string name) => library?[name] ?? string.Empty;

		private static E Get<E>(string key, IDictionary<string, E> parsedValues, Func<string, E> parseFunc) {
			if (!parsedValues.ContainsKey(key)) parsedValues.Add(key, parseFunc(String(key)));
			return parsedValues[key];
		}
	}
}