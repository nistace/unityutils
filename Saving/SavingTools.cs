using UnityEditor;
using UnityEngine;

namespace Utils.Saving {
	public static class SavingTools {
#if UNITY_EDITOR

		[MenuItem("Tools/Saving/Open persistent data folder")]
		public static void ToolsOpenPersistentDataFolder() {
			System.Diagnostics.Process.Start("explorer.exe", $"/select,{Application.persistentDataPath.Replace(@"/", @"\")}\\");
		}
#endif
	}
}