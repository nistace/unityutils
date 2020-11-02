using ExitGames.Client.Photon;
using UnityEngine;

namespace Utils.Extensions {
	public static class PunHashtableExtension {
		public static int Int(this Hashtable table, string name, int defaultValue = default) => table.Get(name, defaultValue);
		public static byte Byte(this Hashtable table, string name, byte defaultValue = default) => table.Get(name, defaultValue);
		public static string String(this Hashtable table, string name, string defaultValue = default) => table.Get(name, defaultValue);
		public static float Float(this Hashtable table, string name, float defaultValue = default) => table.Get(name, defaultValue);
		public static bool Bool(this Hashtable table, string name, bool defaultValue = default) => table.Get(name, defaultValue);
		public static Color Color(this Hashtable table, string key) => new Color(table.Float($"{key}.r"), table.Float($"{key}.g"), table.Float($"{key}.b"), table.Float($"{key}.a"));
		public static Vector3 Vector3(this Hashtable table, string key) => new Vector3(table.Float($"{key}.x"), table.Float($"{key}.y"), table.Float($"{key}.z"));
		public static Vector2 Vector2(this Hashtable table, string key) => new Vector2(table.Float($"{key}.x"), table.Float($"{key}.y"));

		public static bool TryGet<E>(this Hashtable table, string name, out E value) {
			if (table.ContainsKey(name) && table[name] is E castValue) {
				value = castValue;
				return true;
			}
			value = default;
			return false;
		}

		private static E Get<E>(this Hashtable table, string name, E defaultValue = default) => table.TryGet<E>(name, out var castValue) ? castValue : defaultValue;

		public static void Define(this Hashtable table, string key, Color color) {
			table[$"{key}.r"] = color.r;
			table[$"{key}.g"] = color.g;
			table[$"{key}.b"] = color.b;
			table[$"{key}.a"] = color.a;
		}

		public static void Define(this Hashtable table, string key, Vector3 v3) {
			table[$"{key}.x"] = v3.x;
			table[$"{key}.y"] = v3.y;
			table[$"{key}.z"] = v3.z;
		}

		public static void Define(this Hashtable table, string key, Vector2 v2) {
			table[$"{key}.x"] = v2.x;
			table[$"{key}.y"] = v2.y;
		}

		public static void Define(this Hashtable table, string key, int i) => table[key] = i;
		public static void Define(this Hashtable table, string key, float f) => table[key] = f;
		public static void Define(this Hashtable table, string key, string s) => table[key] = s;
		public static void Define(this Hashtable table, string key, bool b) => table[key] = b;
		public static void Define(this Hashtable table, string key, byte b) => table[key] = b;
	}
}