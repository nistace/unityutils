using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utils.Id.Editor {
	public static class DataIdTools {
		[MenuItem("Tools/DataId/Check uniqueness")]
		public static void Check() {
			var identities = Resources.LoadAll<DataMonoBehaviour>("").Cast<IData>().Union(Resources.LoadAll<DataScriptableObject>("")).ToList();
			identities.Sort((t, u) => t.id - u.id);
			var countErrors = 0;
			for (var i = 0; i < identities.Count - 1; ++i) {
				if (identities[i].id != identities[i + 1].id) continue;
				countErrors++;
				Debug.LogError("Two objects with the same id: " + identities[i] + " and " + identities[i + 1] + ". ID " + identities[i].id);
			}
			if (countErrors == 0) Debug.Log("No ID error found.");
			else Debug.LogWarning(countErrors + " errors found.");
		}

		[MenuItem("Tools/DataId/Get max ID")]
		public static void Max() => Debug.Log("Max ID: " + Resources.LoadAll<DataMonoBehaviour>("").Cast<IData>().Union(Resources.LoadAll<DataScriptableObject>("")).Max(t => t.id));
	}
}