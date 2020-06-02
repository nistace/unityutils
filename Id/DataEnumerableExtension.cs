using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DataEnumerableExtension {
	public static E WithId<E>(IEnumerable<E> items, int id) where E : IData {
		var item = items.SingleOrDefault(t => t.id == id);
		if (item == null) Debug.LogError($"No {typeof(E).Name} with id {id}");
		return item;
	}
}