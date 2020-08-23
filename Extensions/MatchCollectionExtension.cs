using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Utils.Extensions {
	public static class MatchCollectionExtension {
		public static IEnumerable<T> Select<T>(this MatchCollection collection, Func<Match, T> select) {
			var result = new List<T>();
			for (var i = 0; i < collection.Count; ++i) result.Add(select(collection[i]));
			return result;
		}
	}
}