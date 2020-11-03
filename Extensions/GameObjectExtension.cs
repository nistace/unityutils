using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public static class GameObjectExtension {
	public static E GetOrAddComponent<E>(this Component t) where E : Component {
		return t.gameObject.GetOrAddComponent<E>();
	}

	public static E GetOrAddComponent<E>(this GameObject t) where E : Component {
		var e = t.GetComponent<E>();
		if (e) return e;
		return t.AddComponent<E>();
	}

	public static IEnumerable<Transform> Children(this Transform parent) {
		var children = new List<Transform>();
		for (var i = 0; i < parent.childCount; ++i) {
			children.Add(parent.GetChild(i));
		}
		return children;
	}

	public static void ClearChildren(this Transform parent) {
		var destroyAction = Application.isPlaying ? (Action<Object>) Object.Destroy : Object.DestroyImmediate;
		foreach (var child in parent.Children()) {
			destroyAction.Invoke(child.gameObject);
		}
	}

	public static bool TryFindNamedChild<E>(this Component source, out E component, string name, bool caseSensitive = true) where E : Component =>
		TryFindNamedChild(source?.transform, out component, name, caseSensitive);

	public static bool TryFindNamedChild<E>(this GameObject gameObject, out E component, string name, bool caseSensitive = true) where E : Component =>
		TryFindNamedChild(gameObject?.transform, out component, name, caseSensitive);

	public static bool TryFindNamedChild<E>(this Transform transform, out E component, string name, bool caseSensitive = true) where E : Component {
		component = null;
		var strComparison = caseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;
		foreach (var e in transform.GetComponentsInChildren<E>()) {
			Debug.Log($"{e.name} ?= {name} : {string.Equals(e.name, name, strComparison)}");
			if (!string.Equals(e.name, name, strComparison)) continue;
			component = e;
			return true;
		}
		return false;
	}

	public static GameObject ParentedTo(this GameObject o, Transform parent) {
		o.transform.SetParent(parent);
		return o;
	}

	public static E ParentedTo<E>(this E o, Transform parent) where E : Component {
		o.transform.SetParent(parent);
		return o;
	}

	public static E Named<E>(this E o, string name) where E : Object {
		o.name = name;
		return o;
	}

	public static string GetAbsoluteName(this Component component) => component?.gameObject.GetAbsoluteName();

	public static string GetAbsoluteName(this GameObject gameObject) {
		if (!gameObject) return "null";
		if (!gameObject.transform.parent) return gameObject.name;
		return gameObject.transform.parent.GetAbsoluteName() + "/" + gameObject.name;
	}

	public static E FirstChildWithComponentOrDefault<E>(this Transform transform) where E : Component =>
		transform.Children().Select(child => child.GetComponent<E>()).FirstOrDefault(component => component);

	public static bool TryGetComponents<E>(this Component component, out E[] components) => component.gameObject.TryGetComponents(out components);

	public static bool TryGetComponents<E>(this GameObject gameObject, out E[] components) {
		components = gameObject.GetComponents<E>();
		return (components?.Length ?? 0) > 0;
	}

	public static bool TryGetComponentInParent<E>(this GameObject gameObject, out E component) {
		component = gameObject.GetComponentInParent<E>();
		return component != null;
	}

	public static IEnumerable<E> SelectNotNullComponents<E>(this IEnumerable<GameObject> gameObjects) {
		var selects = new List<E>();
		foreach (var gameObject in gameObjects.NotNull())
			if (gameObject && gameObject.TryGetComponent<E>(out var component))
				selects.Add(component);
		return selects;
	}

	public static IEnumerable<E> SelectNotNullComponents<E>(this IEnumerable<Component> components) {
		var selects = new List<E>();
		foreach (var component in components.NotNull())
			if (component && component.TryGetComponent<E>(out var eComponent))
				selects.Add(eComponent);
		return selects;
	}

	public static IEnumerable<E> SelectNotNullComponentsWhere<E>(this IEnumerable<Component> components, Func<E, bool> where) => components.SelectNotNullComponents<E>().Where(where);

	public static E SingleOrDefault<E>(this IEnumerable<GameObject> gameObjects, bool defaultIfMultiple = false) where E : Component {
		var selects = new List<E>();
		foreach (var gameObject in gameObjects)
			if (gameObject.TryGetComponent<E>(out var component))
				selects.Add(component);
		if (selects.Count > 1 && defaultIfMultiple) return default;
		if (selects.Count > 1) throw new InvalidOperationException("Sequence contains more than one element");
		if (selects.Count == 0) return default;
		return selects[0];
	}

	public static E SingleOrDefault<E>(this IEnumerable<Component> components) where E : Component => components.Select(t => t.gameObject).SingleOrDefault<E>();

	public static bool TrySingleOrDefault<E>(this IEnumerable<GameObject> gameObjects, out E single) where E : Component {
		single = gameObjects.SingleOrDefault<E>(true);
		return single;
	}

	public static bool TrySingleOrDefault<E>(this IEnumerable<Component> components, out E single) where E : Component => components.Select(t => t.gameObject).TrySingleOrDefault(out single);

	public static GameObject Active(this GameObject go) {
		go.SetActive(true);
		return go;
	}

	public static GameObject Inactive(this GameObject go) {
		go.SetActive(false);
		return go;
	}
}