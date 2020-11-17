using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtension {
	public static void Shuffle<E>(this IList<E> list) {
		for (var i = 0; i < list.Count - 1; ++i) {
			var swapWithIndex = UnityEngine.Random.Range(i, list.Count);
			var itemAtSwap = list[swapWithIndex];
			list[swapWithIndex] = list[i];
			list[i] = itemAtSwap;
		}
	}

	public static IList<E> Shuffled<E>(this IList<E> list) {
		list.Shuffle();
		return list;
	}

	public static bool TryRandom<E>(this IEnumerable<E> array, out E result) {
		result = default;
		var enumerable = array as E[] ?? array.ToArray();
		var r = UnityEngine.Random.Range(0, enumerable.Length);
		foreach (var e in enumerable) {
			if (r == 0) {
				result = e;
				return true;
			}
			r--;
		}
		return false;
	}

	public static E Random<E>(this IEnumerable<E> array) {
		if (array.TryRandom(out var e)) return e;
		throw new IndexOutOfRangeException("Cannot get a random item from an empty collection");
	}

	public static E RandomOrDefault<E>(this IEnumerable<E> array) {
		if (array.TryRandom(out var e)) return e;
		return default;
	}

	public static int RandomIndex<E>(this E[] array) {
		return UnityEngine.Random.Range(0, array.Length);
	}

	public static E Random<E>(this IEnumerable<E> array, Func<E, float> probability) {
		var enumerable = array as E[] ?? array.ToArray();
		if (enumerable.Length == 0) return default;
		var sumProbability = enumerable.Sum(probability);
		if (sumProbability <= 0) return default;
		var r = UnityEngine.Random.Range(0, sumProbability);
		foreach (var e in enumerable) {
			r -= probability(e);
			if (r <= 0) return e;
		}
		return default;
	}

	public static E[] Random<E>(this IList<E> array, int size, Func<E, float> probability) => array.Random(size, probability, UnityEngine.Random.Range);
	public static E[] NetworkRandom<E>(this IList<E> array, int size, Func<E, float> probability) => array.Random(size, probability, Utils.RandomUtils.NetworkRandom.Range);

	private static E[] Random<E>(this IList<E> array, int size, Func<E, float> probability, Func<float, float, float> randomRangeFunc) {
		if (array == null || array.Count == 0) return default;
		if (size == 0) return new E[] { };
		var probabilities = array.Select(probability).ToArray();
		var sumProbability = probabilities.Sum();
		if (sumProbability <= 0) return default;
		var result = new E[size];
		for (var i = 0; i < size; ++i) {
			var r = randomRangeFunc(0, sumProbability);
			var index = -1;
			while (r >= 0 && index < array.Count) {
				index++;
				r -= probabilities[index];
			}
			result[i] = array[index];
		}
		return result;
	}

	public static void EnqueueAll<E>(this Queue<E> queue, IEnumerable<E> items) {
		foreach (var e in items) queue.Enqueue(e);
	}

	public static void ForEach<E>(this IEnumerable<E> array, Action<E> func) {
		foreach (var e in array) func(e);
	}

	public static void ForEach<E>(this IReadOnlyList<E> array, Action<E, int> func) {
		for (var i = 0; i < array.Count; ++i) func(array[i], i);
	}

	public static int IndexOf<E>(this E[] array, E item) {
		for (var i = 0; i < array.Length; ++i)
			if (Equals(array[i], item))
				return i;
		return -1;
	}

	public static int IndexWhere<E>(this E[] array, Func<E, bool> condition) {
		for (var i = 0; i < array.Length; ++i)
			if (condition(array[i]))
				return i;
		return -1;
	}

	public static F SelectSingleOrDefault<E, F>(this IEnumerable<E> items, Func<E, F> select, Func<E, bool> where) where E : class {
		var singleOrDefault = items.SingleOrDefault(where);
		return singleOrDefault == null ? default : select(singleOrDefault);
	}

	public static void RemoveAll<E>(this ICollection<E> collection, IEnumerable<E> removeItems) {
		foreach (var e in removeItems) {
			collection.Remove(e);
		}
	}

	public static E[] WithAppended<E>(this E[] items, params E[] append) {
		var newArray = new E[items.Length + append.Length];
		for (var i = 0; i < items.Length; ++i)
			newArray[i] = items[i];
		for (var i = 0; i < append.Length; ++i)
			newArray[items.Length + i] = append[i];
		return newArray;
	}

	public static E[] WithEnding<E>(this E[] items, params E[] end) {
		var newArray = new E[items.Length];
		var lengthDelta = items.Length - end.Length;
		for (var i = 0; i < items.Length; ++i) {
			newArray[i] = i >= lengthDelta ? end[i - lengthDelta] : items[i];
		}
		return newArray;
	}

	public static E[] WithPrepended<E>(this E[] items, params E[] prepend) => prepend.WithAppended(items);

	public static E[] WithItemRemovedAt<E>(this E[] items, int removeIndex) {
		if (removeIndex < 0 || removeIndex >= items.Length) return items;
		var newArray = items.WithLength(items.Length - 1);
		for (var i = removeIndex; i < newArray.Length; ++i) newArray[i] = items[i + 1];
		return newArray;
	}

	public static void AddAll<E>(this ISet<E> set, IEnumerable<E> itemsToAdd) {
		foreach (var e in itemsToAdd) {
			set.Add(e);
		}
	}

	/// <summary> Get the first index for which the element satisfies the condition. If no item satisfies the condition, -1 is returned </summary>
	public static int FirstIndex<E>(this E[] array, Func<E, bool> where) {
		for (var i = 0; i < array.Length; ++i) {
			if (where(array[i])) return i;
		}
		return -1;
	}

	public static F ValueOrDefault<E, F>(this IDictionary<E, F> dictionary, E key) {
		return dictionary.ContainsKey(key) ? dictionary[key] : default;
	}

	public static IEnumerable<E> Firsts<E>(this IEnumerable<E> enumerable, int amount) {
		var result = new List<E>();
		if (amount <= 0) return result;
		var added = 0;
		foreach (var e in enumerable) {
			result.Add(e);
			added++;
			if (added == amount) {
				return result;
			}
		}
		return result;
	}

	public static E[] WithLength<E>(this E[] array, int newLength, E defaultValue = default) {
		if (array.Length == newLength) return array;
		var newArray = new E[newLength];
		for (var i = 0; i < newArray.Length; ++i) {
			newArray[i] = i < array.Length ? array[i] : defaultValue;
		}
		return newArray;
	}

	public static int LastIndex<E>(this E[] array, Func<E, bool> where) {
		for (var i = array.Length - 1; i >= 0; --i) {
			if (where(array[i])) return i;
		}
		return -1;
	}

	public static void Swap<E>(this E[] array, int index1, int index2) {
		var item1 = array[index1];
		array[index1] = array[index2];
		array[index2] = item1;
	}

	public static string Join<E>(this IEnumerable<E> strings, string separator) {
		return String.Join(separator, strings.Select(t => t?.ToString() ?? "null"));
	}

	public static E[] CreateArray<E>(this int length, Func<int, E> createItemFunc) {
		var array = new E[length];
		for (var i = 0; i < length; ++i) {
			array[i] = createItemFunc(i);
		}
		return array;
	}

	public static IEnumerable<E> WhereIndex<E>(this IEnumerable<E> items, Func<int, bool> condition) => items.Where((t, i) => condition(i)).ToList();

	public static F MaxOrDefault<E, F>(this IEnumerable<E> items, Func<E, F> maxFunc, F defaultValue = default) where F : IComparable<F> {
		var enumerable = items as E[] ?? items.ToArray();
		return enumerable.Any() ? enumerable.Max(maxFunc) : defaultValue;
	}

	public static F MinOrDefault<E, F>(this IEnumerable<E> items, Func<E, F> minFunc, F defaultValue = default) {
		var enumerable = items as E[] ?? items.ToArray();
		return enumerable.Any() ? enumerable.Min(minFunc) : defaultValue;
	}

	public static IEnumerable<E> NotNull<E>(this IEnumerable<E> items) where E : class {
		return items.Where(t => t != null);
	}

	public static E Last<E>(this IList<E> list) {
		return list.Count == 0 ? default : list[list.Count - 1];
	}

	public static void Sort<E>(this List<E> list, Func<E, int> valFunc) {
		list.Sort((t, u) => valFunc(t) - valFunc(u));
	}

	public static void Sort<E>(this List<E> list, Func<E, float> valFunc) {
		list.Sort((t, u) => (valFunc(t) - valFunc(u)).RoundAwayFrom(0));
	}

	public static HashSet<E> ToSet<E>(this IEnumerable<E> enumerable) {
		return new HashSet<E>(enumerable);
	}

	public static void RemoveWhere<E>(this ICollection<E> items, Func<E, bool> where) {
		items.RemoveAll(items.Where(where).ToSet());
	}

	public static IEnumerable<E> WithRemoved<E>(this IEnumerable<E> items, Func<E, bool> where) {
		var enumerable = items as E[] ?? items.ToArray();
		return enumerable.Except(enumerable.Where(where));
	}

	public static E WhereMax<E>(this IEnumerable<E> items, Func<E, float> getValue) {
		var max = float.MinValue;
		E itemMax = default;
		foreach (var item in items) {
			var itemValue = getValue(item);
			if (itemValue <= max) continue;
			max = itemValue;
			itemMax = item;
		}
		return itemMax;
	}

	public static E WhereMin<E>(this IEnumerable<E> items, Func<E, float> getValue) {
		var min = float.MaxValue;
		E itemMin = default;
		foreach (var item in items) {
			var itemValue = getValue(item);
			if (itemValue >= min) continue;
			min = itemValue;
			itemMin = item;
		}
		return itemMin;
	}

	public static FloatRange Sum<E>(this IEnumerable<E> items, Func<E, FloatRange> getValue) {
		return items.Aggregate(FloatRange.zero, (current, e) => current + getValue(e));
	}

	public static IntRange Sum<E>(this IEnumerable<E> items, Func<E, IntRange> getValue) {
		return items.Aggregate(IntRange.zero, (current, e) => current + getValue(e));
	}

	public static IEnumerable<E> Except<E>(this IEnumerable<E> items, Func<E, bool> exclusionFunction) => items.Where(t => !exclusionFunction(t));

	public static E Cleared<E, F>(this E collection) where E : ICollection<F> {
		collection.Clear();
		return collection;
	}
}