using System.Collections.Generic;
using System.Linq;

namespace Utils.Extensions {
	public static class DictionaryExtension {
		private static void Set<E, F>(this Dictionary<E, F> dico, KeyValuePair<E, F> kvp) {
			dico.Set(kvp.Key, kvp.Value);
		}

		public static void Set<E, F>(this IDictionary<E, F> dico, E e, F f) {
			if (dico.ContainsKey(e)) dico[e] = f;
			else dico.Add(e, f);
		}

		public static void Set<E, F>(this Dictionary<E, F> dico, E e, F f) => (dico as IDictionary<E, F>).Set(e, f);

		public static void SetAll<E, F>(this Dictionary<E, F> dico, IEnumerable<KeyValuePair<E, F>> values) {
			values.ForEach(dico.Set);
		}

		public static void CompleteWith<E, F>(this Dictionary<E, F> dico, IEnumerable<KeyValuePair<E, F>> additionalValues) {
			foreach (var additional in additionalValues) {
				if (!dico.ContainsKey(additional.Key)) {
					dico.Add(additional.Key, additional.Value);
				}
			}
		}

		public static F Of<E, F>(this IReadOnlyDictionary<E, F> dico, E key, F defaultValue = default) => dico.ContainsKey(key) ? dico[key] : defaultValue;
		public static F Of<E, F>(this IDictionary<E, F> dico, E key, F defaultValue = default) => (dico as IReadOnlyDictionary<E, F>).Of(key, defaultValue);
		public static F Of<E, F>(this Dictionary<E, F> dico, E key, F defaultValue = default) => (dico as IReadOnlyDictionary<E, F>).Of(key, defaultValue);

		public static void Append<E, F>(this Dictionary<E, HashSet<F>> dico, E key, IEnumerable<F> newItems) {
			var itemsToAdd = newItems as F[] ?? newItems.ToArray();
			if (!itemsToAdd.Any()) return;
			if (!dico.ContainsKey(key)) dico.Add(key, new HashSet<F>());
			dico[key].AddAll(itemsToAdd);
		}

		public static void Append<E, F>(this Dictionary<E, IEnumerable<F>> dico, E key, IEnumerable<F> newItems) {
			var itemsToAdd = newItems as F[] ?? newItems.ToArray();
			if (!itemsToAdd.Any()) return;
			if (!dico.ContainsKey(key)) dico.Add(key, new HashSet<F>());
			dico.Set(key, dico[key].Union(itemsToAdd));
		}

		public static E[] ToArray<E>(this Dictionary<int, E> dictionary) {
			var array = new E[dictionary.Keys.Max()];
			dictionary.ForEach(t => array[t.Key] = t.Value);
			return array;
		}

		public static void RemoveAll<E, F>(this Dictionary<E, F> dico, IEnumerable<E> itemsToRemove) {
			foreach (var e in itemsToRemove) dico.Remove(e);
		}
	}
}