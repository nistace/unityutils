using NiUtils.Extensions;
using UnityEngine;

namespace NiUtils.Libraries {
	[CreateAssetMenu]
	public class LibraryBuildData : ScriptableObject {
		[SerializeField] protected ColorLibrary[]     _colorLibraries;
		[SerializeField] protected CursorLibrary[]    _cursorLibraries;
		[SerializeField] protected AudioClipLibrary[] _audioClipLibraries;
		[SerializeField] protected ParticlesLibrary[] _particlesLibraries;
		[SerializeField] protected SpriteLibrary[]    _spriteLibraries;

		public void Build() {
			_colorLibraries.ForEach(Colors.LoadLibrary);
			_cursorLibraries.ForEach(CursorManager.LoadLibrary);
			_audioClipLibraries.ForEach(AudioClips.LoadLibrary);
			_particlesLibraries.ForEach(Particles.LoadLibrary);
			_spriteLibraries.ForEach(Sprites.LoadLibrary);
		}
	}
}