using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Utils.Extensions;
using Utils.Saving;

namespace Utils.Id {
	public static class DataKeys {
		public static string Child(string root, string childName) => $"{root}.{childName}";
		public static string Item(string root, int index) => $"{root}.{index}";

		public static void SaveList<E>(Hashtable data, string root, IReadOnlyList<E> items) where E : ISaveToHashtable {
			data.Define(root, items.Count);
			items.ForEach((t, i) => t.Save(data, Item(root, i)));
		}

		public static void SaveList<E>(Hashtable data, string root, IReadOnlyList<E> items, Action<E, string> saveItemFunc) {
			data.Define(root, items.Count);
			items.ForEach((t, i) => saveItemFunc(t, Item(root, i)));
		}

		public static void SaveList<E>(Hashtable data, string root, IReadOnlyList<E> items, Action<E, Hashtable, string> saveItemFunc) {
			data.Define(root, items.Count);
			items.ForEach((t, i) => saveItemFunc(t, data, Item(root, i)));
		}

		public static E[] LoadList<E>(Hashtable data, string root, Func<string, E> parseFunc) {
			var result = new E[data.Int(root)];
			for (var i = 0; i < result.Length; ++i) result[i] = parseFunc(Item(root, i));
			return result;
		}

		public static E[] LoadList<E>(Hashtable data, string root, Func<Hashtable, string, E> parseFunc) {
			var result = new E[data.Int(root)];
			for (var i = 0; i < result.Length; ++i) result[i] = parseFunc(data, Item(root, i));
			return result;
		}

		public static void SaveListOfIds<E>(Hashtable data, string root, IReadOnlyList<E> items) where E : DataScriptableObject {
			data.Define(root, items.Count);
			items.ForEach((t, i) => data.Define(Item(root, i), t.id));
		}

		public static E[] LoadListFromIds<E>(Hashtable data, string root, IReadOnlyDictionary<int, E> dictionary) where E : DataScriptableObject {
			var result = new E[data.Int(root)];
			for (var i = 0; i < result.Length; ++i) result[i] = dictionary[data.Int(Item(root, i))];
			return result;
		}

		public static E[] LoadListFromIds<E>(Hashtable data, string root, Func<int, E> getFromIdFunc) where E : DataScriptableObject {
			var result = new E[data.Int(root)];
			for (var i = 0; i < result.Length; ++i) result[i] = getFromIdFunc(data.Int(Item(root, i)));
			return result;
		}

		public static void ForEach(Hashtable data, string root, Action<string> foreachFunc) {
			var count = data.Int(root);
			for (var i = 0; i < count; ++i) foreachFunc(Item(root, i));
		}
	}
}