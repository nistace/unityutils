using System;
using System.Linq;
using UnityEngine;

public static class ObjectExtension {
	public static bool In<E>(this E e, params E[] es) {
		if (es == null || es.Length == 0) return false;
		return es.Contains(e);
	}

	public static void ApplyRecursivelyToComponents<E>(this GameObject go, Action<E> action) => go.GetComponentsInChildren<E>().ForEach(action);
	public static void ApplyRecursivelyToComponents<E>(this Component behaviour, Action<E> action) => behaviour.gameObject.ApplyRecursivelyToComponents(action);

	public static E Do<E>(this E o, Action<E> action) {
		action(o);
		return o;
	}
}