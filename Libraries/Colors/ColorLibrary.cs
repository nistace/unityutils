using UnityEngine;

namespace NiUtils.Libraries {
	[CreateAssetMenu(menuName = "Constants/Libraries/Colors")]
	public class ColorLibrary : Library<Color> {
		protected override string GetNonExistingWarningMessage(string key) => $"No color value for the key {key} in the color library {name}";
	}
}