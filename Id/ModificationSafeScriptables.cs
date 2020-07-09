using System.Collections.Generic;
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
	}
}