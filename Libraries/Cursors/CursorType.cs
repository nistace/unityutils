using UnityEngine;

namespace Utils.Libraries {
	[CreateAssetMenu(menuName = "Constants/Cursor")]
	public class CursorType : ScriptableObject {
		[SerializeField] protected Texture2D[] _textures;
		[SerializeField] protected Vector2     _hotspot;
		[SerializeField] protected float       _animationTick = .1f;

		public int frameCount => _textures.Length;
		public Texture2D this[int frameIndex] => _textures[frameIndex];
		public Vector2 hotspot       => _hotspot;
		public float   animationTick => _animationTick;
	}
}