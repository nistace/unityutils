using UnityEditor;
using UnityEngine;
using Utils.Extensions;

namespace Utils.Libraries {
	[CreateAssetMenu(menuName = "Constants/Libraries/Sprites")]
	public class SpriteLibrary : Library<Sprite> {
		protected override string GetNonExistingWarningMessage(string key) => $"No sprite value for the key {key} in the sprite library {name}";

#if UNITY_EDITOR

		[MenuItem("Tools/Libraries/Textures")]
		public static void ToolsLoad() {
			var libraries = Resources.LoadAll<SpriteLibrary>("Libraries");
			libraries.ForEach(EditorUtility.SetDirty);
			AssetDatabase.FindAssets("t:sprite", new[] {"Assets/Textures/GameComponents"}).ForEach(t => AddToLibrariesFromAssetGuid(t, "Assets/Textures/GameComponents/", libraries));
			AssetDatabase.SaveAssets();
		}

#endif
	}
}