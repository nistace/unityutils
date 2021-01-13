using UnityEngine;

namespace Utils.Libraries {
	[CreateAssetMenu(menuName = "Constants/Libraries/Particles")]
	public class ParticlesLibrary : Library<ParticleSystem> {
		protected override string GetNonExistingWarningMessage(string key) => $"No color value for the key {key} in the particles library {name}";
	}
}