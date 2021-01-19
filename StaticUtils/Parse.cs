using System;
using System.Globalization;

namespace Utils.StaticUtils {
	public static class Parse {
		public static bool Bool(string str) => TryBool(str, out var res) ? res : throw new FormatException($"Cannot parse {str} to bool");
		public static float Float(string str) => TryFloat(str, out var res) ? res : throw new FormatException($"Cannot parse {str} to float");
		public static byte Byte(string str) => TryByte(str, out var res) ? res : throw new FormatException($"Cannot parse {str} to byte");
		public static int Int(string str) => TryInt(str, out var res) ? res : throw new FormatException($"Cannot parse {str} to int");

		public static bool TryFloat(string str, out float res) => float.TryParse(str.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out res);
		public static bool TryByte(string str, out byte res) => byte.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out res);
		public static bool TryInt(string str, out int res) => int.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out res);
		public static bool TryBool(string str, out bool res) => bool.TryParse(str, out res);
	}
}