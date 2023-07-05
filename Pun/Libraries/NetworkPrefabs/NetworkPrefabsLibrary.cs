using NiUtils.Extensions;
using NiUtils.Libraries;
using UnityEditor;
using UnityEngine;

namespace NiUtils.Pun.Libraries.Network {
	[CreateAssetMenu(menuName = "Constants/Libraries/Network prefabs")]
	public class NetworkPrefabsLibrary : Library<GameObject> {
		protected override string GetNonExistingWarningMessage(string key) => $"No object value for the key {key} in the network prefabs library {name}";

#if UNITY_EDITOR

		[MenuItem("Tools/Libraries/Networed Prefabs")]
		public static void ToolsLoad() {
			var libraries = Resources.LoadAll<NetworkPrefabsLibrary>("Libraries");
			libraries.ForEach(EditorUtility.SetDirty);
			AssetDatabase.FindAssets("t:gameObject", new[] { "Assets/Prefabs/Networked" }).ForEach(t => AddToLibrariesFromAssetGuid(t, "Assets/Prefabs/Networked/", libraries));
			AssetDatabase.SaveAssets();
		}

#endif
	}
}