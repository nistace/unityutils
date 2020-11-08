using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExitGames.Client.Photon;
using UnityEngine;

namespace Utils.Extensions {
	public static class PunHashtableExtension {
		public static int Int(this Hashtable table, string name, int defaultValue = default) => table.TryInt(name, out var value) ? value : defaultValue;
		public static bool TryInt(this Hashtable table, string name, out int value) => table.TryGet(name, out value, int.Parse);
		public static byte Byte(this Hashtable table, string name, byte defaultValue = default) => table.TryByte(name, out var value) ? value : defaultValue;
		public static bool TryByte(this Hashtable table, string name, out byte value) => table.TryGet(name, out value, byte.Parse);
		public static string String(this Hashtable table, string name, string defaultValue = default) => table.TryString(name, out var value) ? value : defaultValue;
		public static bool TryString(this Hashtable table, string name, out string value) => table.TryGet(name, out value, t => t);
		public static float Float(this Hashtable table, string name, float defaultValue = default) => table.TryFloat(name, out var value) ? value : defaultValue;
		public static bool TryFloat(this Hashtable table, string name, out float value) => table.TryGet(name, out value, float.Parse);
		public static bool Bool(this Hashtable table, string name, bool defaultValue = default) => table.TryBool(name, out var value) ? value : defaultValue;
		public static bool TryBool(this Hashtable table, string name, out bool value) => table.TryGet(name, out value, bool.Parse);
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

		private static bool TryGet<E>(this Hashtable table, string name, out E value, Func<string, E> parseFunc) {
			value = default;
			if (!table.ContainsKey(name)) return false;
			if (table[name] is E castValue) {
				value = castValue;
				return true;
			}
			if (parseFunc == null) return false;
			if (!(table[name] is string strValue)) return false;
			try {
				value = parseFunc.Invoke(strValue);
				table[name] = value;
				return true;
			}
			catch (FormatException) { }
			return false;
		}

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

		private const int  fileCharOffset = 32;
		private const char fileSeparator  = (char) 0;

		public static void Save(this Hashtable table, string path) {
			using (var o = File.CreateText(path)) {
				var previousKey = string.Empty;
				foreach (var key in table.Keys.Select(t => t.ToString()).OrderBy(t => t)) {
					var copyPrevious = 0;
					while (copyPrevious < previousKey.Length && copyPrevious < key.Length && previousKey[copyPrevious] == key[copyPrevious]) copyPrevious++;
					o.Write($"{(char) (copyPrevious + fileCharOffset)}{key.Substring(copyPrevious)}={table[key]}{fileSeparator}");
					previousKey = key;
				}
			}
		}

		public static Hashtable Load(this Hashtable table, string path) {
			using (var o = File.OpenText(path)) {
				var text = o.ReadToEnd();
				var lines = text.Split(fileSeparator);
				var previousKey = string.Empty;
				foreach (var line in lines) {
					if (string.IsNullOrEmpty(line)) continue;
					var keyPart = line.Substring(0, line.IndexOf('='));
					var copyPrevious = keyPart[0] - fileCharOffset;
					var key = previousKey.Substring(0, copyPrevious) + keyPart.Substring(1);
					var value = line.Substring(line.IndexOf('=') + 1);
					table[key] = value;
					previousKey = key;
				}
			}
			return table;
		}
	}
}