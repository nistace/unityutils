using System.Linq;
using System.Text;

public static class StringExtension {
	public static bool TryParseFloat(this string str, out float res) {
		return float.TryParse(str.Replace(".", ","), out res);
	}

	public static bool TryParseInt(this string str, out int res) {
		return int.TryParse(str.Replace(".", ","), out res);
	}

	public static string WithLength(this string str, int length, char character) {
		if (str.Length == length) return str;
		if (str.Length > length) return str.Substring(0, length);
		var strBuilder = new StringBuilder(str);
		for (var i = str.Length; i < length; ++i)
			strBuilder.Append(character);
		return strBuilder.ToString();
	}

	public static string WithEnding(this string str, string ending) {
		if (str.EndsWith(ending)) return str;
		return $"{str}{ending}";
	}

	public static string CleanKey(this string rawKey) {
		return rawKey?.Trim().ToLower().Replace(" ", "") ?? "";
	}

	public static string ToUpperFirst(this string str) {
		if (str.Length < 2) return str.ToUpper();
		return $"{str[0].ToString().ToUpper()}{str.Substring(1)}";
	}

	public static string ToLowerFirst(this string str) {
		if (str.Length < 2) return str.ToLower();
		return $"{str[0].ToString().ToLower()}{str.Substring(1)}";
	}

	public static string PascalCase(this string str) => string.IsNullOrEmpty(str) ? string.Empty : str.ToLower().Split(' ').Select(t => t.ToUpperFirst()).Join("");
	public static string CamelCase(this string str) => str.PascalCase().ToLowerFirst();
}