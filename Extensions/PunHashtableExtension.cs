using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using UnityEngine;

namespace Utils.Extensions {
	public static class PunHashtableExtension {
		public static int Int(this Hashtable table, string name, int defaultValue = default) => table.Get(name, defaultValue);
		public static byte Byte(this Hashtable table, string name, byte defaultValue = default) => table.Get(name, defaultValue);
		public static string String(this Hashtable table, string name, string defaultValue = default) => table.Get(name, defaultValue);
		public static float Float(this Hashtable table, string name, float defaultValue = default) => table.Get(name, defaultValue);
		public static bool Bool(this Hashtable table, string name, bool defaultValue = default) => table.Get(name, defaultValue);
		public static string[] StringArray(this Hashtable table, string key) => table.Array(key, t => t);
		public static int[] IntArray(this Hashtable table, string key) => table.Array(key, int.Parse);
		public static float[] FloatArray(this Hashtable table, string key) => table.Array(key, float.Parse);

		private static E[] Array<E>(this Hashtable table, string key, Func<string, E> parse) {
			var stringValue = table.String(key);
			if (string.IsNullOrEmpty(stringValue)) return new E[0];
			return stringValue.Split('|').Select(parse).ToArray();
		}

		public static Color Color(this Hashtable table, string key) {
			var asArray = table.FloatArray(key);
			return new Color(asArray[0], asArray[1], asArray[2], asArray[3]);
		}

		public static Vector3 Vector3(this Hashtable table, string key) {
			var asArray = table.FloatArray(key);
			return new Vector3(asArray[0], asArray[1], asArray[2]);
		}

		public static Vector2 Vector2(this Hashtable table, string key) {
			var asArray = table.FloatArray(key);
			return new Vector2(asArray[0], asArray[1]);
		}

		public static bool TryGet<E>(this Hashtable table, string name, out E value) {
			if (table.ContainsKey(name) && table[name] is E castValue) {
				value = castValue;
				return true;
			}
			value = default;
			return false;
		}

		private static E Get<E>(this Hashtable table, string name, E defaultValue = default) => table.TryGet<E>(name, out var castValue) ? castValue : defaultValue;

		public static void Define(this Hashtable table, string key, int i) => table[key] = i;
		public static void Define(this Hashtable table, string key, float f) => table[key] = f;
		public static void Define(this Hashtable table, string key, string s) => table[key] = s;
		public static void Define(this Hashtable table, string key, bool b) => table[key] = b;
		public static void Define(this Hashtable table, string key, byte b) => table[key] = b;
		public static void Define(this Hashtable table, string key, Color color) => table.Define(key, $"{color.r}|{color.g}|{color.b}|{color.a}");
		public static void Define(this Hashtable table, string key, Vector3 v3) => table.Define(key, $"{v3.x}|{v3.y}|{v3.z}");
		public static void Define(this Hashtable table, string key, Vector2 v2) => table.Define(key, $"{v2.x}|{v2.y}");
		public static void Define(this Hashtable table, string key, IEnumerable<string> array) => table.Define(key, array.Join("|"));
		public static void Define(this Hashtable table, string key, IEnumerable<int> array) => table.Define(key, array.Join("|"));
		public static void Define(this Hashtable table, string key, IEnumerable<float> array) => table.Define(key, array.Join("|"));
	}
}