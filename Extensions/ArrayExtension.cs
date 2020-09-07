using System;
using System.Collections.Generic;

public static class ArrayExtension {
	public static E[] SubArray<E>(this E[] array, int firstIndex, int length) {
		var result = new E[length];
		for (var i = 0; i < length; ++i) {
			result[i] = array[i + firstIndex];
		}
		return result;
	}

	public static E[] FilledWith<E>(this E[] array, E value) {
		for (var i = 0; i < array.Length; ++i) array[i] = value;
		return array;
	}

	public static E[,] FilledWith<E>(this E[,] array, E value) {
		for (var i = 0; i < array.GetLength(0); ++i)
		for (var j = 0; j < array.GetLength(1); ++j)
			array[i, j] = value;
		return array;
	}

	public static IEnumerable<E> Items<E>(this E[,] array) {
		var items = new HashSet<E>();
		for (var i = 0; i < array.GetLength(0); ++i)
		for (var j = 0; j < array.GetLength(1); ++j)
			items.Add(array[i, j]);
		return items;
	}

	public static int Sum<E>(this E[,] array, Func<E, int> func) {
		var sum = 0;
		for (var i = 0; i < array.GetLength(0); ++i)
		for (var j = 0; j < array.GetLength(1); ++j)
			sum += func(array[i, j]);
		return sum;
	}

	public static float Sum<E>(this E[,] array, Func<E, float> func) {
		var sum = 0f;
		for (var i = 0; i < array.GetLength(0); ++i)
		for (var j = 0; j < array.GetLength(1); ++j)
			sum += func(array[i, j]);
		return sum;
	}

	public static void RemoveDupes<E>(this List<E> items) {
		for (var i = 0; i < items.Count - 1; ++i)
		for (var j = i + 1; j < items.Count; ++j) {
			if (ReferenceEquals(items[i], items[j]) || items[i].Equals(items[j])) items.RemoveAt(j);
		}
	}

	public static void InsertFirst<E>(this List<E> items, E item) => items.Insert(0, item);

	public static void Append<E>(this List<E> items, IEnumerable<E> newItems) => items.AddRange(newItems);

	public static void AppendReversed<E>(this List<E> items, IEnumerable<E> newItems) {
		var index = items.Count;
		foreach (var newItem in newItems) items.Insert(index, newItem);
	}

	public static void Prepend<E>(this List<E> items, IEnumerable<E> newItems) {
		var nextIndex = 0;
		foreach (var newItem in newItems) {
			items.Insert(nextIndex, newItem);
			nextIndex++;
		}
	}

	public static void PrependRevered<E>(this List<E> items, IEnumerable<E> newItems) {
		foreach (var newItem in newItems) items.Insert(0, newItem);
	}

	public static E GetSafe<E>(this IReadOnlyList<E> array, int index, E defaultValue = default) => array.TryGet(index, out var e) ? e : defaultValue;

	public static bool TryGet<E>(this IReadOnlyList<E> list, int index, out E e) {
		e = default;
		if (index < 0 || index >= list.Count) return false;
		e = list[index];
		return true;
	}

	public static int IndexOf<E>(this IReadOnlyList<E> list, E item) {
		for (var i = 0; i < list.Count; ++i)
			if (!ReferenceEquals(list[i], null) && list[i].Equals(item))
				return i;
		return -1;
	}

	public static int IndexOfMax(this IList<float> list) => list.IndexOfMax(t => t);
	public static int IndexOfMax(this IList<int> list) => list.IndexOfMax(t => t);

	public static int IndexOfMax<E>(this IList<E> list, Func<E, float> maxFunc) {
		var max = float.MinValue;
		var index = -1;
		for (var i = 0; i < list.Count; ++i) {
			var itemValue = maxFunc(list[i]);
			if (!(itemValue > max)) continue;
			index = i;
			max = itemValue;
		}
		return index;
	}
}