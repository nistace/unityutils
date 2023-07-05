using NiUtils.Extensions;
using UnityEditor;
using UnityEngine;

namespace NiUtils.Libraries {
	[CreateAssetMenu(menuName = "Constants/Libraries/Particles")]
	public class ParticlesLibrary : Library<ParticleSystem> {
		protected override string GetNonExistingWarningMessage(string key) => $"No particle value for the key {key} in the particles library {name}";

#if UNITY_EDITOR

		[MenuItem("Tools/Libraries/Particles")]
		public static void ToolsLoad() {
			var libraries = Resources.LoadAll<ParticlesLibrary>("Libraries");
			libraries.ForEach(EditorUtility.SetDirty);
			AssetDatabase.FindAssets("t:prefab", new[] { "Assets/Prefabs/FX" }).ForEach(t => AddComponentToLibrariesFromAssetGuid(t, "Assets/Prefabs/FX/", libraries));
			AssetDatabase.SaveAssets();
		}

#endif
	}
}