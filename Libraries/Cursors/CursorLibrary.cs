using UnityEditor;
using UnityEngine;
using Utils.Extensions;

namespace Utils.Libraries {
	[CreateAssetMenu(menuName = "Constants/Libraries/Cursor")]
	public class CursorLibrary : Library<CursorType> {
		protected override string GetNonExistingWarningMessage(string key) => $"No clip value for the key {key} in the cursor library {name}";

#if UNITY_EDITOR

		[MenuItem("Tools/Libraries/Cursors")]
		public static void ToolsLoad() {
			var libraries = Resources.LoadAll<CursorLibrary>("Libraries");
			libraries.ForEach(EditorUtility.SetDirty);
			AssetDatabase.FindAssets("t:cursorType", new[] {"Assets/Resources/Libraries/Cursors"}).ForEach(t => AddToLibrariesFromAssetGuid(t, "Assets/Resources/Libraries/Cursors/", libraries));
			AssetDatabase.SaveAssets();
		}

#endif
	}
}