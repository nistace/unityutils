using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Factories {
	public class Factory : MonoBehaviour {
		private static Dictionary<Type, Factory> factories { get; } = new Dictionary<Type, Factory>();

		public static TE Get<TE>() where TE : Factory {
			if (factories.ContainsKey(typeof(TE)) && factories[typeof(TE)]) return factories[typeof(TE)] as TE;
			Debug.LogWarning($"No factory of type {typeof(TE).FullName}. Instantiating default one.");
			return new GameObject(typeof(TE).Name).AddComponent<TE>();
		}

		private void Awake() {
			if (factories.ContainsKey(GetType())) factories.Remove(GetType());
			factories.Add(GetType(), this);
		}

		private void OnDestroy() {
			if (factories.ContainsKey(GetType()) && factories[GetType()] == this) {
				factories.Remove(GetType());
			}
		}
	}
}