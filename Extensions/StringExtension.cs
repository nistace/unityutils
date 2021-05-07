using System.Linq;
using System.Text;

namespace Utils.Extensions {
	public static class StringExtension {
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

		public static string CleanKey(this string rawKey, bool keepCase = false) {
			if (string.IsNullOrEmpty(rawKey)) return string.Empty;
			var cleanKey = rawKey.Trim().Replace(" ", "");
			if (!keepCase) cleanKey = cleanKey.ToLower();
			return cleanKey;
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
		public static string LowerNoSpace(this string str) => str.PascalCase().ToLower();
		public static string Repeated(this string str, int count) => new StringBuilder(str.Length * count).Insert(0, str, count).ToString();
	}
}