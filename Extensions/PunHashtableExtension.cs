using ExitGames.Client.Photon;
using UnityEngine;

namespace Utils.Extensions {
	public static class PunHashtableExtension {
		public static int Int(this Hashtable table, string name, int defaultValue = default) => table.Get(name, defaultValue);
		public static byte Byte(this Hashtable table, string name, byte defaultValue = default) => table.Get(name, defaultValue);
		public static string String(this Hashtable table, string name, string defaultValue = default) => table.Get(name, defaultValue);
		public static float Float(this Hashtable table, string name, float defaultValue = default) => table.Get(name, defaultValue);
		public static bool Bool(this Hashtable table, string name, bool defaultValue = default) => table.Get(name, defaultValue);

		public static bool TryGet<E>(this Hashtable table, string name, out E value) {
			if (table.ContainsKey(name) && table[name] is E castValue) {
				value = castValue;
				return true;
			}
			value = default;
			return false;
		}

		public static E Get<E>(this Hashtable table, string name, E defaultValue = default) => table.TryGet<E>(name, out var castValue) ? castValue : defaultValue;

		public static void PunSet(this Hashtable table, string key, Color color) {
			table[$"{key}.r"] = color.r;
			table[$"{key}.g"] = color.g;
			table[$"{key}.b"] = color.b;
			table[$"{key}.a"] = color.a;
		}

		public static Color GetColor(this Hashtable table, string key) => new Color(table.Float($"{key}.r"), table.Float($"{key}.g"), table.Float($"{key}.b"), table.Float($"{key}.a"));
	}
}