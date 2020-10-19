using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Constants/Libraries/Sprites")]
public class SpriteLibrary : Library<Sprite> {
	protected override string GetNonExistingWarningMessage(string key) => $"No sprite value for the key {key} in the sprite library {name}";

}