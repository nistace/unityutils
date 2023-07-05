using NiUtils.Extensions;
using UnityEditor;
using UnityEngine;

namespace NiUtils.Libraries {
	[CreateAssetMenu(menuName = "Constants/Libraries/Audio")]
	public class AudioClipLibrary : Library<AudioClip> {
		protected override string GetNonExistingWarningMessage(string key) => $"No clip value for the key {key} in the audio clip library {name}";

#if UNITY_EDITOR

		[MenuItem("Tools/Libraries/Audio")]
		public static void ToolsLoad() {
			var libraries = Resources.LoadAll<AudioClipLibrary>("Libraries");
			libraries.ForEach(EditorUtility.SetDirty);
			AssetDatabase.FindAssets("t:audioClip", new[] { "Assets/Audio/Music" }).ForEach(t => AddToLibrariesFromAssetGuid(t, "Assets/Audio/", libraries));
			AssetDatabase.FindAssets("t:audioClip", new[] { "Assets/Audio/Sfx" }).ForEach(t => AddToLibrariesFromAssetGuid(t, "Assets/Audio/Sfx/", libraries));
			AssetDatabase.FindAssets("t:audioClip", new[] { "Assets/Audio/Voices" }).ForEach(t => AddToLibrariesFromAssetGuid(t, "Assets/Audio/Voices/", libraries));
			AssetDatabase.SaveAssets();
		}

#endif
	}
}