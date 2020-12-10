using UnityEngine;

namespace Utils.Libraries {
	[CreateAssetMenu(menuName = "Constants/Libraries/Audio")]
	public class AudioClipLibrary : Library<AudioClip> {
		protected override string GetNonExistingWarningMessage(string key) => $"No clip value for the key {key} in the audio clip library {name}";
	}
}