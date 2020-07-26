using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.Id {
	public static class ModificationSafeScriptables {
		private static Dictionary<ScriptableObject, ScriptableObject> scriptables { get; } = new Dictionary<ScriptableObject, ScriptableObject>();

		public static E GetModifiableInstance<E>(E source) where E : ScriptableObject {
			if (scriptables.ContainsKey(source)) return (E) scriptables[source];
			var modifiableInstance = Object.Instantiate(source);
			modifiableInstance.name = source.name;
			scriptables.Set(source, modifiableInstance);
			scriptables.Set(modifiableInstance, modifiableInstance);
			return modifiableInstance;
		}

		public static bool Contains<E>(IEnumerable<E> items, E item) where E : ScriptableObject {
			var modificationSafeItem = GetModifiableInstance(item);
			return items.Any(t => modificationSafeItem == GetModifiableInstance(t));
		}

		public static bool Equals<E>(E first, E second) where E : ScriptableObject => GetModifiableInstance(first) == GetModifiableInstance(second);
	}
}