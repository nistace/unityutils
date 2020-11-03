using UnityEngine;

[CreateAssetMenu(menuName = "Constants/Libraries/Network prefabs")]
public class NetworkPrefabsLibrary : Library<GameObject> {
	protected override string GetNonExistingWarningMessage(string key) => $"No object value for the key {key} in the network prefabs library {name}";
}