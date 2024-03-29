﻿using System;
using System.Collections.Generic;
using System.Linq;
using NiUtils.Extensions;
using NiUtils.Id;
using UnityEditor;
using UnityEngine;

namespace NiUtils.Libraries {
	public abstract class Library<E> : DataScriptableObject {
		[SerializeField] protected E        _defaultItem;
		[SerializeField] protected int      _orderIndex;
		[SerializeField] protected string[] _itemNames;
		[SerializeField] protected E[]      _items;

		public  int                   orderIndex  => _orderIndex;
		private Dictionary<string, E> map         { get; } = new Dictionary<string, E>();
		public  IEnumerable<E>        allItems    => map.Values;
		public  int                   count       => _items.Length;
		public  E                     defaultItem => _defaultItem;

		public E this[string key] {
			get {
				var cleanKey = key.CleanKey();
				if (map.ContainsKey(cleanKey)) return map[cleanKey];
				if (Application.isPlaying) Debug.LogWarning(GetNonExistingWarningMessage(cleanKey));
				return _defaultItem;
			}
		}

		protected abstract string GetNonExistingWarningMessage(string key);

		public void Load() {
			map.Clear();
			for (var i = 0; i < Mathf.Min(_itemNames.Length, _items.Length); ++i) {
				var cleanKey = _itemNames[i].CleanKey();
				if (map.ContainsKey(cleanKey)) Debug.LogWarning($"Two keys with the same name : {cleanKey}");
				map.Set(cleanKey, _items[i]);
			}
		}

		public void Set(string spriteIdentifier, E data) {
			var index = _itemNames.IndexOf(spriteIdentifier);
			if (index < 0) {
				index = _itemNames.Length;
				_itemNames = _itemNames.WithAppended(spriteIdentifier);
			}
			if (_items.Length <= index) _items = _items.WithLength(index + 1);
			_items[index] = data;
		}

		public E GetRandom(string keyRoot) => this[GetRandomKey(keyRoot)];

		private bool TryGetRandomKey(string keyRoot, out string randomKey) {
			randomKey = default;
			var cleanKey = keyRoot.CleanKey().WithEnding(".");
			if (map.Where(t => t.Key.StartsWith(cleanKey)).TryRandom(out var result)) {
				randomKey = result.Key;
				return true;
			}
			return false;
		}

		public string GetRandomKey(string keyRoot) {
			if (!TryGetRandomKey(keyRoot, out var randomKey) && Application.isPlaying) {
				Debug.LogWarning(GetNonExistingWarningMessage(keyRoot.CleanKey().WithEnding(".")));
			}
			return randomKey;
		}

		public bool HasKey(string key) => map.ContainsKey(key.CleanKey());
		public bool HasRandomKey(string keyRoot) => TryGetRandomKey(keyRoot, out _);

		public IReadOnlyDictionary<string, E> AllStartingWith(string keyRoot) {
			var cleanKey = keyRoot.CleanKey().WithEnding(".");
			return map.Keys.Where(t => t.StartsWith(cleanKey)).ToDictionary(t => t, t => this[t]);
		}

		[Serializable]
		public class NamedItem {
			[SerializeField] protected string _name;
			[SerializeField] protected E      _item;

			public string name => _name;

			public E item => _item;
		}

		public void SortItems() {
			var orderedCouples = Mathf.Min(_items.Length, _itemNames.Length).CreateArray(i => (_itemNames[i], _items[i])).OrderBy(t => t.Item1).ToArray();
			_itemNames = orderedCouples.Select(t => t.Item1).ToArray();
			_items = orderedCouples.Select(t => t.Item2).ToArray();
		}

#if UNITY_EDITOR
		private static string AssetPathToIdentifier(string assetPath, string pathPrefix) => assetPath.Substring(pathPrefix.Length, assetPath.LastIndexOf('.') - pathPrefix.Length).Replace('/', '.')
			.Split('.').Select(t => t.ToLowerFirst()).Join(".");

		protected static void AddToLibrariesFromAssetGuid<C>(string assetGuid, string pathPrefix, IEnumerable<Library<C>> libraries) where C : UnityEngine.Object {
			var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
			var clip = (C)AssetDatabase.LoadAssetAtPath(assetPath, typeof(C));
			var identifier = AssetPathToIdentifier(assetPath, pathPrefix);
			libraries.ForEach(t => t.Set(identifier, clip));
		}

		protected static void AddComponentToLibrariesFromAssetGuid<C>(string assetGuid, string pathPrefix, IEnumerable<Library<C>> libraries) where C : Component {
			var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
			var go = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
			var identifier = AssetPathToIdentifier(assetPath, pathPrefix);
			libraries.ForEach(t => t.Set(identifier, go.GetComponent<C>()));
		}

#endif
	}
}