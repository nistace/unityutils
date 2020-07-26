using UnityEngine;

[CreateAssetMenu(menuName = "Constants/Libraries/Cursor")]
public class CursorLibrary : Library<CursorType> {
	protected override string GetNonExistingWarningMessage(string key) => $"No clip value for the key {key} in the cursor library {name}";
}