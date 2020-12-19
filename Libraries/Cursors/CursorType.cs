using System.Collections.Generic;
using UnityEngine;

namespace Utils.Libraries {
	[CreateAssetMenu(menuName = "Constants/Cursor")]
	public class CursorType : ScriptableObject {
		[SerializeField] protected Texture2D[] _textures;
		[SerializeField] protected Vector2     _hotspot;
		[SerializeField] protected float       _animationTick = .1f;
		[SerializeField] protected Color       _color         = Color.white;

		public Texture2D this[int frameIndex] => _textures[frameIndex];
		public Vector2                  hotspot       => _hotspot;
		public float                    animationTick => _animationTick;
		public IReadOnlyList<Texture2D> frames        => _textures;
		public int                      frameCount    => _textures.Length;
		public Color                    color         => _color;
	}
}