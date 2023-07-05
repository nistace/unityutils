#if PHOTON_UNITY_NETWORKING
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExitGames.Client.Photon;
using NiUtils.Extensions;
using NiUtils.StaticUtils;
using UnityEngine;

namespace NiUtils.Pun.Extensions {
	public static class PunHashtableExtension {
		public static int Int(this Hashtable table, string name, int defaultValue = default) {
			if (table.TryInt(name, out var value)) return value;
			if (table.TryLong(name, out var longValue)) return (int)longValue;
			return defaultValue;
		}

		public static bool TryInt(this Hashtable table, string name, out int value) => table.TryGet(name, out value, Parse.Int);
		public static bool TryLong(this Hashtable table, string name, out long value) => table.TryGet(name, out value, Parse.Long);
		public static byte Byte(this Hashtable table, string name, byte defaultValue = default) => table.TryByte(name, out var value) ? value : defaultValue;
		public static bool TryByte(this Hashtable table, string name, out byte value) => table.TryGet(name, out value, Parse.Byte);
		public static string String(this Hashtable table, string name, string defaultValue = default) => table.TryString(name, out var value) ? value : defaultValue;
		public static bool TryString(this Hashtable table, string name, out string value) => table.TryGet(name, out value, t => t);
		public static float Float(this Hashtable table, string name, float defaultValue = default) => table.TryFloat(name, out var value) ? value : defaultValue;
		public static bool TryFloat(this Hashtable table, string name, out float value) => table.TryGet(name, out value, Parse.Float);
		public static bool Bool(this Hashtable table, string name, bool defaultValue = default) => table.TryBool(name, out var value) ? value : defaultValue;
		public static bool TryBool(this Hashtable table, string name, out bool value) => table.TryGet(name, out value, Parse.Bool);
		public static string[] StringArray(this Hashtable table, string key) => table.Array(key, t => t);
		public static int[] IntArray(this Hashtable table, string key) => table.Array(key, Parse.Int);
		public static float[] FloatArray(this Hashtable table, string key) => table.Array(key, Parse.Float);

		private static E[] Array<E>(this Hashtable table, string key, Func<string, E> parse) {
			var stringValue = table.String(key);
			if (string.IsNullOrEmpty(stringValue)) return new E[0];
			return stringValue.Split('|').Select(parse).ToArray();
		}

		public static Color Color(this Hashtable table, string key) {
			var asArray = table.FloatArray(key);
			return new Color(asArray?.GetSafe(0) ?? 0, asArray?.GetSafe(1) ?? 0, asArray?.GetSafe(2) ?? 0, asArray?.GetSafe(3) ?? 0);
		}

		public static Vector3 Vector3(this Hashtable table, string key) {
			var asArray = table.FloatArray(key);
			return new Vector3(asArray?.GetSafe(0) ?? 0, asArray?.GetSafe(1) ?? 0, asArray?.GetSafe(2) ?? 0);
		}

		public static Quaternion Quaternion(this Hashtable table, string key) {
			var asArray = table.FloatArray(key);
			return new Quaternion(asArray?.GetSafe(0) ?? 0, asArray?.GetSafe(1) ?? 0, asArray?.GetSafe(2) ?? 0, asArray?.GetSafe(3) ?? 0);
		}

		public static Vector2 Vector2(this Hashtable table, string key) {
			var asArray = table.FloatArray(key);
			return new Vector2(asArray?.GetSafe(0) ?? 0, asArray?.GetSafe(1) ?? 0);
		}

		public static Vector2Int Vector2Int(this Hashtable table, string key) {
			var asArray = table.IntArray(key);
			return new Vector2Int(asArray?.GetSafe(0) ?? 0, asArray?.GetSafe(1) ?? 0);
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

		private static Hashtable With<E>(this Hashtable table, string key, E val) {
			table[key] = val;
			return table;
		}

		public static Hashtable Define(this Hashtable table, string key, int i) => table.With(key, i);
		public static Hashtable Define(this Hashtable table, string key, float f) => table.With(key, f);
		public static Hashtable Define(this Hashtable table, string key, string s) => table.With(key, s);
		public static Hashtable Define(this Hashtable table, string key, bool b) => table.With(key, b);
		public static Hashtable Define(this Hashtable table, string key, byte b) => table.With(key, b);
		public static Hashtable Define(this Hashtable table, string key, Color color) => table.Define(key, $"{color.r}|{color.g}|{color.b}|{color.a}");
		public static Hashtable Define(this Hashtable table, string key, Vector3 v3) => table.Define(key, $"{v3.x}|{v3.y}|{v3.z}");
		public static Hashtable Define(this Hashtable table, string key, Vector2 v2) => table.Define(key, $"{v2.x}|{v2.y}");
		public static Hashtable Define(this Hashtable table, string key, Vector2Int v2) => table.Define(key, $"{v2.x}|{v2.y}");
		public static Hashtable Define(this Hashtable table, string key, Quaternion q) => table.Define(key, $"{q.x}|{q.y}|{q.z}|{q.w}");
		public static Hashtable Define(this Hashtable table, string key, IEnumerable<string> array) => table.Define(key, array.Join("|"));
		public static Hashtable Define(this Hashtable table, string key, IEnumerable<int> array) => table.Define(key, array.Join("|"));
		public static Hashtable Define(this Hashtable table, string key, IEnumerable<float> array) => table.Define(key, array.Join("|"));

		private const int  fileCharOffset = 32;
		private const char fileSeparator  = (char)0;

		public static void Save(this Hashtable table, string path) {
			using (var o = File.CreateText(path)) {
				var previousKey = string.Empty;
				foreach (var key in table.Keys.Select(t => t.ToString()).OrderBy(t => t)) {
					var copyPrevious = 0;
					while (copyPrevious < previousKey.Length && copyPrevious < key.Length && previousKey[copyPrevious] == key[copyPrevious]) copyPrevious++;
					o.Write($"{(char)(copyPrevious + fileCharOffset)}{key.Substring(copyPrevious)}={table[key]}{fileSeparator}");
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
#endif